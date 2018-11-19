using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JK.Channels.HttpUtils;
using DeviceTest.Base;
using Newtonsoft.Json;
using JK.Program.Pattern;
using JK.Libs.Storage;
using JK.Channels.TCP;
using JK.Channels.UDP;
using JK.Channels.Base;
using DeviceTest.Center;
using JK.Libs.Utils;



namespace DeviceTest.Manager
{
    public enum HttpRequestType
    { 
        Request            = 1000,
        RefreshDeviceNodes = 1001,
        PostDeviceRecordData= 1002,
        RefreshGatewayNodes = 1010,
        PostGatewayBindData = 1011,

        QueryRegistedDeviceData = 1020,       
        QueryProductNodes = 1021,
        QuerySerialNumberListData = 1022,
    }


    public enum WorkSpace
    {
        Zigbee = 1,
        Gateway = 2,
        iLopGPRS = 3,
        iLopVoicePanel = 4,
    }

    public class Manager : Subject
    {
        public string section { get; set; }
        public string pathFileName { get; set; }

        public Manager()
        {
            this.section = "";
            this.pathFileName = "";
        }

        public virtual void Initialize(string fileName)
        {
            return;
        }

        public virtual void Dispose()
        {
            return;
        }
        public  virtual void LoadFromFile(string fileName)
        {
            this.pathFileName = fileName;
            string[] list = IniFiles.GetAllSectionNames(fileName);
            if (!list.Contains(this.section))
            {
                this.SaveToFile(fileName);
            }
        }

        public virtual void LoadFromFile()
        {
            if (this.pathFileName != "")
            {
                this.LoadFromFile(this.pathFileName);
            }            
        }

        public virtual void SaveToFile(string fileName)
        {
            this.pathFileName = fileName;
            return;
        }

        public virtual void SaveToFile()
        {
            if (this.pathFileName != "")
            {
                this.SaveToFile(this.pathFileName);
            }
            else
            {
                this.SaveToFile(StringResource.ConfigFileName);
            }
        }

        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            return;
        }
    
    }


    public class UpgradeController
    {
        private const int  ERROR_CODE_OK                   = 0;
        private const int  ERROR_CODE_FAIL                 = 1;
        private const int  ERROR_CODE_UNKNOWN              = 2;

        private const int  RESULT_CODE_TO_UPGRADE          = 0;
        private const int  RESULT_CODE_NO_UPGRADE         = 1;

        public string section { get; set; }
        public Boolean checkedVersion { get; set; }
        public int errorCode { get; set; }
        public int result { get; set; }
        public DateTime startDateTime { get; set; }
        public UpgradeController()
        {
            this.checkedVersion = false;
            this.section = "CommandResult";
            this.startDateTime = DateTime.Now;
        }

        public Boolean  WaitTimeOut()
        {
            TimeSpan span = DateTime.Now - this.startDateTime;
            return span.TotalSeconds > 10;
        }

        public void LoadCheckResult(string fileName)
        {
            this.errorCode = IniFiles.GetIntValue(fileName, this.section, "errorCode", ERROR_CODE_UNKNOWN);
            this.result = IniFiles.GetIntValue(fileName, this.section, "result", RESULT_CODE_NO_UPGRADE);
        }

        public Boolean CheckVersion()
        { 
            this.LoadCheckResult(StringResource.ConfigFileName);
            return (this.errorCode == ERROR_CODE_OK) && (this.result == RESULT_CODE_TO_UPGRADE);
        }


        public void CheckToUpgrade()
        {
            if (!this.checkedVersion)
            {
                Process commandProcess = new Process();
                commandProcess.StartInfo.FileName = StringResource.AutoUpgraderFileName;
                commandProcess.StartInfo.Arguments = "version";
                commandProcess.StartInfo.CreateNoWindow = true;
                commandProcess.StartInfo.UseShellExecute = false;
                try
                {
                    commandProcess.Start();
                    this.checkedVersion = true;

                }
                catch (System.InvalidOperationException)
                {
                }
            }
        }
    }


    public class TestManager : Manager   
    {
        public DeviceItem activeDevice = null;
        //Http请求管理
        public HttpController httpController { get; set; }
        

        public DeviceController deviceController { get; set; }
        public TestControllerPool testControllerPool { get; set; }
        public ProjectService projectService { get; set; }
        public LabelController labelController { get;set; }
        public GatewayController gatewayController { get; set; }

        public UpgradeController upgradeController { get; set; }

        public WorkSpace workSpace { get; set; }

        public TestManager():
            base()
        {
            this.section = "TestManager";
            this.httpController = new HttpController();
            

            this.deviceController = new DeviceController(this);
            
            this.testControllerPool = new TestControllerPool(this);
            this.projectService = new ProjectService();
            this.labelController = new LabelController();
            this.gatewayController = new GatewayController(this);
            this.upgradeController = new UpgradeController();
                        
            this.httpController.AttachObserver(this.observer.Update);
            this.deviceController.AttachObserver(this.observer.Update);
            this.labelController.AttachObserver(this.observer.Update);
            

            this.activeDevice = null;
            this.workSpace = WorkSpace.Zigbee;

        }
        public void UpdateActiveDevice(DeviceItem device)
        {
            this.activeDevice = device;
        }




        public Boolean InvalidWorkPath()
        {
            return Utils.ContainChinese(StringResource.AppPath);
        }
        public void PermitJoining()
        {
            if (!this.deviceController.gateway.permitJoining)
            {
                this.StartPermitJoining();
            }
            else
            {
                this.StopPermitJoining();
            }            
        }
        public void PrintLabelContext()
        {
            
            if (this.activeDevice != null)
            {                
                this.labelController.PrintLabelContext(this.activeDevice.labelData);
                this.activeDevice.IncPrintLabelSum();
                this.projectService.activeProject.IncSerialNumber();
            }
        
        }

        public override void Dispose()
        {
            this.deviceController.Dispose();
        }
        public void GetInitializeInfo()
        {
            this.deviceController.GetGatewayAllAttribute();
            this.deviceController.GetDeviceList();              
        }

        public void DeviceQuery(BaseData data)
        {
            List<BaseData> datas = new List<BaseData>();
            datas.Add(data);
            this.deviceController.DeviceQuery(datas);            
        }


        public void DeviceControl(BaseData data)
        {
            List<BaseData> datas = new List<BaseData>();
            datas.Add(data);
            this.deviceController.DeviceControl(datas); 
        }

        public void StartPermitJoining()
        {
            this.deviceController.AllowGatewayAdd(255);       
        }

        public void StopPermitJoining()
        {
            this.deviceController.AllowGatewayAdd(0);
        }
        public override void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
                       
            this.deviceController.Initialize(fileName);
            
            this.projectService.Initialize(fileName);
            this.labelController.Initialize(fileName);
            this.gatewayController.Initialize(fileName);
        }
        public override void LoadFromFile(string fileName)
        {
            this.httpController.baseUrl = IniFiles.GetStringValue(fileName, this.section, "BaseURL", StringResource.DEFAULT_BASE_URL);
            base.LoadFromFile(fileName);
        }

        public  override void SaveToFile(string fileName)
        {
            IniFiles.WriteStringValue(fileName, this.section, "BaseURL", this.httpController.baseUrl );  
        }


        

        public void OperateTask()
        {          

            if (this.deviceController.TCPClient.ConnectTimeout() && !this.deviceController.TCPClient.Active())
            {
                this.deviceController.TCPClient.OpenSync();
            }


            if (this.deviceController.gateway.BeatHeartTimeout())
            {
                this.deviceController.BeatHeart();
            }
            else
            {
                if (this.deviceController.gateway.InvalidConnected() && this.deviceController.gateway.active)
                {
                    this.deviceController.gateway.active = false;
                    this.deviceController.TCPClient.Close();
                }      
            }
            
        }



        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            switch (notifyEvent)
            {
                case (int)DeviceController.PRINT_DEVICE_LABEL:
                    {
                        if (result != null)
                        {
                            if (this.projectService.GenerateLabelContent((DeviceItem)result))
                            {
                                this.labelController.PrintLabelContext(((DeviceItem)result).labelData);
                            }
                        }
                        break;
                    }

                case TCPClientChannel.TCP_CONTROL_EVENT:
                    {
                        ChannelControl control = (ChannelControl)Enum.Parse(typeof(ChannelControl), flag);
                        if (control == ChannelControl.Open)
                        {
                            if ((ChannelResult)result == ChannelResult.OK)
                            {
                                this.GetInitializeInfo();
                            }
                        }
                        break;
                    }
                case UDPClientChannel.UDP_DATA_EVENT:
                    {
                        ChannelControl channelCtrl = (ChannelControl)Enum.Parse(typeof(ChannelControl), flag);
                        if ((channelCtrl == ChannelControl.Receive) && ((ChannelResult)result == ChannelResult.OK))
                        {
                            try
                            {
                                this.projectService.responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(message);
                                this.Notify(ProjectService.PROJECT_LABEL_PRINT_FINISH_EVENT, "", null, "");
                            }
                            catch (Newtonsoft.Json.JsonReaderException)
                            {

                            }
                        }
                        break;
                    }

            }

            this.Notify(notifyEvent,flag,result,message);
        }


        public void AddDeviceTestResultRecord()
        {
            if (this.activeDevice != null)
            {
                this.AddDeviceTestResultRecord(this.activeDevice);
            }
        }

        public void AddDeviceTestResultRecord(DeviceItem device)
        {
            this.httpController.updated = false;
            this.httpController.useBaseUrl = true;
            this.httpController.commandUrl = StringResource.DEVICE_RECORED_ADD_URL;
            this.httpController.commandFlag = (int)HttpRequestType.PostDeviceRecordData;
            this.httpController.paramList.Clear();
            KeyValuePair<string, string> param = new KeyValuePair<string, string>("model_id",device.modelId);
            this.httpController.paramList.Add(param);

            if (device.serialCode != "")
            {
                param = new KeyValuePair<string, string>("serial_code", device.serialCode);
                this.httpController.paramList.Add(param);
            }

            param = new KeyValuePair<string, string>("remark", this.projectService.activeProjectName);
            this.httpController.paramList.Add(param);

            param = new KeyValuePair<string, string>("identity", device.deviceId);
            this.httpController.paramList.Add(param);

            param = new KeyValuePair<string, string>("version", device.softwareVersion);
            this.httpController.paramList.Add(param);

            param = new KeyValuePair<string, string>("status", ((int)device.testResult).ToString());
            this.httpController.paramList.Add(param);

            param = new KeyValuePair<string, string>("operation", ((int)device.operation).ToString());
            this.httpController.paramList.Add(param);

            param = new KeyValuePair<string, string>("serial_number", device.serialNumber);
            this.httpController.paramList.Add(param);

            this.httpController.Post(); 
        }

        

    }
}
