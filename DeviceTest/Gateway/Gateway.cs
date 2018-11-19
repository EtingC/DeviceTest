using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JK.Program.Pattern;
using DeviceTest.Base;
using DeviceTest.Manager;
using Newtonsoft.Json;

namespace DeviceTest.Center
{



    public class BeatHeartCommand : BaseOperate
    {
        public string Period;
        public BeatHeartCommand()
        {
            this.Command = "TcpBeatHeart";
            this.Period = "60";
        }
    }


    public class ScanNetWorkCommand : BaseOperate
    { 
        public ScanNetWorkCommand()
        {
            this.Command = "RequestTcp";
        }
        public override string Pack()
        {
            string package = JsonConvert.SerializeObject(this);
            return package;
        } 
    }

    public class GatewayCommand : DispatchCommand
    {
        public GatewayCommand() :
            base()
        {
            BaseData data = new BaseData();
            data.DeviceId = "0000000000000000";
            data.Key = "";
            data.Value = "";
            this.Data.Add(data);
        }   
    
    }

    public class AllowAddCommand : GatewayCommand
    {       
        public AllowAddCommand(int allowSenconds) :
            base()
        {
            this.Type = CommandType.Add.ToString();            
            this.Data[0].Key = "Time";
            this.Data[0].Value = allowSenconds.ToString();
        }
    }


    public class GetDeviceListCommand : GatewayCommand
    {
        public GetDeviceListCommand() :
            base()
        {
            this.Type = CommandType.DevList.ToString();
            this.Data[0].Key = "DeviceList";
        }
    }

    public class DeleteDeviceCommand : GatewayCommand
    {
        public DeleteDeviceCommand(string deviceId) :
            base()
        {
            this.Type = CommandType.Delete.ToString();
            this.Data[0].DeviceId = deviceId;
        }
    }


    public class ResetFactoryCommand : GatewayCommand
    {
        public ResetFactoryCommand() :
            base()
        {
            this.Type = CommandType.ReFactory.ToString();
            this.Data.Clear();
        }
    }



    public class GetCooInfoCommand : GatewayCommand
    {
        public GetCooInfoCommand() :
            base()
        {
            this.Type = CommandType.Ctrl.ToString();
            this.Data[0].Key = "GetCooInfo";
        }
    }




    public class GetDeviceStatusCommand : GatewayCommand
    {
        public GetDeviceStatusCommand(string deviceId) :
            base()
        {
            this.Type = CommandType.Ctrl.ToString();
            this.Data[0].DeviceId = deviceId;
            this.Data[0].Key = "GetStatus";
        }
    }




    public class GetDeviceAllAttributeCommand : GatewayCommand
    {
        //网关
        public GetDeviceAllAttributeCommand() :
            base()
        {
            this.UpdateData();              
        }

        //设备
        public GetDeviceAllAttributeCommand(string deviceId) :
            base()
        {
            this.UpdateData();
            this.Data[0].DeviceId = deviceId;
        }

        public void UpdateData()
        {
            this.Type = CommandType.DevAttri.ToString();
            this.Data[0].Key = "All";       
        }
    }





    public class GetCooMACCommand: GatewayCommand
    {
        public GetCooMACCommand() :
            base()
        {
            this.Type = CommandType.Attribute.ToString();     
        }
        
    }








    /// <summary>
    /// 获取设备部分属性
    /// </summary>
    public class GetDeviceAttributesCommand:GatewayCommand
    {
        //key  =deviceid
        //value= Attribute name
        public GetDeviceAttributesCommand(List<BaseData> attributes) :
            base()          
        {
            this.Type = CommandType.Attribute.ToString();
            this.Data.Clear();
            for (int i = 0; i < attributes.Count; i++)
            {
                BaseData data = new BaseData();
                data.DeviceId = attributes[i].DeviceId;
                data.Key = attributes[i].Key;
                this.Data.Add(data);
            }            
        }  
    }



    //FIXME:查询指定设备的邻居信息
    //FIXME:查询指定设备的子节点信息



    public class Gateway:FactDevice
    {
        public Boolean active { get; set; }
        public int allowedseconds { get; set; }
        public int beatHeartSeconds { get; set; }
        public int activeSeconds { get; set; }
        public DateTime startBeatHeartDatetime { get; set; }
        public DateTime lastBeatHeartDatetime { get; set; }
        public Boolean autoPermitJoining { get; set; }
        public int port { get; set; }
        public string address { get;set;}

        public string discrible {
            get {
                return this.name + "[" + this.address + ":" + this.port.ToString() + "]";
            }
        }



        public void Update()
        {
            Property property = this.GetProperty("SofterVersion");
                if (property != null)
                {
                    this.softwareVersion = property.value;
                }
                property = this.GetProperty("FirmwareVersion");
                if (property != null)
                {
                    this.hardwareVersion  = property.value;
                }
                            
        }
        public Boolean permitJoining
        {
            get
            {
                Property property = this.GetProperty("PermitJoining");
                if (property != null)
                {
                    return property.value == "1";
                }
                else
                {
                    return false;
                }
            
            }
        }

        public Gateway():
            base()
        {
            this.active = false;
            this.allowedseconds = 0;
            this.beatHeartSeconds = 60;
            this.startBeatHeartDatetime = DateTime.Now;
            this.lastBeatHeartDatetime = DateTime.Now;
            this.address = "";
            this.port = 0;
        }

         
        public Boolean BeatHeartTimeout()
        {
            TimeSpan span = DateTime.Now - this.startBeatHeartDatetime;
            return span.TotalSeconds >= this.beatHeartSeconds;
        }

        public Boolean InvalidConnected()
        {
            TimeSpan span = DateTime.Now - this.lastBeatHeartDatetime;
            return span.TotalSeconds >= this.beatHeartSeconds;            
        }
        public string permitJoiningText
        {
            get
            {
                string result = "";
                if (this.permitJoining)
                {
                    return result + "允许(" + this.allowedseconds.ToString()+")";
                }
                else
                {
                    return result + "禁止(" + this.allowedseconds.ToString() + ")";
                }
            }
        }

        public string onlineText
        {
            get {
                string result = "";
                if (this.active)
                {
                    return result + "连接";
                }
                else 
                {
                    return result + "断开";
                }
            }
        }
        public string detailMessage
        {
            get {
                    string result = "连接:"+this.onlineText + " " +
                                   "入网:"+this.permitJoiningText + " " +
                                   "硬件版本:" + this.hardwareVersion + " " +
                                   "软件版本:" + this.softwareVersion;
                    return result;            
                }
        }
    }



    public class GatewayNodes
    {
        public int total { get; set; }
        public List<GatewayNode> rows { get; set; }

        public GatewayNodes()
        {
            this.rows = new List<GatewayNode>();
        }

        public string GetModelByType(string type)
        {
            foreach (GatewayNode node in this.rows)
            {
                if (type == node.index.ToString())
                {
                    return node.model;
                }
            }
            return "未知类型";
        }
        public string GetNameByType(string type)
        {
            foreach (GatewayNode node in this.rows)
            {
                if (type == node.index.ToString())
                {
                    return node.name;
                }
            }
            return "未知名称";
        }
    }

    public class GatewayNode
    {
        public int index { get; set; }
        public string serialCode { get; set; }
        public string model { get; set; }
        public string name { get; set; }   
        public GatewayNode()
        {
            this.index = 0;
            this.serialCode = "";
            this.model = "";
            this.name = ""; 
        }
    }

    
    
}
