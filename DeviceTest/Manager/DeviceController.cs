using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JK.Program.Pattern;
using JK.Libs.Storage;
using JK.Channels.TCP;
using JK.Channels.Base;
using DeviceTest.Base;
using Newtonsoft.Json;
using DeviceTest.Center;
using System.IO;
using System.Text.RegularExpressions;
using JK.Libs.Utils;
using JK.Channels.Serial;
using DeviceTest.Projects;
using JK.Channels.HttpUtils;




namespace DeviceTest.Manager
{

    public enum DeviceOperateEvent
    { 
        ListAll = 1,
        ListAdd = 2,
        ListDelete = 3,
        ListUpdate = 4,
        Refresh = 5,
        Property = 6,
        PrintLabel=7,
    }




    public class DeviceController:Manager
    {
        public const int REFRESH_DEVICE_LIST_EVENT = 4000;
        public const int REFRESH_DEVICE_EVENT = 4001;
        public const int REFRESH_GATEWAY_EVENT = 4002;
        public const int PRINT_DEVICE_LABEL = 4010;

        private HttpController httpController { get; set; }
        private TestManager manager { get; set; }
        public TCPClientChannel TCPClient { get; set; }

        public SerialChannel serialChannel { get; set; }

        public List<DeviceItem> deviceList { get; set; }

        //设备profile文件管理
        public DeviceConfigList deviceConfigList { get; set; }
        //云端设备配置信息
        public DeviceNodes deviceNodes { get; set; }
        public DeviceCodeList deviceCodeList { get; set; }
        public ProductNodes productNodes { get; set; }
        public iLopController iLopController { get; set; }

        public Gateway gateway { get; set; }

        
        public DeviceController(TestManager manager) :
            base()
        {
            this.section = "DeviceController";
            this.manager = manager;
            this.httpController = manager.httpController;
            this.httpController.AttachObserver(this.observer.Update);
            this.deviceNodes = new DeviceNodes();
            this.deviceConfigList = new DeviceConfigList();
            this.productNodes = new ProductNodes();

            this.TCPClient = new TCPClientChannel();
            this.TCPClient.AttachObserver(this.observer.Update);
            this.deviceList = new List<DeviceItem>();
            this.gateway = new Gateway();
            this.serialChannel = new SerialChannel();
            this.serialChannel.AttachObserver(this.observer.Update);
            this.deviceCodeList = new DeviceCodeList();
            this.iLopController = new iLopController(this);          
        }


        public override void Initialize(string filename)
        {
            this.LoadFromFile(filename);
            this.TCPClient.LoadFromFile(filename);
            this.TCPClient.StartAsyncReceiveData();
            this.deviceConfigList.Initialize();
            this.serialChannel.LoadFromFile(filename);
            this.serialChannel.Open();
            this.RefreshDeviceNodes();
            this.iLopController.Initialize(filename);
        }

        public void UpdateToDeviceNodes()
        {
            foreach (DeviceNode node in this.deviceNodes.rows)
            {
                foreach(ProductNode productNode in this.productNodes.products)
                {
                    if (productNode.mcode == node.serialCode)
                    {
                        node.productKey = productNode.productKey;
                        break;
                    }
                }
            }

            foreach (DeviceItem item in this.deviceList)
            {
                foreach (DeviceNode node in this.deviceNodes.rows)
                {
                    if (item.serialCode == node.serialCode)
                    {
                        item.productKey = node.productKey;
                    }
                }
            }
        }

        public Boolean AppendToDeviceList(DeviceItem device)
        {
            if (this.manager.projectService.activeProject != null)
            {              
                if (this.manager.projectService.activeProject.serialCode == "None")
                {
                    this.deviceList.Add(device);
                    return true;
                }
                else if (this.manager.projectService.activeProject.activeDeviceNode!=null)
                {
                    if (device.modelId == this.manager.projectService.activeProject.activeDeviceNode.firmware.modelId)
                    {
                        device.serialCode = this.manager.projectService.activeProject.activeDeviceNode.serialCode;
                        this.deviceList.Add(device);
                        return true;
                    }                  
                }               
            }
            return false;
            
        }
        public void UpdateWithDeviceItem(DeviceItem item)
        {
            if (!item.updateConfig)
            {
                foreach (DeviceConfig config in this.deviceConfigList.items)
                {
                    if (config.modelId.Contains(item.modelId))
                    {
                        item.modelId = config.modelId;
                        item.Assign(config);
                        item.updateConfig = true;
                    }
                }
            }
            if (!item.updateWithNode)
            {
                string projectFlag = "";
                if (this.manager.projectService.activeProject != null)
                {
                    projectFlag = this.manager.projectService.activeProject.serialCode;
                }
                foreach (DeviceNode node in this.deviceNodes.rows)
                {

                    if (projectFlag != "")
                    {
                        if ((node.serialCode == item.serialCode ||node.firmware.modelId.Contains(item.modelId) )&& (node.flag == projectFlag))
                        {
                            item.modelId = node.firmware.modelId;
                            item.name = node.name;
                            item.model = node.model;
                            //item.serialCode = node.serialCode;
                            item.productKey = node.productKey;
                            item.lastSoftwareVersion = node.firmware.version;
                            item.updateWithNode = true;
                        }
                    }
                }
            }

        }

        private void UpdateDeviceConfig()
        {
            foreach (DeviceNode node in this.deviceNodes.rows)
            {
                if (node.config != null)
                {
                    this.deviceConfigList.UpdateConfig(node.config);
                }
            }

        }

        public DeviceItem AddNewDeviceByImportText(string importText)
        {
            if (importText != "")
            {                
                if (importText.Contains("https://"))
                {
                    DeviceItem device = new DeviceItem();
                    string[] list = importText.Split('&');
                    string serialNumber = "";
                    string value = "";
                    foreach (string item in list)
                    {
                        string line = item + ";";
                        if (Utils.GetValueAnd("sn=", ";", line, ref serialNumber))
                        {
                            continue;
                        }
                        if (Utils.GetValueAnd("mcode=", ";", line, ref value))
                        {
                            device.serialCode = value;
                            continue;
                        }
                    }
                    //FIXME 获取MAC地址 deviceId
                    this.UpdateWithDeviceItem(device);
                    device.deleted = false;
                    this.AddDeviceItem(device);                   
                    this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListAdd.ToString(), device, "");
                    return device;
                }
            }
            return null;
        }

        public void ProcessHttpControllerResponseQueryRegistedDeviceData(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            if (responseResult == ResponseResult.Ok)
            {
                try
                {
                    this.iLopController.deviceCodeItem = JsonConvert.DeserializeObject<DeviceCodeItem>((string)result);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }        
        }

        public void ProcessHttpControllerResponseQuerySerialNumberListData(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            if (responseResult == ResponseResult.Ok)
            {
                try
                {
                    this.deviceCodeList = JsonConvert.DeserializeObject<DeviceCodeList>((string)result);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }                    
        }

        public void ProcessHttpControllerResponseQueryProductNodes(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            if (responseResult == ResponseResult.Ok)
            {
                try
                {
                    this.productNodes = JsonConvert.DeserializeObject<ProductNodes>((string)result);
                    this.UpdateToDeviceNodes();
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }        
        
        }

        public void ProcessHttpControllerResponseRefreshDeviceNodes(string flag, object result, string message)
        {
            ResponseResult responseResult = (ResponseResult)Enum.Parse(typeof(ResponseResult), flag);
            if (responseResult == ResponseResult.Ok)
            {
                try
                {
                    this.deviceNodes = JsonConvert.DeserializeObject<DeviceNodes>((string)result);
                    foreach (DeviceItem item in this.deviceList)
                    {
                        this.UpdateWithDeviceItem(item);
                    }
                    this.UpdateDeviceConfig();
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }
        }

        

        public void QueryRegistedDeviceData(string productKey)
        {
            this.iLopController.ClearDevice();
            this.iLopController.productKey  = productKey;
            this.iLopController.activeDevice.model = productKey;         
            this.httpController.updated = false;
            this.httpController.useBaseUrl = true;
            this.httpController.commandUrl = StringResource.QUERY_REGISTER_DEVICE_URL;
            this.httpController.commandFlag = (int)HttpRequestType.QueryRegistedDeviceData;
            this.httpController.paramList.Clear();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>("device_name", productKey);
            this.httpController.paramList.Add(pair);

            pair = new KeyValuePair<string, string>("code_type", "iLop");
            this.httpController.paramList.Add(pair);
            this.httpController.Get();            
        }

        public void QueryDeviceCodeList(string productKey,Boolean active,string startDate,string endDate)
        {
            this.httpController.updated = false;
            this.httpController.useBaseUrl = true;
            this.httpController.commandUrl = StringResource.QUERY_SERIAL_NUMBER_LIST_URL;
            this.httpController.commandFlag = (int)HttpRequestType.QuerySerialNumberListData;
            this.httpController.paramList.Clear();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>("product_key", productKey);
            this.httpController.paramList.Add(pair);
            string value = "1";
            if (!active)
                value = "0";
            pair = new KeyValuePair<string, string>("active", value);
            this.httpController.paramList.Add(pair);

            pair = new KeyValuePair<string, string>("start_date", startDate);
            this.httpController.paramList.Add(pair);

            pair = new KeyValuePair<string, string>("end_date", endDate);
            this.httpController.paramList.Add(pair);

            this.httpController.Get();                   
        }


        public void QueryProductNodes()
        {
            this.httpController.updated = false;
            this.httpController.useBaseUrl = false;
            this.httpController.commandUrl = StringResource.QUERY_PRODUCT_NODES_URL;
            this.httpController.commandFlag = (int)HttpRequestType.QueryProductNodes;
            this.httpController.paramList.Clear();
            this.httpController.Get();                
        }

        public void RefreshDeviceNodes()
        {
            this.httpController.updated = false;
            this.httpController.useBaseUrl = true;
            this.httpController.commandUrl = StringResource.REFRESH_DEVICE_NODES_URL;
            this.httpController.commandFlag = (int)HttpRequestType.RefreshDeviceNodes;
            this.httpController.paramList.Clear();
            this.httpController.Get();
        }

        public override void LoadFromFile(string fileName)
        {
            this.gateway.beatHeartSeconds = IniFiles.GetIntValue(fileName, this.section, "BeatHeartSeconds",60);
            this.gateway.activeSeconds = IniFiles.GetIntValue(fileName, this.section, "ActiveSeconds", 180);
            this.gateway.autoPermitJoining = IniFiles.GetBoolValue(fileName, this.section, "AutoPermitJoining", true);
            base.LoadFromFile(fileName);
        }

        public string exportTextHead
        {
            get
            {
                return "唯一标识(MAC)" + "," +
                        "名称" + "," +
                        "型号" + "," +
                        "modelId" + "," +
                        "版本状态" + "," +
                        "当前版本" + "," +
                        "SN" + "," +
                        "项目名称";
            }
        }

        public string DeviceItemExportText(DeviceItem device)
        { 
            if (device!=null)
            {
                return device.exportText+","+this.manager.projectService.activeProjectName;
            }
            return ""; 
        }

        public void SaveHistoryDevicesItemsWithProject()
        {
            string projectFolder = StringResource.MainDataPath  + this.manager.projectService.activeProjectName +"\\";
            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }
            string filePathName = projectFolder + StringResource.TestHistoryFileName;

            Boolean newFile = false;
            if (!File.Exists(filePathName))
            {
                File.Create(filePathName).Close();
                newFile = true;
            }
            string content = Utils.LoadFromFile(filePathName); ;
            string[] contentLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> items = new List<string>();

            StreamWriter streamWriter = new StreamWriter(filePathName, true);
            if (newFile)
            {
                streamWriter.WriteLine(this.exportTextHead);    
            }
            try
            {
                Boolean find = false;
                foreach (DeviceItem item in this.deviceList)
                {
                    find = false;
                    foreach (string line in contentLines)
                    {
                        if (line.IndexOf(item.deviceId) >= 0)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        string message = this.DeviceItemExportText(item);
                        if (message.Length > 0)
                        {
                            streamWriter.WriteLine(message);
                        }
                    }
                }
            }
            finally
            {
                streamWriter.Close();
            }
        }
        public void  SaveHistoryDeviceItems()
        {
            if(!Directory.Exists(StringResource.TestHistoryFolder))
            {
                    Directory.CreateDirectory(StringResource.TestHistoryFolder);                
            }
            string filePathName = StringResource.TestHistoryFolder + StringResource.TestHistoryFileName;

            if (!File.Exists(filePathName))
            {
                File.Create(filePathName).Close();
            }
            string content = Utils.LoadFromFile(filePathName); ;
            string[] contentLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> items = new List<string>();
            
            StreamWriter streamWriter = new StreamWriter(filePathName, true);
            try
            {
                Boolean find = false;
                foreach (DeviceItem item in this.deviceList)
                {
                    find = false;
                    foreach (string line in contentLines)
                    {
                        if (line.IndexOf(item.deviceId) >= 0)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        string message = this.DeviceItemExportText(item);
                        if(message.Length>0)
                        {
                            streamWriter.WriteLine(message);
                        }
                    }
                }
            }
            finally
            {
                streamWriter.Close();
            }
  


        }

        public override void SaveToFile(string fileName)
        {
            IniFiles.WriteIntValue(fileName, this.section, "BeatHeartSeconds", this.gateway.beatHeartSeconds);
            IniFiles.WriteIntValue(fileName, this.section, "ActiveSeconds", this.gateway.activeSeconds);
            IniFiles.WriteBoolValue(fileName, this.section, "AutoPermitJoining",  this.gateway.autoPermitJoining);
        }

        public DeviceItem GetByDeviceId(string deviceId)
        {
            DeviceItem item = null;
            for (int i = 0; i < this.deviceList.Count; i++)
            {
                if (this.deviceList[i].deviceId == deviceId)
                {
                    item = this.deviceList[i];
                    break;
                }
            }
            return item;
        }

        public override void Dispose()
        {
            this.TCPClient.StopReceiveData();
            //this.serialChannel.Close();         
        }



        private string Unpack(string receiveMessage)
        {
            char[] trims = new char[2];
            trims[0]= (char)BaseCommand.STX;
            trims[1]= (char)BaseCommand.ETX ;
            return receiveMessage.Trim(trims);
        }






        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            switch (notifyEvent)
            {
                case TCPClientChannel.TCP_DATA_EVENT:
                    {
                        message = this.Unpack(message);
                        this.ProcessTCPClientReceivedData(flag, result, message);
                        break;
                    }
                case TCPClientChannel.TCP_CONTROL_EVENT:
                    {
                        this.ProcessTCPClientControlResult(flag, result, message);
                        break;
                    }
                case SerialChannel.COM_DATA_EVENT:
                    {

                        this.ProcessSerialChannelReceiveResult(flag, result, message);
                        break;
                    }
                case (int)HttpRequestType.RefreshDeviceNodes:
                    {
                        this.ProcessHttpControllerResponseRefreshDeviceNodes(flag, result, message);
                        this.QueryProductNodes();
                        break;
                    }
                case (int)HttpRequestType.QueryProductNodes:
                    {
                        this.ProcessHttpControllerResponseQueryProductNodes(flag, result, message);
                        break;                        
                    }
                case (int)HttpRequestType.QueryRegistedDeviceData:
                    {
                        this.ProcessHttpControllerResponseQueryRegistedDeviceData(flag, result, message);
                        break;                        
                    }
                case (int)HttpRequestType.QuerySerialNumberListData:
                    {
                        this.ProcessHttpControllerResponseQuerySerialNumberListData(flag, result, message);
                        break;    
                    }
            }
            this.Notify( notifyEvent,  flag, result,  message);
        }


        private void ProcessTCPClientControlResult(string flag, object result, string message)
        {
            ChannelControl channelCtrl = (ChannelControl)Enum.Parse(typeof(ChannelControl),flag);            
            if (channelCtrl == ChannelControl.Open)
            {
                this.gateway.active = ((ChannelResult)result == ChannelResult.OK);
            }       
        }

        private void ProcessForiLopProject(string message)
        {
            int commandIndex = -1;

            Boolean  result = this.iLopController.ProcessCommandResponse(message, ref commandIndex);
            if (result & ( commandIndex != -1))
            {
                this.Notify(REFRESH_DEVICE_EVENT, DeviceOperateEvent.Refresh.ToString(), this.iLopController.activeDevice, this.iLopController.activeDevice.statusMessage);
            }

        }
      
        private void ProcessForLierdaProject(string message)
        {
            Boolean finished = false;
            string deviceId = "";
            string messageInfo = "";
            if (LierdaProject.GetResultWithLine(message, ref finished, ref deviceId, ref messageInfo))
            {
                DeviceItem device = this.GetByDeviceId(deviceId);
                if (device == null)
                {
                    device = LierdaProject.CreateNewLierdaDevice();
                    this.AppendToDeviceList(device);
                }

                device.deviceId = deviceId;
                device.online = true;
                device.registered = true;
                device.deleted = false;
                if (finished)
                {
                    device.testResult = TestResult.Pass;
                    this.Notify(PRINT_DEVICE_LABEL, DeviceOperateEvent.PrintLabel.ToString(), device, "正在打印标签");
                }
                device.statusMessage = messageInfo;
                this.UpdateWithDeviceItem(device);
                this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListAdd.ToString(), device, "");
            }           
         }

        public void PrintDeviceLabel(DeviceItem device)
        {
            this.Notify(PRINT_DEVICE_LABEL, DeviceOperateEvent.PrintLabel.ToString(), device, "正在打印标签");       
        }
        private void ProcessSerialChannelReceiveResult(string flag, object result, string message)
        {
            if ((flag == ChannelControl.Receive.ToString()) && ((ChannelResult)result == ChannelResult.OK) && message.Length > 0)
            {
                switch (Program.TestManager.workSpace)
                {
                    case WorkSpace.Zigbee:
                        {
                            this.ProcessForLierdaProject(message);
                            break;
                        }
                    case WorkSpace.iLopGPRS:
                        {
                            this.ProcessForiLopProject(message);
                            break;
                        }
                }
            }
        
        }
        private void ProcessTCPClientReceivedData(string flag, object result, string message)
        {
            if ((flag ==ChannelControl.Receive.ToString()) && ((ChannelResult)result == ChannelResult.OK) && message.Length>0)
            {
                try
                {
                    BaseContext context = JsonConvert.DeserializeObject<BaseContext>((string)message);

                    ControlCommand ctrlCommand = (ControlCommand)Enum.Parse(typeof(ControlCommand), context.Command);

                    switch (ctrlCommand)
                    {
                        case ControlCommand.BeatHeartResponse:
                            {
                                this.gateway.lastBeatHeartDatetime = DateTime.Now;
                                break;
                            }
                        case ControlCommand.Report:
                        {
                            try
                            {
                                CommandType commandType = (CommandType)Enum.Parse(typeof(CommandType), context.Type);
                                switch (commandType)
                                {
                                    case CommandType.DevList:
                                        {
                                            this.ProcessReportDeviceList(message);
                                            break;
                                        }
                                    case CommandType.Register:
                                        {
                                            this.ProcessReportDeviceRegister(message);
                                            break;
                                        }
                                    case CommandType.UnRegister:
                                        {
                                            this.ProcessReportDeviceUnRegister(message);
                                            break;
                                        }
                                    case CommandType.DevAttri:
                                        {
                                            this.ProcessReportDeviceAllAttributes(message);
                                            break;
                                        }
                                    case CommandType.Attribute:
                                        {
                                            this.ProcessReportDeviceAttribute(message);
                                            break;
                                        }
                                    case CommandType.OnOff:
                                        {
                                            this.ProcessReportDeviceOnlineStatus(message);
                                            break;
                                        }
                                    case CommandType.Event:
                                        {
                                            this.ProcessReportDeviceEvent(message);
                                            break;
                                        }
                                }
                            }
                            catch (Exception)
                            { 
                            
                            }
                            break;
                        }
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {

                }
            }
        }

        public void NotifyListUpdate(DeviceItem device)
        {
            if (device == null)
            {
                this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListAll.ToString(), null, "");
            }
            else
            {
                this.Notify(REFRESH_DEVICE_EVENT, DeviceOperateEvent.ListUpdate.ToString(), device, "");
            }
        }

        private void UnregisterDeviceItem(string deviceId)
        {
            foreach (DeviceItem item in this.deviceList)
            {
                if (item.deviceId == deviceId)
                {
                    item.deleted = true;
                    this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListDelete.ToString(), item, "");                                   
                    break;
                }
            }
            
        }

        private DeviceItem AddDeviceItem(BaseData data) 
        {
            DeviceItem item = new DeviceItem();
            item.modelId = data.ModelId;
            item.deviceId = data.DeviceId;
            item.UpdateProperty(data);
            if (this.AppendToDeviceList(item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }
        private Boolean AddDeviceItem(DeviceItem item)
        {
            int index = -1;
            for (int i = 0; i < this.deviceList.Count; i++)
            { 
                if (this.deviceList[i].deviceId == item.deviceId )
                {
                    this.deviceList[i].Update(item);
                    index = i;
                    break;
                }            
            }
            if (index == -1)
            {
                return  this.AppendToDeviceList(item);
            }
            return true;        
        }

        private void ProcessReportDeviceList(string message)
        {
            try
            {
                DeviceListContext deviceContext = JsonConvert.DeserializeObject<DeviceListContext>((string)message);
                for (int i = 0; i < deviceContext.Data.Count; i++)
                {
                    DeviceItem item = this.GetByDeviceId(deviceContext.Data[i].DeviceId);
                    if (item == null)
                    {
                        item = new DeviceItem();
                    }                   
                    item.Assign(deviceContext.Data[i]);
                    this.AddDeviceItem(item);
                    this.UpdateWithDeviceItem(item);
                }
                this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListAll.ToString(), null, "");

            }
            catch (Newtonsoft.Json.JsonReaderException)
            {

            }
        }

        private void ProcessReportDeviceRegister(string message)
        {
            try
            {
                RegisterContext context = JsonConvert.DeserializeObject<RegisterContext>((string)message);
                DeviceItem item = new DeviceItem();
                foreach (DeviceData data in context.Data)
                {
                    item.deviceId = data.DeviceId;
                    item.secret = data.Secret;
                    item.modelId = data.ModelId;
                    item.registered = true;
                    item.online = true;
                    item.deleted = false;
                    if (this.AddDeviceItem(item))
                    {
                        this.UpdateWithDeviceItem(item);
                        this.Notify(REFRESH_DEVICE_LIST_EVENT, DeviceOperateEvent.ListAdd.ToString(), item, "");
                        this.GetDeviceAllAttribute(item.deviceId);
                    }
                }

            }
            catch (Newtonsoft.Json.JsonReaderException)
            {

            }

        }
        private void ProcessReportDeviceUnRegister(string message)
        {
            try
            { 
                CommandContext context = JsonConvert.DeserializeObject<CommandContext>((string)message);
                foreach (BaseData data in context.Data)
                {
                    this.UnregisterDeviceItem(data.DeviceId);
                    
                }           
     
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {

            }


        }

        private void ProcessReportDeviceOnlineStatus(string message)
        {
            this.ProcessReportDeviceAttribute(message);
        }


        public void DeleteDeviceItem(DeviceItem device)
        {
            if (device != null)
            {
                this.DeleteDevice(device.deviceId);
                device.updateConfig = false;
                device.updateWithNode = false;
                device.deleted = true;
            }      
        }

        private void ProcessReportDeviceAttribute(string message)
        {
            CommandContext context = JsonConvert.DeserializeObject<CommandContext>((string)message);

            foreach (BaseData data in context.Data)
            {
                if (data.DeviceId == StringResource.GATEWAY_DEVICE_ID)
                {
                    this.gateway.UpdateProperty(data);
                    this.gateway.Update();
                    this.Notify(REFRESH_GATEWAY_EVENT, DeviceOperateEvent.ListAll.ToString(), null, "");
                }
                else
                {
                    DeviceItem item = this.GetByDeviceId(data.DeviceId);
                    
                    if (item != null)
                    {
                        this.UpdateWithDeviceItem(item);
                        item.UpdateProperty(data);
                        this.Notify(REFRESH_DEVICE_EVENT, DeviceOperateEvent.ListUpdate.ToString(), item, "");
                    }
                    else
                    {
                        item =  this.AddDeviceItem(data);
                        if (item != null)
                        {
                            this.UpdateWithDeviceItem(item);
                            this.Notify(REFRESH_DEVICE_EVENT, DeviceOperateEvent.ListAdd.ToString(), item, "");
                        }
                    }
                }
                
            }  
        }
        private void ProcessReportDeviceAllAttributes(string message)
        {
            this.ProcessReportDeviceAttribute(message);
        }

        private void ProcessReportDeviceEvent(string message)
        {
            this.ProcessReportDeviceAttribute(message);
        }


        public void BeatHeart()
        {
            this.gateway.startBeatHeartDatetime = DateTime.Now;
            BeatHeartCommand command = new BeatHeartCommand();
            this.TCPClient.SendData(command.Pack());   
        }


        /// <summary>
        /// 设置允许入网时间
        /// </summary>
        /// <param name="seconds"></param>
        /// 范围0~255，0:不允许入网，1~254：允许入网秒数，255：允许永久入网

        public void AllowGatewayAdd(int seconds)
        {
            this.gateway.allowedseconds = seconds;
            BaseCommand command = new AllowAddCommand(seconds);
            this.TCPClient.SendData(command.Pack());         
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public void GetDeviceList()
        {
            BaseCommand command = new GetDeviceListCommand();
            this.TCPClient.SendData(command.Pack());  
        }

        /// <summary>
        /// 注销设备
        /// </summary>
        /// <param name="deviceId"></param>
        public void DeleteDevice(string deviceId)
        {
            BaseCommand command = new DeleteDeviceCommand(deviceId);
            this.TCPClient.SendData(command.Pack());             
        }
        /// <summary>
        /// 获取设备当前所有属性或状态指令（子设备控制指令）
        /// </summary>
        /// <param name="deviceId"></param>
        public void GetDeviceStatus(string deviceId)
        {
            BaseCommand command = new GetDeviceStatusCommand( deviceId);
            this.TCPClient.SendData(command.Pack());   
        
        }


        /// <summary>
        /// 查询设备所有属性指令
        /// </summary>
        /// <param name="deviceId"></param>
        public void GetDeviceAllAttribute(string deviceId)
        {
            BaseCommand command = new GetDeviceAllAttributeCommand(deviceId);
            this.TCPClient.SendData(command.Pack());         
        }
       /// <summary>
        /// 查询网关所有属性指令
       /// </summary>
        public void GetGatewayAllAttribute()
        {
            BaseCommand command = new GetDeviceAllAttributeCommand();
            this.TCPClient.SendData(command.Pack());
        }
        /// <summary>
        /// 查询设备单个或部分属性指令
        /// </summary>
        /// <param name="attributes"></param>
        public void GetDeviceAttributes(List<BaseData> attributes)
        {
            BaseCommand command = new GetDeviceAttributesCommand(attributes);
            this.TCPClient.SendData(command.Pack());                 
        }

        /// <summary>
        /// 设备控制指令 支持多属性
        /// </summary>
        /// <param name="datas"></param>
        public void DeviceControl(List<BaseData> datas)
        {
            BaseCommand command = new DeviceCtrlCommand(datas);
            this.TCPClient.SendData(command.Pack());   
        }

        /// <summary>
        /// 设备查询指令 支持多属性
        /// </summary>
        /// <param name="datas"></param>
        public void DeviceQuery(List<BaseData> datas)
        {
            BaseCommand command = new DeviceQueryCommand(datas);
            this.TCPClient.SendData(command.Pack());             
        }

        
    }
}
