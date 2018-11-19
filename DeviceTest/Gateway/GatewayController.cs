using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceTest.Center;
using JK.Channels.UDP;
using System.Threading;
using DeviceTest.Manager;
using JK.Channels.HttpUtils;
using JK.Channels.Base;
using Newtonsoft.Json;

namespace DeviceTest.Center
{


    public class ScanResponse
    {
        public int Port { get; set; }
        public string IP { get; set; }
        public string Mac { get; set; }
        public string Ver { get; set; }
        public string Model { get; set; }      
    }
    
    public enum ScanOperate
    { 
        StartScan = 0,
        StopScan = 1,
        ScanResponse =2,
    }

    public class GatewayController : DeviceTest.Manager.Manager
    {
        public const int GATEWAY_CONTROLLER_EVENT = 20000;

        public const int GATEWAY_CONTROLLER_SCAN_EVENT  = 20001;

        public const string PRODUCT_TYPE_GATEWAY = "1";
        public TestManager manager { get; set; }
        public GatewayNodes gatewayNodes { get; set; }
        public List<Gateway> gatewayItems { get; set; }
        public UDPClientChannel UDPClient { get; set; }

        private Thread scanProcessor;
        private Boolean terminated { get; set; }
        private int scanInterval { get;set;}
        private DateTime startScanDatetime { get; set; }

        public GatewayController(TestManager manager):
            base()
        {
            this.manager = manager;
            this.manager.httpController.AttachObserver(this.observer.Update);
            this.gatewayNodes = new GatewayNodes();
            this.gatewayItems = new List<Gateway>();
            this.UDPClient = new UDPClientChannel("GatewayScan");
            this.UDPClient.AttachObserver(this.observer.Update);
            this.scanInterval = 3000;
        }

        public override void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
            this.UDPClient.LoadFromFile(fileName);
            this.UDPClient.Open();
            this.UDPClient.StartAsyncReceiveData();
           
        }

        public override void LoadFromFile(string fileName)
        {
            base.LoadFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
 	         
        }

        private Boolean ScanTimeout()
        {
            TimeSpan span = DateTime.Now - this.startScanDatetime;
            Boolean result =  (span.TotalMilliseconds >= this.scanInterval);
            if (result)
            {
                this.startScanDatetime = DateTime.Now;
            }
            return result;
        }
        public void StartScan()
        {
            this.terminated = false;
            this.scanProcessor = new Thread(new ThreadStart(this.ScanDeviceInLocalNetwork));
            this.scanProcessor.IsBackground = true;
            this.startScanDatetime = DateTime.Now;
            this.scanProcessor.Start();     
        }

        public void StopScan()
        {
            this.terminated = true;
        }
        private void ScanDeviceInLocalNetwork()
        { 
            while(!this.terminated)
            {
                Thread.Sleep(1);
                if (this.ScanTimeout())
                {
                    this.ScanNetWork();
                }             
            }       
        }


        public void ScanNetWork()
        {
            ScanNetWorkCommand command = new ScanNetWorkCommand();
            this.UDPClient.SendASCIIData(command.Pack());              
        }

        public string GetGatewayType(string type)
        {
            return this.gatewayNodes.GetNameByType(type);
        }

        public string GetGatewayModel(string type)
        {
            return this.gatewayNodes.GetModelByType(type);
        }

        public void ProcessHttpControllerResponseRefreshGatewayNodes(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            string lastMessage = "";
            if (responseResult == ResponseResult.Ok)
            {
                try
                {
                    this.gatewayNodes = JsonConvert.DeserializeObject<GatewayNodes>((string)result);
                    lastMessage = "获取网关类型列表成功";
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    lastMessage = "解析网关类型列表失败";
                }
            }
            lastMessage = "获取网关类型列表失败";
            this.Notify(GATEWAY_CONTROLLER_EVENT, HttpRequestType.RefreshGatewayNodes.ToString(), responseResult, lastMessage);
        }

        public void ProcessUDPChannelReceivedResponse(string flag, object result, string message)
        {
            ChannelControl channelCtrl = (ChannelControl)Enum.Parse(typeof(ChannelControl), flag);
            if ((channelCtrl == ChannelControl.Receive) && ((ChannelResult)result == ChannelResult.OK))
            {
                try
                {
                    ScanResponse response = JsonConvert.DeserializeObject<ScanResponse>(message);
                    foreach(Gateway gateway in this.gatewayItems)
                    {
                        if (response.Mac == gateway.deviceId)
                        {
                            gateway.model = response.Model;
                            gateway.softwareVersion = response.Ver;
                            gateway.port = response.Port;
                            gateway.address = gateway.address;
                            string model = this.gatewayNodes.GetModelByType(gateway.deviceType);
                            if (model != response.Model)
                            {
                                gateway.resultStatus = "类型不匹配";
                            }
                            else
                            {
                                gateway.resultStatus = "类型匹配";
                            }
                            this.Notify(GATEWAY_CONTROLLER_SCAN_EVENT,ScanOperate.ScanResponse.ToString(), gateway, "设备"+gateway.discrible+"扫描返回");
                            return;
                        }
                    }                                 
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }           
        
        
        }

        public void ProcessHttpControllerResponsePostGatewayBindData(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            string lastMessage = "";
            if (responseResult == ResponseResult.Ok)
            {
                try
                {

                    foreach (Gateway item in this.gatewayItems)
                    {
                        item.resultStatus = "绑定成功";
                    }
                    lastMessage = "绑定网关成功";
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    lastMessage = "解析绑定网关返回值失败";
                }
            }
            lastMessage = "绑定网关失败";
            this.Notify(GATEWAY_CONTROLLER_EVENT, HttpRequestType.PostGatewayBindData.ToString(), responseResult, lastMessage);
        }

        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            switch (notifyEvent)
            {
                case (int)HttpRequestType.RefreshGatewayNodes:
                    {
                        this.ProcessHttpControllerResponseRefreshGatewayNodes(flag, result, message);
                        break;
                    }
                case (int)HttpRequestType.PostGatewayBindData:
                    {
                        this.ProcessHttpControllerResponsePostGatewayBindData(flag, result, message);
                        break;
                    }
                case UDPClientChannel.UDP_DATA_EVENT:
                    {
                        this.ProcessUDPChannelReceivedResponse(flag, result, message);
                        break;
                    
                    }
            }
                      
        }

        public void Remove(string address)
        {
            foreach (Gateway item in this.gatewayItems)
            {
                if (item.deviceId == address)
                {
                    this.gatewayItems.Remove(item);
                    break;
                }
            }
        }
        public void AppendGatewayItem(string address)
        {
            foreach (Gateway item in this.gatewayItems)
            {
                if (item.deviceId == address)
                {
                    return;
                }
            }
            Gateway newItem = new Gateway();
            newItem.deviceId = address;
            this.gatewayItems.Add(newItem);
        }






        public void RefreshGatewayNodes()
        {
            this.manager.httpController.updated = false;
            this.manager.httpController.useBaseUrl = true;
            this.manager.httpController.commandUrl = StringResource.REFRESH_GATEWAY_NODES_URL;
            this.manager.httpController.commandFlag = (int)HttpRequestType.RefreshGatewayNodes;
            this.manager.httpController.paramList.Clear();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>("type", PRODUCT_TYPE_GATEWAY);
            this.manager.httpController.paramList.Add(pair);
            this.manager.httpController.Get();
        }

        public void PostGatewayBind(string productType)
        {
            string addressList = "";
            for(int i=0 ;i< this.gatewayItems.Count;i++)
            {
                Gateway item = this.gatewayItems[i];
                if(i!=this.gatewayItems.Count-1)
                {
                    addressList += item.deviceId + ",";
                }
                else
                {
                    addressList += item.deviceId;
                }
                item.deviceType = productType;
            }
            this.PostGatewayBind(addressList, productType);
        }

        public void PostGatewayBind(string address,string productType)
        {
            this.manager.httpController.updated = false;
            this.manager.httpController.useBaseUrl = true;
            this.manager.httpController.commandUrl = StringResource.GATEWAY_TYPE_BIND_URL;
            this.manager.httpController.commandFlag = (int)HttpRequestType.PostGatewayBindData;
            this.manager.httpController.paramList.Clear();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>("index", productType);
            this.manager.httpController.paramList.Add(pair);

            pair = new KeyValuePair<string, string>("address", address);
            this.manager.httpController.paramList.Add(pair);
            this.manager.httpController.Post();            
        }
    }
}
