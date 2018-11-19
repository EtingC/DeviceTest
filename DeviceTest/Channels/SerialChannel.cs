using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JK.Libs.Storage;
using JK.Channels.Base;

namespace JK.Channels.Serial
{

    public class SerialChannel : BaseChannel
    {

        private System.IO.Ports.SerialPort serialPort;
        public const int COM_CONTROL_EVENT = 1000;
        public const int COM_COMMAND_EVENT = 1001;
        public const int COM_DATA_EVENT = 1002;

        public const int READ_TIMEOUT_MAX = 3000;

        public bool  RTUMode = true;



        public string Port
        {
            get
            {
                return this.serialPort.PortName;
            }
            set
            {
                this.Close();
                this.serialPort.PortName = value;
                this.Open();
            }
        }

        public int BaudRate
        {
            get
            {
                return this.serialPort.BaudRate;
            }
            set
            {
                this.serialPort.BaudRate = value;
            }
        }

        public int ReadTimeout
        {
            get
            {
                return this.serialPort.ReadTimeout;
            }
            set
            {
                this.serialPort.ReadTimeout = value;
            }       
        }

        public void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
        }

        public override Boolean Open()
        {
            Boolean result = false;
            String Name = "串口[" + this.serialPort.PortName + "]";
            try
            {
                this.serialPort.Open();
                result = this.serialPort.IsOpen;
                this.LastErrorMessage = Name + "打开成功！";

                this.Notify(COM_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.OK, this.LastErrorMessage);
                return result;
            }
            catch (System.UnauthorizedAccessException)
            {
                this.LastErrorMessage = Name + "访问拒绝，请确定是否被占用！";
                result = false;
            }
            catch (System.InvalidOperationException)
            {
                this.LastErrorMessage = Name + "打开失败，请确定是否被占用！";
                result = false;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.LastErrorMessage = Name + "打开失败,串口名称无效！";
                result = false;
            }
            catch (System.ArgumentException)
            {
                this.LastErrorMessage = Name + "打开失败，端口无效！";
                result = false;           
            }

            catch (System.IO.IOException)
            {
                this.LastErrorMessage = Name + "打开失败，端口无效！";
                result = false;
            }

            this.Notify(COM_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.CanNotOpen, this.LastErrorMessage);
            return result;
        }

        public override Boolean Close()
        {
            String Name = "串口[" + this.serialPort.PortName + "]";

            if (this.serialPort.IsOpen)
            {
                try
                {
                    this.serialPort.Close();
                    this.LastErrorMessage = Name + "关闭成功！";
                    this.Notify(COM_CONTROL_EVENT, ChannelControl.Close.ToString(), ChannelResult.OK, this.LastErrorMessage);
                }
                catch (System.IO.IOException)
                {
                    this.LastErrorMessage = Name + "无效，关闭失败！";
                    this.Notify(COM_CONTROL_EVENT, ChannelControl.Close.ToString(), ChannelResult.CanNotClose, this.LastErrorMessage);
                    return false;
                }
                
            }
            return !this.serialPort.IsOpen; 
        }

        public override Boolean Active()
        {
            return this.serialPort.IsOpen;
        }

        public  Boolean SendCommandSync(String Command)
        {
            if (!this.serialPort.IsOpen)
            {
                this.Open();
            }
            if ((this.serialPort.IsOpen) && (Command.Length > 0))
            {
                this.serialPort.Write(Command);
                return true;
            }             
            return false;
        }



        public override Boolean SendCommand(String Command)
        {
            if (!this.serialPort.IsOpen)
            {
                this.Open();          
            }
            if ((this.serialPort.IsOpen) && (Command.Length > 0))
            {
                this.Responsed = false;
                this.serialPort.Write(Command);

                DateTime start = DateTime.Now;
                TimeSpan times = DateTime.Now - start;
                this.serialPort.ReadTimeout = 1000;

                while (true)
                {
                    if (times.TotalMilliseconds <= this.serialPort.ReadTimeout) 
                    {
                        if (this.Responsed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (!this.Responsed)
                        {
                            if (processResponse != null)
                            {
                                processResponse(ResponseResult.TimeOut, "");
                            }
                            return false;              
                        }
                    }
                    times = DateTime.Now - start;
                }
                return true;   
            }

            if (processResponse != null)
            {
                processResponse(ResponseResult.Error, "");
            }
            return false;
        }

        public SerialChannel(System.IO.Ports.SerialPort  ASerialPort)
        {
            this.Caption = "SerialPort";
            this.serialPort = ASerialPort;
            this.serialPort.ReadTimeout = 1000;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived); 
        }

        public SerialChannel()
        {
            this.Caption = "SerialPort";
            this.serialPort = new System.IO.Ports.SerialPort();
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);  
        }



        public SerialChannel(String Caption,System.IO.Ports.SerialPort  ASerialPort)
        {
            this.Caption = Caption;
            this.serialPort = ASerialPort;
            this.serialPort.ReadTimeout = 1000;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived); 
        }

        public override void LoadFromFile(string fileName)
        {
            try
            {
                this.serialPort.PortName = IniFiles.GetStringValue(fileName, this.Caption, "PortName", "COM3");
                this.serialPort.BaudRate = IniFiles.GetIntValue(fileName, this.Caption, "BaudRate", 9600);
                this.serialPort.Parity = (System.IO.Ports.Parity)IniFiles.GetIntValue(fileName, this.Caption, "Parity", (int)System.IO.Ports.Parity.None);
                this.serialPort.DataBits = IniFiles.GetIntValue(fileName, this.Caption, "DataBits", 8);
                this.serialPort.StopBits = (System.IO.Ports.StopBits)IniFiles.GetIntValue(fileName, this.Caption, "StopBits", (int)System.IO.Ports.StopBits.One);
                this.serialPort.ReadTimeout = IniFiles.GetIntValue(fileName, this.Caption, "ReadTimeout", 1000);

                base.LoadFromFile(fileName);
                return;
            }
            catch (System.IO.IOException)
            {
                this.LastErrorMessage = "初始化访问无效！"; 
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.LastErrorMessage = "初始化串口参数出错！";
            }
            this.Notify(COM_CONTROL_EVENT, ChannelControl.Init.ToString(), ChannelResult.InvalidParam, this.LastErrorMessage);
                    
        }

        public override void SaveToFile(string fileName)
        {
            IniFiles.WriteStringValue(fileName, this.Caption, "PortName", this.serialPort.PortName);
            IniFiles.WriteIntValue(fileName, this.Caption, "BaudRate", this.serialPort.BaudRate);
            IniFiles.WriteIntValue(fileName, this.Caption, "Parity", (int)this.serialPort.Parity);
            IniFiles.WriteIntValue(fileName, this.Caption, "DataBits", this.serialPort.DataBits);
            IniFiles.WriteIntValue(fileName, this.Caption, "StopBits", (int)this.serialPort.StopBits);
            IniFiles.WriteIntValue(fileName, this.Caption, "ReadTimeout", this.serialPort.ReadTimeout);
        }


        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string value;
            try
            {
                value = this.serialPort.ReadLine();
                value = value.Trim();
                this.Responsed = true;

                this.Notify(COM_DATA_EVENT, ChannelControl.Receive.ToString(), ChannelResult.OK, value);

                if (processResponse != null)
                {
                    processResponse(ResponseResult.Ok, value);
                }
            }
            catch(System.IO.IOException)
            {
                this.Notify(COM_DATA_EVENT, ChannelControl.Receive.ToString(), ChannelResult.ReceiveError, ""); 
            }
            catch (System.InvalidOperationException)
            {
                this.Notify(COM_DATA_EVENT, ChannelControl.Receive.ToString(), ChannelResult.ReceiveError, "");
            }
            catch (System.UnauthorizedAccessException)
            {
                this.Notify(COM_DATA_EVENT, ChannelControl.Receive.ToString(), ChannelResult.ReceiveError, "");
            }
                
            catch (System.TimeoutException)
            {
                this.Notify(COM_DATA_EVENT, ChannelControl.Receive.ToString(), ChannelResult.ReceiveError, "");
                if (processResponse != null)
                {
                    processResponse(ResponseResult.Error, "");
                }
            }

        }

    }
}
