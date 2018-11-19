using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JK.Program.Pattern;
using JK.Libs.Storage;
using JK.Channels.UDP;
using JK.Channels.Base;
using Newtonsoft.Json;
using System.IO;
using JK.Libs.Utils;

namespace DeviceTest.Manager
{
    public enum PrinterErrorCode
    {
        Ok = 0,
        Ready = 1,
        Buzy = 2,
        NotExists = 3,
        Offline = 4,
        Error = 5
    }
    public class ResponseMessage
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public ResponseMessage()
        {
            this.Message = "";
            this.ErrorCode = (int)PrinterErrorCode.Ok;
        }
    }

    public class LabelController:Manager
    {
        public UDPClientChannel UDPClient { get; set; }
        
        public LabelController()
        {
            this.section = "LabelController";
            this.UDPClient = new UDPClientChannel();
        }

        public override void LoadFromFile(string fileName)
        {

            base.LoadFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
            base.SaveToFile(fileName);
           
        }
        public override void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
            this.UDPClient.LoadFromFile(fileName);
            this.UDPClient.AttachObserver(this.observer.Update);
            this.UDPClient.Open();
            this.UDPClient.StartAsyncReceiveData();
            
        }
        
        public void PrintLabelContext(string labelContext)
        {
            this.UDPClient.SendData(labelContext);        
        }

        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            this.Notify(notifyEvent, flag, result, message);
        }

        public void SavePrintLogFile(string message)
        { 
            string projectFolder = StringResource.MainDataPath    +"标签记录\\";
            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }
            string filePathName = projectFolder + StringResource.TestHistoryFileName;

            string content = Utils.LoadFromFile(filePathName); ;
            string[] contentLines = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> items = new List<string>();

            StreamWriter streamWriter = new StreamWriter(filePathName, true);

            try
            {
                Boolean find = false;

                foreach (string line in contentLines)
                {
                    if (line.IndexOf(message) >= 0)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {                    
                    if (message.Length > 0)
                    {
                        streamWriter.WriteLine(message);
                    }
                }              
            }
            finally
            {
                streamWriter.Close();
            }
        }
    }
}
