using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using JK.Libs.Utils;
using DeviceTest.Manager;

namespace DeviceTest.Base
{

    public enum Operation
    {
        Unknown = 0, //'未知'
        UpdateFirmware = 1, //固件烧写
        Test = 2,// 测试
        Aging = 3,//老化
        Finish =4//入库
    }


    public enum DeviceType
    { 
        AL = 0,
        HA = 1,
        Other =2,
    }

    public enum ControlCommand
    {
        Dispatch = 0,
        Report   = 1,
        BeatHeartResponse =2
    }

    public enum CommandType
    { 
        Ctrl =10001,//设备控制
        Add  =10002,//允许设备入网
        Delete = 10003, //设备删除
        Attribute = 10004,//查询设备属性
        DevAttri = 10005, //查询设备全部属性
        DevList = 10006,//获取设备列表
        NeighborInfo = 10007, //查询邻居信息
        ChildrenInfo = 10008,//子节点信息
        ReFactory = 10009,//恢复出厂设置

        Register = 10010,//设备注册上报
        UnRegister = 10011, //设备注销上报
        OnOff = 10012, //设备在线状态上报
        Event = 10016,//设备事件上报
        CooInfo = 10018,//COO网络信息上报

    }


    public enum Operate
    { 
        Ctrl = 1,
        Event = 2,
        Attribute = 4
    }

    public enum DataType
    {
        Enum = 0,//枚举
        Range = 1,//范围
        Value = 2,//值
    }
    public class ValueConfig
    {
        public DataType dataType { get; set; }
        /// <summary>
        /// 范围值，configs[0]为下限值  configs[1]为下限值
        /// 枚举值，KeyValuePair中 key 为枚举值描述 value为 枚举值 
        /// </summary>
        public List<KeyValuePair<string, string>> configs { get; set; }
        public ValueConfig()
        {
            this.dataType = DataType.Range;
            this.configs = new List<KeyValuePair<string, string>>();
        }

        public void Assign(ValueConfig config)
        {
            this.dataType = config.dataType;
            this.configs.Clear();
            foreach (KeyValuePair<string, string> pair in config.configs)
            {
                KeyValuePair<string, string> keyValue = new KeyValuePair<string, string>(pair.Key,pair.Value);
                this.configs.Add(keyValue);
            }
        }
           
    
    }

    public  class BaseProperty
    {
        /// <summary>
        ///参数关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 参数值配置
        /// </summary>
        public ValueConfig valueConfig { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 操作方式
        /// </summary>
        public List<Operate> operates { get; set; }
        /// <summary>
        /// 关键字名称
        /// </summary>
        public string keyName { get; set; }
        /// <summary>
        ///属性描述
        /// </summary>
        public string describe { get; set; }
        public BaseProperty()
        { 
            this.key     = "";
            this.value   = "";
            this.unit    = "";
            this.operates = new List<Operate>();
            this.keyName = "";
            this.describe= "";
            this.valueConfig = new ValueConfig();
        }

        public string rangeText {
            get {
                string result = "----";
                switch (this.valueConfig.dataType)
                {
                    case DataType.Value:
                    case DataType.Range:
                        {
                            if (this.valueConfig.configs.Count == 2)
                            {
                                result = "["+this.valueConfig.configs[0].Value +","+ this.valueConfig.configs[1].Value+"]";
                            }                        
                            break;
                        }                  
                    case DataType.Enum:
                        {
                            result = "----";
                            break;
                        }
                }
                return result;             
            }
        }


        public Boolean validValue {
            get {
                if ((this.valueConfig.dataType == DataType.Range) && (this.valueConfig.configs.Count == 2))
                { 
                    try
                    {
                        double data = 0.0;
                        double min = 0.0;
                        double max = 0.0;
                        double eps = 1e-6;
                        Boolean resultValue = double.TryParse(this.value,out data);
                        Boolean resultMin   = double.TryParse(this.valueConfig.configs[0].Value,out min);
                        Boolean resultMax   = double.TryParse(this.valueConfig.configs[1].Value,out max);
                        if (resultValue && resultMin && resultMax)
                        {
                            return ((data - min) > -eps) && ((data - max) < eps);
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public string AliasValueText(string value)
        {
            string result = "未知";
            switch (this.valueConfig.dataType)
            {
                case DataType.Range:
                    {
                        result = this.value;
                        break;
                    }
                case DataType.Enum:
                    {
                        foreach (KeyValuePair<string, string> pair in this.valueConfig.configs)
                        {
                            if (this.value == pair.Value)
                            {
                                result = pair.Key;
                                break;
                            }
                        }
                        break;
                    }
            }
            return result;
        }

        public string valueText {
            get {
                return this.AliasValueText(this.value);
            }
        }

        public string operateText
        {
            get {
                return this.operates.ToString();
            }
        }
    }

    public class BaseDevice
    {
        public string serialCode { get;set;}
        public string name{get;set;}
        public string model { get; set; }
        public string deviceId { get; set; }
        public string modelId{get;set;}
        public string secret{get;set;}
        public int subDeviceType{get;set;}
        public string deviceType { get; set; }
        public string hardwareVersion{get;set;}
        //当前版本
        public string softwareVersion{get;set;}

        public string mcuBinVersion { get; set; }
        public string resultStatus { get; set; }
        public string remark { get;set;}
        public DeviceType type {
            get {
                if (this.deviceType.Length == 0)
                    return DeviceType.Other;
                return (DeviceType)Enum.Parse(typeof(DeviceType), this.deviceType); 
            }
        }
        public BaseDevice()
        {
            this.serialCode = "";
            this.name = "";
            this.model  = "";
            this.deviceId  = "";
            this.modelId = "";
            this.secret = "";
            this.subDeviceType = 0;
            this.deviceType = "";
            this.hardwareVersion = "";
            this.softwareVersion = "";
            this.mcuBinVersion = "";
            this.resultStatus = "";
            this.remark = "";
        }
    }


    public class PropertyValue
    {
        public string value { get; set; }
        public DateTime dateTime { get; set; }

        public string unit { get; set; }
        public PropertyValue()
        {
            this.value = "";
            this.dateTime = DateTime.Now;
            this.unit = "";
        }

    }


    public class Property : BaseProperty
    {
        public DateTime lastUpdateTime { get; set; }
        public List<PropertyValue> values { get; set; }
        public Property() :
            base()
        {
            this.lastUpdateTime = DateTime.Now;

            this.values = new List<PropertyValue>();
        }

        public void AddValue(string value)
        {
            PropertyValue propertyValue = new PropertyValue();
            propertyValue.value = value;
            propertyValue.unit = this.unit;
            this.values.Add(propertyValue);
        }
        public void Assign(PropertyConfig config)
        {
            this.key = config.key;
            this.value = config.value;
            this.unit = config.unit;
            foreach (Operate operate in config.operates)
            {
                this.operates.Add(operate);
            }
            this.keyName = config.keyName;
            this.describe = config.describe;
            this.lastUpdateTime = DateTime.Now;
            this.valueConfig.Assign(config.valueConfig);
        }
    }
    public  class FactDevice:BaseDevice
    {
        public Boolean online { get; set; }
        public Boolean registered { get; set; }
        /// <summary>
        /// 最新版本
        /// </summary>
        public string lastSoftwareVersion { get; set; }
        public List<Property> propertys { get; set; }

        public FactDevice():
            base()
        { 
            this.online  = false;
            this.registered  = false;
            this.lastSoftwareVersion = "";
            this.propertys = new List<Property>();          
        }

        public void UpdateProperty(PropertyConfig propertyConfig)
        {
            foreach (Property property in this.propertys)
            {
                if (property.key == propertyConfig.key)
                {
                    property.Assign(propertyConfig);
                    return;
                }
            }
            this.AddProperty(propertyConfig);        
        }

        public void AddProperty(PropertyConfig propertyConfig)
        {
            Property newProperty = new Property();
            newProperty.Assign(propertyConfig);
            this.propertys.Add(newProperty);
        }

        public void UpdateProperty(BaseData data)
        {
            if (data.Key == "SofterVersion")
            {
                this.softwareVersion = data.Value;
                return;
            }
            if (data.Key == "FirmwareVersion")
            {
                this.hardwareVersion = data.Value;
                return;
            }

            foreach (Property property in this.propertys)
            {

                if (property.key == data.Key)
                {
                    property.value = data.Value;
                    string newValue = property.AliasValueText(data.Value);
                    property.AddValue(newValue);
                    property.lastUpdateTime = DateTime.Now;
                    return;
                }
            }
            this.AddProperty(data);
            
        }

        public Property GetProperty(string key)
        {
            foreach (Property property in this.propertys)
            {
                if (property.key == key)
                {
                    return property;
                }
            }
            return null;
        }


        public void AddProperty(BaseData data)
        {
            Property newProperty = new Property();
            newProperty.key = data.Key;
            newProperty.value = data.Value;
            newProperty.lastUpdateTime = DateTime.Now;
            this.propertys.Add(newProperty);
        }

   }

    public class DeviceItem : FactDevice
    {

        public string[] productKeys { get; set; }

        public string statusMessage { get; set; }
        public string labelData { get; set; }
        public Boolean updateLabelData { get; set; }
        public Boolean updateConfig { get; set; }
        public Boolean updateWithNode { get; set; }
        public TestResult testResult { get; set; }
        public string productKey { get; set; }
        public string serialNumber { get; set; }
        public Operation operation { get; set; }
        public Boolean deleted { get; set; }
        public int printlabelSum { get; set; }

        public string status
        {
            get {
                if (statusMessage.Length == 0)
                {
                    return this.versionStatusText;
                }
                else
                {
                    return statusMessage;
                }
                
            }
        }
        public string onlineText
        {
            get {
                if (this.online)
                {
                    return "在线";
                }
                else
                {
                    return "离线";
                }
            }
        }


        public string exportText {
            get {
                return this.deviceId + "," + 
                    this.name + "," + 
                    this.model + "," + 
                    this.modelId + "," + 
                    this.status + "," + 
                    this.softwareVersion+ "," + 
                    this.serialNumber;
            }
        }

        public void IncPrintLabelSum()
        {
            this.printlabelSum += 1;
        }

        public string registedText {
            get {
                if (this.registered)
                {
                    return "注册";
                }
                else
                {
                    return "注销";
                }           
            }
        }
        public string versionStatusText
        {
            get
            {
                if (this.versionStatus)
                {
                    return "版本正常";
                }
                else
                {
                    return "版本不一致";
                }   
            }
        }
        public Boolean versionStatus
        {
            get
            {
                if ( this.lastSoftwareVersion!="") 
                {
                    return this.softwareVersion == this.lastSoftwareVersion;                
                }
                return true;
            }
        }

        public DeviceItem() :
            base()
        {
            this.updateConfig = false;
            this.updateWithNode = false;
            this.testResult = TestResult.Unknown;
            this.updateLabelData = false;
            this.deleted = false;
            this.operation = Operation.Test;
            this.statusMessage = "";
            this.printlabelSum = 0;
            this.productKeys = new string[3] { "", "", "" };
        }


        public void ClearProductKeys()
        {
            for (int i = 0; i < 3; i++)
            {
                this.productKeys[i] = "";
            }

        }

        public void UpdateValueWithPropertyKey(string key, string value)
        {
            foreach (Property property in this.propertys)
            {
                if (property.key == key)
                {
                    property.value = value;
                    string newValue = property.AliasValueText(value);
                    property.AddValue(newValue);
                    property.lastUpdateTime = DateTime.Now;
                    return;
                }
            }        
        }
        public void Update(DeviceItem item)
        {
            this.online = item.online;
            this.modelId = item.modelId;
            this.secret = item.secret;
            this.registered = item.registered;
            this.softwareVersion = item.softwareVersion;
            this.deleted = item.deleted;
            this.operation = item.operation;
        }

        public void Assign(DeviceConfig config)
        {
            this.name = config.name;
            this.model = config.model;
            this.secret = config.secret;
            this.subDeviceType = config.subDeviceType;
            this.lastSoftwareVersion = config.softwareVersion;
            this.mcuBinVersion = config.mcuBinVersion;
            foreach (PropertyConfig property in config.propertys)
            {
                this.UpdateProperty(property);
            }
        }
        public void Assign(DeviceData data)
        {
            this.online =  (data.Online == "1");
            this.modelId = data.ModelId;
            this.deviceId = data.DeviceId;
            this.secret = data.Secret;
            this.registered = (data.RegisterStatus == RegisterStatus.Registed);
            this.softwareVersion = data.Version;
        }
        
   
    }

    //处理接收到的网关数据




    public class BaseOperate
    {
        public const byte STX = 2;
        public const byte ETX = 3;
        public string Command { get; set; }
        public BaseOperate()
        {
            this.Command = "";
        }

        public virtual string Pack()
        {
            string package = ((char)STX).ToString() + JsonConvert.SerializeObject(this) + ((char)ETX).ToString();
            return package;                   
        }        
    }
    public class BaseContext : BaseOperate
    {
        public string FrameNumber{get;set;}
        public string Type{get;set;}
        public BaseContext() :
            base()
        {
            this.FrameNumber = "";
            this.Type = "";           
        }
    }

    public class BaseData
    {
        public string DeviceId { get; set; }
        public string ModelId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public BaseData()
        {
            this.DeviceId = "";
            this.ModelId = "";
            this.Key = "";
            this.Value = "";
        }
    }






    public class CommandContext : BaseContext
    {
        public List<BaseData> Data { get; set; }
        public CommandContext():
            base()
        {
            this.Data = new List<BaseData>();
        }
        
    }

    public class RegisterContext : BaseContext
    {
        public List<DeviceData> Data { get; set; }
        public RegisterContext() :
            base()
        {
            this.Data = new List<DeviceData>();
        }
    }




    public enum RegisterStatus
    {
        Unregisted = 0,
        Registed   = 1   
    }

    public class DeviceData:BaseData
    {
        public string Version { get; set; }
        public string Secret { get; set; }
        public string Online {get;set;}
        public RegisterStatus RegisterStatus{get;set;}

    }

    public class DeviceListContext : BaseContext
    { 
        public int TotalNumber{get;set;}
        public int AlreadyReportNumber{get;set;}
        public List<DeviceData> Data { get; set; }
    }





    //COMMAND

    public class BaseCommand : BaseContext
    {

        public List<BaseData> Data;
        public BaseCommand()
        {
            this.Data = new List<BaseData>();
            this.FrameNumber = "00";
        }
    }



    public class DispatchCommand : BaseCommand
    {
        public DispatchCommand() :
            base()
        {
            this.Command = ControlCommand.Dispatch.ToString();            
        }   
    }

    public class DeviceCtrlCommand : DispatchCommand
    {
        public DeviceCtrlCommand(List<BaseData> datas) :
            base()
        {
            this.Type = CommandType.Ctrl.ToString();
            foreach (BaseData data in datas)
            {
                BaseData newData = new BaseData();
                newData.DeviceId = data.DeviceId;
                newData.Key = data.Key;
                newData.Value = data.Value;
                this.Data.Add(newData);
            }
            
        }
    }

    public class DeviceQueryCommand : DispatchCommand
    {
        public DeviceQueryCommand(List<BaseData> datas) :
            base()
        {
            this.Type = CommandType.Attribute.ToString();
            foreach (BaseData data in datas)
            {
                BaseData newData = new BaseData();
                newData.DeviceId = data.DeviceId;
                newData.Key = data.Key;
                this.Data.Add(newData);
            }          
        }
       
    }



    /// <summary>-----------------------------------
    /// 云端设备数据接口处理类
    /// </summary>
    public class Module
    {
        public string model { get; set; }
        public string name { get; set; }
        public string serialCode { get; set; }

    }
    public class Firmware : Module
    {
        public string firmwareURL { get; set; }
        public string modelId { get; set; }
        public string reason { get; set; }
        public string secret { get; set; }
        public int status { get; set; }
        public string version { get; set; }
    }


    public class DeviceCodeList
    {
        public string param { get; set; }
        public int total { get; set; }
        public List<DeviceCodeItem> rows { get; set; }

        public DeviceCodeList()
        {
            this.total = 0;
            this.param = "";
            this.rows = new List<DeviceCodeItem>();
        }
        public void Clear()
        {
            this.param = "";
            this.total = 0;
            this.rows.Clear();
        }

        public Boolean ExportToFile(string fileName)
        {
            List<string> lines = new List<string>();
            string line = "ProductKey,Identity(MAC),SerialCode";
            lines.Add(line);

            foreach (DeviceCodeItem item in this.rows)
            { 
                line = item.deviceId+","+item.identity+","+item.remark;
                lines.Add(line);
            }
            Utils.WriteListToFile(lines, fileName);
            return true;
        }

    }

    public class DeviceNode : Module
    {
        public string companyName { get; set; }
        public int errorCode { get; set; }
        public string flag { get; set; }
        public string productKey { get;set;}
        public Firmware firmware { get; set; }
        public Module module { get; set; }
        public DeviceConfig config { get; set; }
        public int totalCount { get; set; }
        public int remainCount { get;set;}
              
    }


    public class ProductNode
    {
        public string mcode{get;set;}
        public string productKey{get;set;}
        public string productTsl{get;set;}
        public string  zigbeeModel{get;set;}
        public string modelId{get;set;}    
    }
    public class ProductNodes
    {
        public string version { get; set; }
        public List<ProductNode> products { get; set; }

        public ProductNodes()
        {
            this.products = new List<ProductNode>();
        }
    }

    public class DeviceNodes
    {
        public int total { get; set; }
        public List<DeviceNode> rows { get; set; }



        public DeviceNodes()
        {
            this.rows = new List<DeviceNode>();
        }

        public DeviceNode GetBySerialCode(string code)
        {
            foreach (DeviceNode node in this.rows)
            {
                if (node.serialCode == code)
                {
                    return node;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 云端设备数据接口处理类
    /// </summary>------------------------------------------


    public class PropertyConfig : BaseProperty
    {
        public PropertyConfig() :
            base()
        {
            this.valueConfig = new ValueConfig();
        }
        
    }
    
    public class DeviceConfig : BaseDevice
    {
        public string updatedDateTime { get; set; }
        public List<PropertyConfig> propertys { get; set; }

        public DeviceConfig() :
            base()
        {
            this.propertys = new List<PropertyConfig>(); 
        }

        public string deviceProfilePathName()
        { 
           return StringResource.DevicesPath + this.modelId  + StringResource.ProfileFileName;
        }
        public void SaveToFile()
        {
            string content = JsonConvert.SerializeObject(this);
            string path = StringResource.DevicesPath + this.modelId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Utils.SaveToFile(this.deviceProfilePathName(), content);
        }
    }


    public class DeviceConfigList
    {
        public List<DeviceConfig> items { get; set; }
        public DeviceConfigList()
        {
            this.items = new List<DeviceConfig>();
        }
        private DeviceConfig GetByModelId(string modelId)
        {
            foreach (DeviceConfig config in this.items)
            {
                if (modelId.Contains(config.modelId) || config.modelId.Contains(modelId))
                {
                    return config;
                }
            }
            return null;
        
        }

        public void UpdateConfig(DeviceConfig config)
        {
            DeviceConfig selfConfig = this.GetByModelId(config.modelId);
            if (selfConfig!=null)
            {
                DateTime localDatetime =  Convert.ToDateTime(selfConfig.updatedDateTime);
                DateTime serverDatetime = Convert.ToDateTime(config.updatedDateTime);
                if (serverDatetime > localDatetime)
                {
                    config.SaveToFile();
                }           
            }
            else
            {
                config.SaveToFile();
            }          
        }


        public void Initialize()
        {
            if (Directory.Exists(StringResource.DevicesPath))
            {
                this.items.Clear();
                string[] devicePaths = System.IO.Directory.GetDirectories(StringResource.DevicesPath);
                string pathName = "";
                foreach (string devicePath in devicePaths)
                {
                    pathName = devicePath + StringResource.ProfileFileName;
                    string content = Utils.LoadFromFile(pathName);
                    if (content.Length > 0)
                    {
                        try
                        {
                            DeviceConfig deviceConfig = JsonConvert.DeserializeObject<DeviceConfig>((string)content);
                            this.items.Add(deviceConfig);
                        }
                        catch (Newtonsoft.Json.JsonReaderException)
                        {

                        }
                    }
                }
            }

        }
    }

    public class DeviceCodeItem
    {
        public int errorCode { get; set; }
        public string deviceId { get; set; } //productKey
        public string secret { get; set; }
        public string identity { get; set; }
        public string codeType { get; set; }
        public string deviceName { get; set; }//SN
        public string remark{get;set;}
        public Boolean active{get;set;}
        public DeviceCodeItem()
        {
            this.errorCode = 0;
            this.deviceId = "";
            this.secret = "";
            this.identity = "";
            this.codeType = "";
            this.deviceName = "";
            this.remark = "";
            this.active = false;
        }
        public string activeText
        {
            get {
                if (this.active)
                    return "激活";
                else
                    return "----";

            }
        }

    }



}
