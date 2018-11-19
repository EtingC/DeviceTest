using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JK.Libs.Storage;
using DeviceTest.Base;
using Newtonsoft.Json;
using JK.Libs.Utils;
using System.IO;
using JK.Libs.Converter;
using JK.Channels.HttpUtils;
using JK.Channels.Base;


namespace DeviceTest.Manager
{

    public enum DeviceColor
    { 
        White  = 1,
        Golden = 2,
        Black  = 3,
        Gray   = 4
    }


    public enum GenerateSNType
    { 
        None=0,
        AutoInc=1,
        FromFile =2,
        FromServer =3,
    }

    public class ArgumentDevice : BaseDevice
    {
        public int deviceColor { get; set; }
        public string SN { get; set; }

        public string labelFilePathName { get;set;}
        public ArgumentDevice(DeviceColor deviceColor) :
            base()
        {
            this.deviceColor = (int)deviceColor ;
            this.SN = "";
            this.labelFilePathName = "";
            
        }
 
       
        public void Assign(DeviceItem device)
        {
            this.deviceId = device.deviceId;
            this.model = device.model;
            this.modelId = device.modelId;
            this.name = device.name;
            this.secret = device.secret;
            this.serialCode = device.serialCode;
            this.softwareVersion = device.softwareVersion;
            this.hardwareVersion = device.hardwareVersion;
            this.remark = device.remark;
        }


    }

    public class Project:Manager
    {
        private const string ArgumentFile = "argument.txt";
       
        private Process commandProcess;
        public string name { get; set; }
        public string executeFilename { get; set; }
        public string executePath { get; set; }
        public string labelPrintFile { get; set; }
        public string arguments { get; set; }
        public Boolean hidden { get; set; }
        public string resultMessage { get; set; }
        public int serialNumber { get; set; }
        public GenerateSNType generateSNType { get; set; }
        public string SNListFileName { get; set; }     
        public DateTime generateDate { get; set; }

        public HttpController httpController { get; set; }
        public string serialCode { get; set; }
        public DeviceNode activeDeviceNode { get; set; }
        public string labelFilePathName
        {
            get{
                return  this.executePath + "\\"+this.labelPrintFile;
            }
        }
        public Boolean active
        {
            get
            {
                return !(this.commandProcess.HasExited);
            }
        }

        public Project():
            base()
        {
            this.section = "Project";
            this.commandProcess = new Process();
            this.resultMessage = "";
            this.name = "";
            this.executeFilename = "";
            this.arguments = "";
            this.hidden = true;
            this.executePath = "";
            this.serialCode = "";
            this.httpController = new HttpController();
            this.httpController.AttachObserver(this.observer.Update);
        }

        public string GenerateSerialNumber(DeviceItem device, ref Boolean result)
        {
            string serialNumber = "";
            string symbol = device.deviceId;
            switch (this.generateSNType)
            {
                case GenerateSNType.None:
                    {
                        serialNumber =  "";
                        result = true;
                        break;
                    }
                case GenerateSNType.AutoInc:
                    { 
                        serialNumber = this.serialNumber.ToString().PadLeft(8, '0');
                        result = true;
                        break;
                    }
                case GenerateSNType.FromFile:
                    {
                        Boolean saved = false;
                        serialNumber = this.GetSerialNumberFromFile(symbol,ref result ,ref saved);
                        if (result && (!saved))
                        {
                            this.SaveSerialNumberLinkSymbol(serialNumber,symbol);
                        }
                        break;
                    }
                case GenerateSNType.FromServer:
                    {
                        serialNumber = this.GetSerialNumberFromServer(device, ref result);
                        break;
                    }
                
            }
            return serialNumber;
        }





        private void SaveSerialNumberLinkSymbol(string serialNumber, string symbol)
        { 
            string pathFileName = this.executePath + "\\" + this.SNListFileName;
            string content = Utils.LoadFromFile(pathFileName);
            content = content.Replace(serialNumber, serialNumber + " " + symbol);
            Utils.SaveToFile(pathFileName, content);   
        }

        private void QuerySerialNumber(DeviceItem device)
        {
            this.httpController.updated = false;
            this.httpController.useBaseUrl = true;
            this.httpController.commandUrl = StringResource.QUERY_SERIAL_NUMBER_DEVICE_URL;
            this.httpController.commandFlag = (int)HttpRequestType.QueryRegistedDeviceData;
            this.httpController.paramList.Clear();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>("product_key", device.productKey);
            this.httpController.paramList.Add(pair);
            pair = new KeyValuePair<string, string>("code_type", "iLop");
            this.httpController.paramList.Add(pair);
            pair = new KeyValuePair<string, string>("identity", device.deviceId);
            this.httpController.paramList.Add(pair);
            pair = new KeyValuePair<string, string>("serial_code", device.serialCode);
            this.httpController.paramList.Add(pair);
            this.httpController.GetAsync();          
        }

        public string GetSerialNumberFromServer(DeviceItem device, ref Boolean result)
        {
            result = false;
            this.QuerySerialNumber(device);
            if (this.httpController.responseCode == ResponseResult.Ok)
            {
                try
                {
                    DeviceCodeItem deviceCodeItem = JsonConvert.DeserializeObject<DeviceCodeItem>(this.httpController.responseText);
                    result = deviceCodeItem.errorCode == 0;
                    return deviceCodeItem.deviceName;
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    return "";
                }

            }
            else
            {

                return "";
            }     
        }
        public string GetSerialNumberFromFile(string symbol, ref Boolean result,ref Boolean saved)
        {
            string pathFileName = this.executePath + "\\" + this.SNListFileName;
            if (File.Exists(pathFileName))
            {
                StreamReader reader = new StreamReader(pathFileName, Encoding.UTF8);
                try
                {                  
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] array = line.Split(new char[2] { ' ', ',' });
                        if (array.Length >= 2)
                        {
                            if (symbol == array[1])
                            {
                                saved = true;
                                result = true;
                                return array[0];
                            }
                            else 
                            {
                                line = reader.ReadLine();
                            }
                        }
                        else if (array.Length == 1)
                        {
                            saved = false;
                            result = true;
                            return array[0];
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                saved = false;
                result = false;
            }
            return "";    
        }

        public Project(string caption, string executeFilename, string arguments)
        {
            this.section = caption;
            this.arguments = arguments;
            this.executeFilename = executeFilename;
            this.commandProcess = new Process();
        }


        public void SaveArgument(string content)
        {
            Utils.SaveToFile(this.executePath + "\\" + ArgumentFile, content);     
        }

        public string Execute(string argumentContent)
        {
            this.SaveArgument(argumentContent);
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = this.hidden;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            processStartInfo.UseShellExecute = false;
            processStartInfo.FileName = this.executeFilename;
            processStartInfo.Arguments = this.executePath + "\\" + this.arguments;          
            Process process = System.Diagnostics.Process.Start(processStartInfo);
            StreamReader outputStreamReader = process.StandardOutput;
            process.WaitForExit();
            
            this.resultMessage = "";

            if (process.HasExited)
            {              
                this.resultMessage = outputStreamReader.ReadToEnd();
            }
            return this.resultMessage;
          

        }


  



        private void processOutputDataReceived(object sender, DataReceivedEventArgs e)
        {

            this.resultMessage = e.Data;

        }

        public void IncSerialNumber()
        {
            TimeSpan span = this.generateDate.Date - DateTime.Now.Date;
            if (span.TotalDays == 0)
            {
                this.serialNumber += 1;
                this.SaveToFile();
            }
            else
            {
                this.generateDate = DateTime.Now;
                this.serialNumber = 1; 
            }
        }


        
        public Boolean Start()
        {
            this.commandProcess.StartInfo.FileName = this.executeFilename;
            this.commandProcess.StartInfo.Arguments = this.arguments;
            this.commandProcess.StartInfo.CreateNoWindow = this.hidden;
            this.commandProcess.StartInfo.UseShellExecute = false;
            try
            {
                return commandProcess.Start();
            }
            catch (System.InvalidOperationException)
            {
                return false;
            }


        }

        public void Stop()
        {
            try
            {
                if (!this.commandProcess.HasExited)
                {
                    this.commandProcess.Kill();
                }
            }
            catch (System.InvalidOperationException)
            {

            }
        }
        public override void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
        }

        public override void LoadFromFile(string fileName)
        {
            this.labelPrintFile = IniFiles.GetStringValue(fileName, this.section, "LabelPrintFile", StringResource.ProjectDefaultPrintLabelFile);
            this.executeFilename = IniFiles.GetStringValue(fileName, this.section, "ExecuteFilename", "");
            this.arguments = IniFiles.GetStringValue(fileName, this.section, "Arguments", "");
            this.hidden = IniFiles.GetBoolValue(fileName, this.section, "Hidden", true);
            this.name = IniFiles.GetStringValue(fileName, this.section, "Name", "");           
            string dateinfo = IniFiles.GetStringValue(fileName, this.section, "GenerateDate", DateTime.Now.ToString("yyyy/MM/dd"));
            this.generateDate = Convert.ToDateTime(dateinfo);
            TimeSpan span = this.generateDate.Date - DateTime.Now.Date;
            if (span.TotalDays == 0)
            {
                this.serialNumber = IniFiles.GetIntValue(fileName, this.section, "SerialNumber", 1);
            }
            else
            {
                this.serialNumber = 1;
                IniFiles.WriteIntValue(fileName, this.section, "SerialNumber", this.serialNumber);
                IniFiles.WriteStringValue(fileName, this.section, "GenerateDate", DateTime.Now.ToString("yyyy/MM/dd"));
            }
            string temp = IniFiles.GetStringValue(fileName, this.section, "GenerateSNType",GenerateSNType.AutoInc.ToString() );
            this.generateSNType = (GenerateSNType)Enum.Parse(typeof(GenerateSNType), temp);

            this.SNListFileName = IniFiles.GetStringValue(fileName, this.section, "SNListFileName", StringResource.ProjectDefaultSNListFile);

            this.httpController.baseUrl = IniFiles.GetStringValue(fileName, this.section, "BaseURL", StringResource.DEFAULT_BASE_URL);
            this.serialCode = IniFiles.GetStringValue(fileName, this.section, "serialCode", "None");

            base.LoadFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
            IniFiles.WriteStringValue(fileName, this.section, "LabelPrintFile", this.labelPrintFile);
            IniFiles.WriteStringValue(fileName, this.section, "ExecuteFilename", this.executeFilename);
            IniFiles.WriteStringValue(fileName, this.section, "Arguments", this.arguments);
            IniFiles.WriteBoolValue(fileName, this.section, "Hidden", this.hidden);
            IniFiles.WriteStringValue(fileName, this.section, "Name", this.name);
            IniFiles.WriteStringValue(fileName, this.section, "GenerateDate", DateTime.Now.ToString("yyyy/MM/dd"));
            IniFiles.WriteIntValue(fileName, this.section, "SerialNumber", this.serialNumber);
            IniFiles.WriteStringValue(fileName, this.section, "GenerateSNType", this.generateSNType.ToString());
            IniFiles.WriteStringValue(fileName, this.section, "SNListFileName", this.SNListFileName);
            IniFiles.WriteStringValue(fileName, this.section, "BaseURL", this.httpController.baseUrl);
            IniFiles.WriteStringValue(fileName, this.section, "SerialCode", this.serialCode);
        }

    }





    public class KeyValueItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public KeyValueItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public class GenerateResult
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string FileURL { get; set; }
        public List<KeyValueItem> Items { get; set; }

        public GenerateResult()
        {
            this.Message = "";
            this.ErrorCode = 0;
            this.FileURL = "";
            this.Items = new List<KeyValueItem>();
        }

        public string GetValue(string key)
        {
            foreach (KeyValueItem item in this.Items)
            {
                if (key == item.Key)
                {
                    return item.Value;
                }
            }
            return "";
        }

    }


    public class ProjectService:Manager
    {
        public const int PROJECT_LABEL_GENERATE_EVENT = 20001;
        public const int PROJECT_LABEL_PRINT_FINISH_EVENT = 20002;
        public List<Project> projects { get; set; }
        public int activeProjectIndex { get; set;}
        public int deviceColor { get; set; }
        public string activeLabelData { get; set; }

        public GenerateResult generateResult { get; set; }

        public ResponseMessage responseMessage;

        public string activeProjectName {
            get
            {
                if (this.activeProject != null)
                {
                    return this.activeProject.name;
                }
                return "None";
            }
        }
        public Boolean result {
            get {
                return this.generateResult.ErrorCode == 0;
            }
        }

        public string message {
            get {
                return this.generateResult.Message;
            }
        }


        public Project activeProject {
            get {
                if (this.activeProjectIndex < this.projects.Count)
                { 
                    return this.projects[this.activeProjectIndex];
                }
                return null;
            }
        }
        public ProjectService():
            base()
        {
            this.section = "ProjectService";
            this.projects = new List<Project>();
            this.activeProjectIndex = 0;
            this.deviceColor = (int)DeviceColor.White;
            this.activeLabelData = "";
            this.generateResult = new GenerateResult();
        }

        public override void Initialize(string fileName)
        {
            this.LoadFromFile(fileName);
            if (Directory.Exists(StringResource.ProjectsPath))
            {
                string[] devicePaths = System.IO.Directory.GetDirectories(StringResource.ProjectsPath);
                foreach (string devicePath in devicePaths)
                {
                    string configFileName = devicePath + StringResource.ProjectConfigFileName;
                    Project project = new Project();
                    project.executePath = devicePath;
                    project.LoadFromFile(configFileName);
                    this.projects.Add(project);
                }
            }
        }

        public override void LoadFromFile(string fileName)
        {
            this.deviceColor = IniFiles.GetIntValue(fileName, this.section, "DeviceColor", (int)DeviceColor.White);
            this.activeProjectIndex = IniFiles.GetIntValue(fileName, this.section, "ActiveProjectIndex", 0);
            base.LoadFromFile(fileName);
        }

        public override void SaveToFile(string fileName)
        {
            base.SaveToFile(fileName);
            IniFiles.WriteIntValue(fileName, this.section, "ActiveProjectIndex", this.activeProjectIndex);
            IniFiles.WriteIntValue(fileName, this.section, "DeviceColor", this.deviceColor);
        }

        private string GenerateSerialNumber(DeviceItem device, ref Boolean result)
        {
            return this.activeProject.GenerateSerialNumber( device,ref result);
        }

        public Boolean GenerateLabelContent(DeviceItem device)
        {
            Boolean result = false;
            if (this.activeProject != null)
            {
                ArgumentDevice argumentDevice = new ArgumentDevice((DeviceColor)this.deviceColor);
                argumentDevice.Assign(device);
                
                argumentDevice.SN = this.GenerateSerialNumber(device,ref result);
                if (result)
                {
                    device.serialNumber = argumentDevice.SN;
                    argumentDevice.labelFilePathName = this.activeProject.labelFilePathName;
                    string argumentDeviceContent = JsonConvert.SerializeObject(argumentDevice);
                    string label = this.activeProject.Execute(argumentDeviceContent);
                    try
                    {
                        this.generateResult = JsonConvert.DeserializeObject<GenerateResult>((string)label);
                    }
                    catch (Newtonsoft.Json.JsonReaderException)
                    {

                    }
                    this.activeLabelData = label;
                    device.labelData = Utils.Unicode2String(label);
                }
                else
                {
                    this.generateResult.ErrorCode = 1;
                    this.generateResult.Message = "无法获取生产标识码(SN)";
                }
            }
            else
            {
                this.activeLabelData = "无法生成标签";
                this.generateResult.ErrorCode = 1;
                this.generateResult.Message = "无法生成标签";
            }
            this.Notify(PROJECT_LABEL_GENERATE_EVENT, "", device, this.activeLabelData);
            return result;
        }
        
    }
}
