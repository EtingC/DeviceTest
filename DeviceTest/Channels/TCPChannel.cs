using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using JK.Libs.Storage;
using JK.Channels.Base;

namespace JK.Channels.TCP
{
    public class TCPClientChannel:BaseChannel
    {
        public const int RECEIVE_BUFFER_SIZE = 4096;

        public const int TCP_CONTROL_EVENT = 2000;
        public const int TCP_COMMAND_EVENT = 2001;
        public const int TCP_DATA_EVENT = 2002;

        private TcpClient TCPClient;
        
        public int Port { get; set; }
        public string Host { get; set; }
        private int ReadTimeout { get; set; }
        private DateTime startConnectDatetime { get;set; }
        private int tryConnectInterval { get; set; }

        private Thread receiveProcessor;
        private Boolean terminated { get; set; }
        public Boolean connecting { get; set; }


        private void Initialize()
        {           
            this.ReadTimeout = 1000;          
        }
        public TCPClientChannel()
        {
            this.Caption = "TCPClient";
            this.TCPClient = new TcpClient();
            this.Initialize();
        }

        public Boolean ReConnect()
        {
            this.Close();
            return this.Open();
        }


        public TCPClientChannel(String Caption)
        {
            this.Caption = Caption + "_TCPClient";
            this.TCPClient = new TcpClient();
            this.Initialize();
        }


        private void ConnectCallback(IAsyncResult result)
        {
 
            TcpClient client = (TcpClient)result.AsyncState;
            try
            {
                this.connecting = false;
                if (client.Connected)
                {
                    client.EndConnect(result);
                    this.LastErrorMessage = "连接成功！";
                    this.Notify(TCP_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.OK, this.LastErrorMessage);
                    return;
                }
                else
                {
                    client.EndConnect(result);
                    this.LastErrorMessage = "连接失败！";                  
                }
            }
            catch (System.ObjectDisposedException)
            {
                this.LastErrorMessage = "客户端是关闭的！";
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.LastErrorMessage = "端口[" + this.Port.ToString() + "]无效！";
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.LastErrorMessage = "网络连接出错！";
            }
            this.Notify(TCP_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.Error, this.LastErrorMessage); 
        }
        public void OpenSync()
        {
            if (!this.connecting)
            {
                try
                {
                    this.startConnectDatetime = DateTime.Now;
                    
                    this.TCPClient.BeginConnect(this.Host, this.Port, new AsyncCallback(ConnectCallback), this.TCPClient);
                    this.LastErrorMessage = "开始尝试连接！";
                    this.Notify(TCP_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.Operating, this.LastErrorMessage);
                    this.connecting = true;
                }
                catch (System.InvalidOperationException)
                {
                    this.TCPClient = new TcpClient();               
                }
            }   
        }

        public override Boolean Open()
        {
            try
            {                
                this.TCPClient.Connect(this.Host, this.Port);
                this.LastErrorMessage = "连接成功！";
                this.Notify(TCP_CONTROL_EVENT,ChannelControl.Open.ToString(), ChannelResult.OK, this.LastErrorMessage);
                return true;
            }
            catch (System.ObjectDisposedException)
            {
                this.LastErrorMessage = "客户端是关闭的！";
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.LastErrorMessage = "端口[" + this.Port.ToString() + "]无效！";
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.LastErrorMessage = "网络连接出错！";
            }
            this.Notify(TCP_CONTROL_EVENT, ChannelControl.Open.ToString(), ChannelResult.CanNotOpen, this.LastErrorMessage);
            return false;
           
        }
        public override Boolean Close()
        {
            try
            {
                this.TCPClient.Close();
                this.connecting = false;
                this.LastErrorMessage = "TCPClient 关闭！";
                this.Notify(TCP_CONTROL_EVENT, ChannelControl.Close.ToString(), ChannelResult.OK, this.LastErrorMessage);
            }
            catch(System.Net.Sockets.SocketException)
            {
                this.LastErrorMessage = "网络异常！";
                this.Notify(TCP_CONTROL_EVENT, ChannelControl.Close.ToString(), ChannelResult.CanNotClose, this.LastErrorMessage);
                return false;
            }
            return true;
        }
        public override Boolean Active()
        {
            return this.TCPClient.Connected; 
        }

        public Boolean ConnectTimeout()
        {
            TimeSpan span = DateTime.Now - this.startConnectDatetime;
            return span.TotalSeconds > this.tryConnectInterval;
        }
        //异步方法
        public  Task<Boolean> SendAsync(string Command, int TimeOut)
        {
            return Task.Run<Boolean>(() =>
            {
                return SendSync(Command, TimeOut);
            });
        }

        public Boolean SendData(string command)
        {
            if (!this.Active())
            {
                return false;
            }
            Boolean result = false;
            try
            {
                var dataGram = Encoding.ASCII.GetBytes(command);
                NetworkStream ClientStream = this.TCPClient.GetStream();
                ClientStream.Write(dataGram, 0, dataGram.Length);
                this.Notify(TCP_DATA_EVENT, ChannelControl.Send.ToString(), ChannelResult.OK, command);
                result = true;
            }
            catch (System.Exception)
            {
                result = false;
            }
            return result;                 
        }

        public void StartAsyncReceiveData()
        {
            this.terminated = false;
            this.receiveProcessor = new Thread(new ThreadStart(this.ReceiveData));
            this.receiveProcessor.IsBackground = true;
            this.receiveProcessor.Start();
        }

        public void StopReceiveData()
        {
            this.terminated = true;
            this.receiveProcessor.Interrupt();
        }

        public void ReceiveData()
        {
            while (!this.terminated)
            {
                try
                {
                    Thread.Sleep(1);
                    ChannelResult resResult = ChannelResult.OK;
                    string ReceiveString = "";
                    byte[] ReceiveBytes = new byte[RECEIVE_BUFFER_SIZE];
                    if (this.Active())
                    {
                        try
                        {
                            NetworkStream ClientStream = this.TCPClient.GetStream();
                            int numberOfReadBytes = ClientStream.Read(ReceiveBytes, 0, RECEIVE_BUFFER_SIZE);

                            byte[] realReceiveBytes = new byte[numberOfReadBytes];
                            Array.Copy(ReceiveBytes, 0, realReceiveBytes, 0, numberOfReadBytes);
                            ReceiveString = System.Text.ASCIIEncoding.Default.GetString(realReceiveBytes);
                            resResult = ChannelResult.OK;

                        }
                        catch (System.Exception)
                        {
                            resResult = ChannelResult.Error;
                        }
                        if (ReceiveString.Length > 0)
                        {
                            this.Notify(TCP_DATA_EVENT, ChannelControl.Receive.ToString(), resResult, ReceiveString);
                        }
                    }                  
                }
                catch (System.Threading.ThreadInterruptedException)
                { 
                }
            }
        }


        //同步方法
        public  Boolean SendSync(string Command, int TimeOut)
        {
            if (!this.Active())
            { 
                this.Open();
            }
            ResponseResult resResult = ResponseResult.Unknown;
            string ReceiveString = "";
            Boolean result = false;
            byte[] ReceiveBytes = new  byte[RECEIVE_BUFFER_SIZE]; 
            if (this.Active())
            {
                try
                {
                    var dataGram = Encoding.ASCII.GetBytes(Command);
                    NetworkStream ClientStream = this.TCPClient.GetStream();
                    ClientStream.Write(dataGram, 0, dataGram.Length);


                    var asyncResult = ClientStream.BeginRead(ReceiveBytes, 0, RECEIVE_BUFFER_SIZE, null, null);                  
                    asyncResult.AsyncWaitHandle.WaitOne(this.ReadTimeout);
                    if (asyncResult.IsCompleted)
                    {
                        try
                        {
                            int numberOfReadBytes = ClientStream.EndRead(asyncResult);
                            byte[] realReceiveBytes = new byte[numberOfReadBytes];
                            Array.Copy(ReceiveBytes, 0, realReceiveBytes, 0, numberOfReadBytes);
                            ReceiveString = System.Text.ASCIIEncoding.Default.GetString(realReceiveBytes);
                            resResult = ResponseResult.Ok;
                            result = true;
                        }
                        catch (System.Exception)
                        {
                            resResult = ResponseResult.Error;
                        }
                    }
                    else
                    {
                        
                        resResult = ResponseResult.TimeOut;
                        this.Close();
                        
                    }
                }
                catch (System.Exception)
                {
                    resResult = ResponseResult.Error;
                }

            }
            else 
            {
                resResult = ResponseResult.Error;
            }
            if (processResponse != null)
            {
                ReceiveString = ReceiveString.Trim();
                processResponse(resResult, ReceiveString);

            }
            return result;  
        }

        public override Boolean SendCommand(String Command)
        { 
            this.SendCommandAsync(Command);
            return true;
        }

        public async void SendCommandAsync(String Command)
        {
            Boolean result = await this.SendAsync(Command, this.ReadTimeout);  
        }

        public override void SaveToFile(string fileName)
        {
            IniFiles.WriteStringValue(fileName, this.Caption, "Host", this.Host);
            IniFiles.WriteIntValue(fileName, this.Caption, "Port", this.Port);
            IniFiles.WriteIntValue(fileName, this.Caption, "ReadTimeout", this.ReadTimeout);
            IniFiles.WriteIntValue(fileName, this.Caption, "TryConnectInterval", this.tryConnectInterval);

        }

        public override void LoadFromFile(string fileName)
        {
            this.Host =         IniFiles.GetStringValue(fileName, this.Caption, "Host", "127.0.0.1");
            this.Port =         IniFiles.GetIntValue(fileName, this.Caption, "Port", 8000);
            this.ReadTimeout =  IniFiles.GetIntValue(fileName, this.Caption, "ReadTimeout", 1000);
            this.tryConnectInterval = IniFiles.GetIntValue(fileName, this.Caption, "TryConnectInterval",10 );
            base.LoadFromFile(fileName);
        }







    }
}
