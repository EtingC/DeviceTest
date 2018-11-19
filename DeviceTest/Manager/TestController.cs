using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceTest.Manager;
using DeviceTest.Base;
using System.Threading;
using JK.Channels.TCP;
using JK.Channels.Base;
using Newtonsoft.Json;

namespace DeviceTest.Manager
{
    public enum TestType
    {
        Auto = 0,
        Manual =1
    }
    public enum TestOperate
    { 
        Idle  =0,
        Start = 1,
        Control = 2,
        Query  =3,
        Stop = 4,
        Wait = 5,
        Notify =6,

    }

    public enum TestResult
    {   
        Pass = 0,
        Fail = 1,
        Unknown = 99,
    }
    
    public class BaseTestController:Manager
    {
        public List<DeviceItem> devices { get; set; }
        public  DeviceItem activeDevice { get; set; }    
        protected TestManager manager { get; set; }

        public BaseTestController(TestManager manager ):
            base()           
        {
            this.manager = manager;
            this.devices = new List<DeviceItem>();
            this.activeDevice = null;  
        }

        
        public void UpdateTestDeviceList(DeviceItem device)
        {
            this.activeDevice = device;
            this.devices.Clear();
            this.devices.Add(device);
        }
        public void UpdateTestDeviceList(List<DeviceItem> devices)
        {
            this.devices.Clear();
            foreach (DeviceItem device in devices)
            {
                this.devices.Add(device);
            }
            if (this.devices.Count > 0)
            {
                this.activeDevice = this.devices[0];
            }          
        }
    
    }
    public class TestController : BaseTestController
    {
        public const int DEFAULT_WAIT_MILLISECONDS = 1000;

        public const int TEST_CONTROLLER_MESSAGE_EVENT = 10000;


        private Thread testProcessor { get; set; }
        protected Boolean terminated { get; set; }
        protected DateTime StartDatetime{get;set;}
        public Boolean active { get; set; }

        public TestResult testResult { get; set; }

        public TestController(TestManager manager):
            base(manager)
        {
            this.active = false;
            this.terminated = false;
            this.testResult = TestResult.Unknown;
        }


        public Boolean TimeOut(int waitMilliseconds)
        {
            TimeSpan span = DateTime.Now - this.StartDatetime;
            return span.TotalMilliseconds >= waitMilliseconds;
        }
        public virtual void StartTest()
        {
            this.testProcessor = new Thread(new ThreadStart(this.TestProcess));
            this.testProcessor.IsBackground = true;
            this.StartDatetime = DateTime.Now;
            this.terminated = false;
            this.testProcessor.Start();
            this.active = true;
            this.testResult = TestResult.Unknown;
        }
        public virtual void StopTest()
        {
            this.terminated = true;
            this.active = false;
        }

        public virtual void TestProcess()
        {
            return;
        }




    }

    public class AutoTestController : TestController
    {
        public AutoTestController(TestManager manager) :
            base(manager)
        { 
            
        }


        public override void StartTest()
        {
            base.StartTest();
            this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Start.ToString(), TestType.Auto, "自动测试开始");
        }
        public override void StopTest()
        {
            base.StopTest();
            this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Stop.ToString(), TestType.Auto, "自动测试停止");
        }


        public override void TestProcess()
        {
            while (!this.terminated)
            {
                Thread.Sleep(1);
                if (this.activeDevice!=null)
                {
                    foreach (Property property in activeDevice.propertys)
                    {
                        if (this.terminated)
                        {
                            break;
                        }
                        foreach(Operate operate in property.operates)
                        {
                            switch (operate)
                            {
                                case Operate.Attribute:
                                    {
                                        BaseData data = new BaseData();
                                        data.DeviceId = activeDevice.deviceId;
                                        data.Key = property.key;
                                        this.manager.DeviceQuery(data);
                                        this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Query.ToString(), TestType.Auto, "正在查询 " + property.keyName + " 数据");
                                        this.StartDatetime = DateTime.Now;
                                        Thread.Sleep(DEFAULT_WAIT_MILLISECONDS);
                                        if (this.terminated)
                                        {
                                            break;
                                        }   
                                        break;
                                    }
                                case Operate.Ctrl:
                                    {
                                        if (property.valueConfig.dataType == DataType.Enum)
                                        {
                                            foreach (KeyValuePair<string, string> pair in property.valueConfig.configs)
                                            {
                                                BaseData data = new BaseData();
                                                data.DeviceId = activeDevice.deviceId;
                                                data.Key = property.key;
                                                data.Value = pair.Value;
                                                this.manager.DeviceControl(data);
                                                this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Control.ToString(), TestType.Auto, "正在操作 " + property.keyName + " " + pair.Key);
                                                Thread.Sleep(DEFAULT_WAIT_MILLISECONDS);
                                                this.StartDatetime = DateTime.Now;
                                                if (this.terminated)
                                                {
                                                    break;
                                                }                                   
                                            }                                        
                                        }                                   
                                        this.StartDatetime = DateTime.Now;
                                        break;
                                    }                    
                            }           
                        }
                    }
                    this.StopTest();
                    break;
                }
            }
        }
    
    }



    public class ManualTestStep
    {
        protected ManualTestController controller { get; set; }
        public ManualTestStep(ManualTestController controller)
        {
            this.controller = controller;
        }

    
        public virtual void Initialize()
        {
            return;
        }

        public virtual void CheckStatus()
        {
            return;
        }

        public virtual void Execute()
        {
            this.CheckStatus();
        }
    }
    
    public class IdleManualTestStep:ManualTestStep      
    {
        public IdleManualTestStep(ManualTestController controller):
            base(controller)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            this.controller.Initialize();
        }
        public override void CheckStatus()
        {
            if(this.controller.testOperate == TestOperate.Start)
            {
                this.controller.testStep = new WaitManualTestStep(this.controller);
            }
        }      
    }



    public class WaitManualTestStep : ManualTestStep
    {
        private Boolean notified { get; set; }
        public WaitManualTestStep(ManualTestController controller) :
            base(controller)
        {
            this.Initialize();
            this.controller.updated = false; 
        }
        public override void Initialize()
        {
            this.notified = false;
        }
        public override void CheckStatus()
        {
            if (this.controller.finished)
            {
                this.controller.testStep = new StopManualTestStep(this.controller);
                return;
            }
        }
        public override void Execute()
        {
            this.CheckStatus();
            if (!this.notified)
            {
                if (this.controller.UpdateWaitDeviceData())
                {
                    string message =  "请手动操作 "+this.controller.lastOperateMessage;
                    this.controller.NotifyMessage(message);
                    this.notified = true;
                }
                else
                {
                    return;
                }

            }
   
            if (this.controller.updated)
            {
                string message ="手动操作 "+this.controller.lastOperateMessage+"完成";
                this.controller.NotifyMessage(message);
                this.controller.ToNextWaitData();
                this.controller.testStep = new WaitManualTestStep(this.controller);
            }
        }

    }


    public class StopManualTestStep : ManualTestStep
    {
        public StopManualTestStep(ManualTestController controller) :
            base(controller)
        {
        }
        public override void CheckStatus()
        {
            if (this.controller.testOperate == TestOperate.Idle)
            {
                this.controller.testResult = TestResult.Pass;
                this.controller.activeDevice.testResult = TestResult.Pass;
                this.controller.testStep = new IdleManualTestStep(this.controller);
            }
        }
        public override void Execute()
        {
            this.controller.StopTest();
            this.CheckStatus();
        }
    }


    public class ManualTestController : TestController
    {
        public TestOperate testOperate { get; set; }
        public ManualTestStep testStep { get; set; }

        public int propertyIndex { get; set; }
        public int configIndex { get; set; }
        public string lastOperateMessage { get; set; }
        public Boolean updated { get; set; }

        public BaseData waitDeviceData { get;set;}

        public Property activeProperty
        {
            get {
                if (this.activeDevice != null)
                {
                    if (this.propertyIndex < this.activeDevice.propertys.Count)
                    {
                        Property property = this.activeDevice.propertys[this.propertyIndex];
                        return property;
                    }
                }
                return null;
            }
        }

        public KeyValuePair<string, string> activeValueConfig
        {
            get {
                if (this.activeProperty != null)
                {
                    if (this.configIndex < this.activeProperty.valueConfig.configs.Count)
                    {
                        KeyValuePair<string, string> config = this.activeProperty.valueConfig.configs[this.configIndex];
                        return config;
                    }
                }
                return new KeyValuePair<string, string>("","");           
            }
        }

        public void ToNextWaitData()
        {
            if (this.activeProperty.valueConfig.dataType == DataType.Range)
            {
                this.propertyIndex += 1;
                if (this.propertyIndex < this.activeDevice.propertys.Count)
                {
                    this.configIndex = 0;
                }
            }
            else if (this.activeProperty.valueConfig.dataType == DataType.Enum)
            {
                this.configIndex += 1;
                if (this.configIndex >= this.activeProperty.valueConfig.configs.Count)
                {
                    this.propertyIndex += 1;
                    this.configIndex = 0;
                }
            }
        
        }

        public Boolean UpdateWaitDeviceData()
        {
            if (this.activeProperty == null)
            {
                return false;
            }
            if (!((this.activeProperty.operates.Contains(Operate.Event) && this.activeProperty.operates.Contains(Operate.Ctrl))))
            {
                this.ToNextWaitData();
                return false;
            }

            KeyValuePair<string, string> config = this.activeProperty.valueConfig.configs[this.configIndex];
            this.waitDeviceData.DeviceId = this.activeDevice.deviceId;
            this.waitDeviceData.Key = this.activeProperty.key;
            this.waitDeviceData.Value = config.Value;
            this.lastOperateMessage = config.Key + " " + this.activeProperty.keyName;
            return true;
        }

        public Boolean finished {
            get {
                if (this.activeDevice!=null)
                {
                    return this.activeProperty == null;
                        
                }
                else
                {
                    return true;
                }
                
            }
        }
        public void NotifyMessage(string message)
        {
           
            this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Wait.ToString(), TestType.Manual, message);
        }


        public void Initialize()
        {
            this.propertyIndex = 0;
            this.configIndex = 0;
            this.waitDeviceData = new BaseData();          
         }


        public ManualTestController(TestManager manager) :
            base(manager)
        {
            this.testOperate = TestOperate.Idle;
            this.Initialize();
            this.testStep = new IdleManualTestStep(this);      
        }

        private void processReportDeviceAttribute(string message)
        {
            CommandContext context = JsonConvert.DeserializeObject<CommandContext>((string)message);
            foreach (BaseData data in context.Data)
            {
                if (this.activeProperty == null)
                {
                    continue;
                }
                switch (this.activeProperty.valueConfig.dataType)
                {
                    case DataType.Range:
                        {
                            if ((this.waitDeviceData.DeviceId == data.DeviceId) && (this.waitDeviceData.Key == data.Key))
                            {
                                this.updated = true;
                            }
                            break;
                        }
                    case DataType.Enum:
                        {
                            if ((this.waitDeviceData.DeviceId == data.DeviceId) &&
                                (this.waitDeviceData.Key == data.Key) &&
                                (this.waitDeviceData.Value == data.Value))
                            {
                                this.updated = true;
                            }
                            break;
                        }
                }
                if (this.updated)
                {
                    break;
                }
               
            }
        }


        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            if (this.testOperate == TestOperate.Idle)
            {
                return;
            }
            switch (notifyEvent)
            {
                case TCPClientChannel.TCP_DATA_EVENT:
                    {
                        if ((flag == ChannelControl.Receive.ToString()) && ((ChannelResult)result == ChannelResult.OK) && message.Length > 0)
                        {
                            try
                            {
                                BaseContext context = JsonConvert.DeserializeObject<BaseContext>((string)message);
                                ControlCommand ctrlCommand = (ControlCommand)Enum.Parse(typeof(ControlCommand), context.Command);
                                switch (ctrlCommand)
                                {
                                    case ControlCommand.Report:
                                        {
                                            CommandType commandType = (CommandType)Enum.Parse(typeof(CommandType), context.Type);
                                            switch (commandType)
                                            {      
                                                case CommandType.Attribute:
                                                case CommandType.Event:
                                                    {
                                                        this.processReportDeviceAttribute(message);
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                            }
                            catch (Newtonsoft.Json.JsonReaderException)
                            {

                            }
                        }
                        break;
                    }
            }


        }

        public override void StartTest()
        {
            base.StartTest();
            this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Start.ToString(), TestType.Manual, "手动测试开始");
            this.Initialize();
            this.testStep = new IdleManualTestStep(this); 
            this.testOperate = TestOperate.Start;

        }
        public override void StopTest()
        {
            base.StopTest();
            this.Notify(TEST_CONTROLLER_MESSAGE_EVENT, TestOperate.Stop.ToString(), TestType.Manual, "手动测试停止");
            this.testOperate = TestOperate.Idle;
        }


        public override void TestProcess()
        {
            while (!this.terminated)
            {
                Thread.Sleep(1);
                if (this.activeDevice != null)
                {
                    this.testStep.Execute();
                }
                else
                {
                    this.StopTest();
                }
            }        
        }
    }

    public class TestControllerPool : BaseTestController
    {

        public AutoTestController autoTestController { get; set; }
        public ManualTestController manualTestController { get; set; }

        public TestControllerPool(TestManager manager):
            base(manager)
        {
            this.autoTestController = new AutoTestController(manager);
            this.manualTestController = new ManualTestController(manager);

            this.autoTestController.AttachObserver(manager.observer.Update);
            this.manualTestController.AttachObserver(manager.observer.Update);
            //侦听deviceController中的tcp返回数据信息
            this.manager.deviceController.AttachObserver(this.manualTestController.observer.Update);            
        }
    
    }




}
