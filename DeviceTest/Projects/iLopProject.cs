using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceTest.Manager;
using DeviceTest.Base;
using JK.Libs.Utils;

namespace DeviceTest.Projects
{



    public enum iLopCommandType
    { 
        None = 0,
        WriteProductkeys = 1000,
        ReadProductkeys  = 1001,
        ReadDeviceStatus = 1002,
        ConnentTotServer = 1003,
        ReadConnectStatus = 1004,
    }
 



    public class DeviceCommand
    {
        public const string STX = "AT+TEST=";
        public const string ETX = "\r\n";
        public const string RESPONSE_ETX = ";";

        public string command { get; set; }
        public iLopCommandType type { get; set; }

        public iLopController controller { get; set; }
        public DeviceCommand(iLopController controller)
        {
            this.controller = controller;
            this.command = "";
            this.type = iLopCommandType.None;
        }

        public virtual string Pack(string value )
        {
            return STX + value + ETX;
        }

        public virtual string Package()
        {
            return "";
        }

        public virtual Boolean ParseResponseText(string response)
        {
            return false;
        }
    }

    public class ReadConnectStatusCommand : DeviceCommand
    {
        public ReadConnectStatusCommand(iLopController controller) :
            base(controller)
        {
            this.type = iLopCommandType.ReadConnectStatus;
        }
        public override string Package()
        {
            string commandText = "CLOUD_STATE?";
            command = this.Pack(commandText);
            return command;
        }
        //  +CLOUD_STATE:1\r\n   1连上，其他未连上
        public override  Boolean ParseResponseText(string response)
        {
            string value = "";
            Boolean result = Utils.GetValueAnd("CLOUD_STATE:", RESPONSE_ETX, response, ref  value);
            if (result)
            {               
                this.controller.activeDevice.online = value == "1";
                this.controller.activeDevice.statusMessage = "获取到平台连接状态[" + this.controller.activeDevice.onlineText+ "}";
                this.controller.activeDevice.UpdateValueWithPropertyKey("ConnectStatus", value);
                return true;
            }
            return false;
        }
    }
    public class ConnectToServerCommand : DeviceCommand
    {
        public ConnectToServerCommand(iLopController controller) :
            base(controller)
        {
            this.type = iLopCommandType.ConnentTotServer;         
        }
        public override string Package()
        {
            string commandText = "CLOUD_CONN";
            command = this.Pack(commandText);
            return command;
        }

        public override Boolean  ParseResponseText(string response)
        {
            if (response.Contains("+CLOUD_CONN"))
            {
                this.controller.activeDevice.statusMessage = "连接平台操作完成";
                return true;
            }
            return false;

            
        }
    }

    public class ReadDeviceStatusCommand : DeviceCommand
    {
        public ReadDeviceStatusCommand(iLopController controller) :
            base(controller)
        {
            this.type = iLopCommandType.ReadDeviceStatus;         
        }
        public override string Package()
        {
            string commandText = "DEV_STATE?";
            command = this.Pack(commandText);
            return command;
        }

        private Boolean GetValue(string value,out ushort outData)
        {
            ushort data = 0;
            ushort result = 0;
            outData = 0;
            try
            {
                string valueText = value.Substring(0, 2);
                data = Convert.ToUInt16(valueText, 16);
                result = (ushort)(256 * data);

                valueText = value.Substring(2, 2);
                data = Convert.ToUInt16(valueText, 16);
                result += data;
                outData = result;
                return true;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return false;
            }          
            
        }
        public override Boolean ParseResponseText(string response)
        {
            string value = "";
            Boolean result = Utils.GetValueAnd("DEV_STATE:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.statusMessage = "获取到设备状态值[" + value + "]";

                try
                {
                    string valueText = value.Substring(0, 4);
                    ushort data = 0;
                    if (this.GetValue(valueText, out data))
                    {
                        this.controller.activeDevice.UpdateValueWithPropertyKey("PowerStatus", data.ToString());
                    }

                    valueText = value.Substring(4, 4);
                    if (this.GetValue(valueText, out data))
                    {
                        this.controller.activeDevice.UpdateValueWithPropertyKey("GasStatus", data.ToString());
                    }

                    valueText = value.Substring(8, 4);
                    if (this.GetValue(valueText, out data))
                    {
                        this.controller.activeDevice.UpdateValueWithPropertyKey("GasSensor", data.ToString());
                    }

                    valueText = value.Substring(12, 4);
                    if (this.GetValue(valueText, out data))
                    {
                        this.controller.activeDevice.UpdateValueWithPropertyKey("PM25", data.ToString());
                    }

                    valueText = value.Substring(16, 4);
                    if (this.GetValue(valueText, out data))
                    {
                        if (data != 0)
                        {
                            short tempData = (short)((data / 10) - 50);
                            this.controller.activeDevice.UpdateValueWithPropertyKey("Temperature", tempData.ToString());
                        }
                    }
                    return true;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return false;
                }
                             
            }
            return false;
        }
    }
    public class ReadDeviceKeysCommand : DeviceCommand
    {
        public ReadDeviceKeysCommand(iLopController controller) :
            base(controller)
        {
            this.type = iLopCommandType.ReadProductkeys;         
        }
        public override string Package()
        {
            string commandText = "P_KEYR?";
            command = this.Pack(commandText);

            commandText = "D_NAMR?";
            command += this.Pack(commandText);

            commandText = "D_SECR?";
            command += this.Pack(commandText);

            return command;
        }


        public override Boolean ParseResponseText(string response)
        {
            string value = "";
            Boolean result = Utils.GetValueAnd("P_KEYR:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[0] = value;
                this.controller.activeDevice.statusMessage = "获取到Product Key值["+value+"]";
                this.controller.activeDevice.UpdateValueWithPropertyKey("ProductKey", value);
                return true;
            }
            value = "";
            result = Utils.GetValueAnd("D_NAMR:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[1] = value;
                this.controller.activeDevice.statusMessage = "获取到Device Name值["+value+"]";
                this.controller.activeDevice.UpdateValueWithPropertyKey("DeviceName", value);
                return true;
            }
            value = "";
            result = Utils.GetValueAnd("D_SECR:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[2] = value;
                this.controller.activeDevice.statusMessage = "获取到Device Secret值[" + value + "]";
                this.controller.activeDevice.UpdateValueWithPropertyKey("Secret", value);
                return true;
            }
            return false;
        }


    }
    public class WriteDeviceKeysCommand : DeviceCommand
    {

        public WriteDeviceKeysCommand(iLopController controller):
            base(controller)
        {
            this.type = iLopCommandType.WriteProductkeys;         
        }

        public override string Package()
        {
            string commandText = "P_KEYW," + this.controller.deviceCodeItem.deviceId;
            command = this.Pack(commandText);

            commandText = "D_NAMW," + this.controller.deviceCodeItem.deviceName;
            command += this.Pack(commandText);

            commandText = "D_SECW," + this.controller.deviceCodeItem.secret;
            command += this.Pack(commandText);

            commandText = "CONFIG_SAVE";
            command += this.Pack(commandText);

            commandText = "RESET";
            command += this.Pack(commandText);

            return command;
        }

        public override Boolean ParseResponseText(string response)
        {
            string value = "";
            Boolean result = Utils.GetValueAnd("P_KEYW:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[0] = value;
                this.controller.activeDevice.statusMessage = "写入Product Key["+value+"]操作";
                return true;
            }
            value = "";
            result = Utils.GetValueAnd("D_NAMW:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[1] = value;
                this.controller.activeDevice.statusMessage = "写入Device Name ["+value+"]操作";
                return true;
            }
            value = "";
            result = Utils.GetValueAnd("D_SECW:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                this.controller.activeDevice.productKeys[2] = value;
                this.controller.activeDevice.statusMessage = "写入Device Secret ["+value+"]操作";
                return true;
            }
            value = "";
            result = Utils.GetValueAnd("CONFIG_SAVE:", RESPONSE_ETX, response, ref  value);
            if (result)
            {
                if (value == "OK")
                {
                    this.controller.activeDevice.statusMessage = "保存参数成功";
                }
                else
                {
                    this.controller.activeDevice.statusMessage = "保存参数失败";
                }
                return true;
            }
            value = "";
            if (response.Contains("RESET:OK"))
            {
                this.controller.activeDevice.statusMessage = "复位成功";
                return true;
            }
           
            return false;

        }

    }



    public class iLopController : DeviceTest.Manager.Manager
    {
        public const int COMMAND_KEYS_MAX = 11;
        public string[] COMMAND_KEYS {get;set;}
        public DeviceController controller { get; set; }
        public DeviceItem activeDevice { get; set; }
        public DeviceCodeItem deviceCodeItem { get; set; }

        public string productKey { get; set; }
        public Boolean validProductKey {
            get { 
                if (this.deviceCodeItem!=null)
                {
                    return this.deviceCodeItem.errorCode == 0;
                }
                return false;
            }
        }
        public iLopController(DeviceController controller) :
            base()
        { 
            this.controller = controller;
            this.productKey = "";
            
            this.COMMAND_KEYS = new string[COMMAND_KEYS_MAX]{
                                    "P_KEYR",                         
                                    "D_NAMR",
                                    "D_SECR",
                                    "P_KEYW",
                                    "D_NAMW",
                                    "D_SECW",                       
                                    "CONFIG_SAVE",
                                    "RESET",
                                    "CLOUD_CONN",
                                    "CLOUD_STATE",
                                    "DEV_STATE",
                                };
            this.activeDevice = new DeviceItem();
            this.deviceCodeItem = new DeviceCodeItem();

        }

        public Boolean KeyEquals(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        return this.activeDevice.productKeys[index] == this.deviceCodeItem.deviceId;
                    }
                case 1:
                    {
                        return this.activeDevice.productKeys[index] == this.deviceCodeItem.deviceName;
                    }
                case 2:
                    {
                        return this.activeDevice.productKeys[index] == this.deviceCodeItem.secret;
                    }
            }
            return false;
        }

        public override void Initialize(string fileName)
        {
            this.activeDevice.modelId = "iLop0001";
            this.controller.UpdateWithDeviceItem(this.activeDevice);           
        }
        public void ClearDevice()
        {
            this.activeDevice.ClearProductKeys();
            this.activeDevice.statusMessage = "";
            this.activeDevice.resultStatus = "";
            this.activeDevice.testResult = TestResult.Unknown;      
        }

        public void UpdateDevice(DeviceCodeItem item)
        {
            this.activeDevice.productKeys[0] = item.deviceId;
            this.activeDevice.productKeys[1] = item.deviceName;
            this.activeDevice.productKeys[2]  = item.secret;
        }

        public int ValidCommandResponse(string message)
        {
            for(int i= 0; i<COMMAND_KEYS_MAX;i++)
            {
                if (message.Contains(COMMAND_KEYS[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public void WriteProductKeys()
        {
            DeviceCommand command = new WriteDeviceKeysCommand(this);
            this.controller.serialChannel.SendCommandSync(command.Package());
        }

        public void ReadProductKeys()
        {
            DeviceCommand command = new ReadDeviceKeysCommand(this);            
            this.controller.serialChannel.SendCommandSync(command.Package());
        }

        public void ReadConnectStatus()
        {
            DeviceCommand command = new ReadConnectStatusCommand(this);
            this.controller.serialChannel.SendCommandSync(command.Package());
        }

        public void ConnectToServer()
        {
            DeviceCommand command = new ConnectToServerCommand(this);
            this.controller.serialChannel.SendCommandSync(command.Package());
        }

        public void ReadDeviceStatus()
        {
            DeviceCommand command = new ReadDeviceStatusCommand(this);
            this.controller.serialChannel.SendCommandSync(command.Package());
        }


        public Boolean  ProcessCommandResponse(string message ,ref int commandIndex)
        {
            message += message + ";";
            commandIndex = this.ValidCommandResponse(message);
            Boolean result = false;
            switch (commandIndex)
            {
                case 0:
                case 1:
                case 2:
                    {
                        DeviceCommand command = new ReadDeviceKeysCommand(this);
                        result = command.ParseResponseText(message);
                        break;
                    }
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    {
                        DeviceCommand command = new WriteDeviceKeysCommand(this);
                        result = command.ParseResponseText(message);
                        break;
                    }
                case 8:                                        {
                        DeviceCommand command = new ConnectToServerCommand(this);
                        result = command.ParseResponseText(message);
                        break;                   
                    }
                case 9:
                    {
                        DeviceCommand command = new ReadConnectStatusCommand(this);
                        result = command.ParseResponseText(message);
                        break;                   
                    }
                case 10:
                    {
                        DeviceCommand command = new ReadDeviceStatusCommand(this);
                        result = command.ParseResponseText(message);
                        break;
                    }
            }
            return result;
        }
        
    

    }




}
