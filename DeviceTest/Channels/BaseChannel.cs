using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JK.Program.Pattern;
using JK.Libs.Storage;

namespace JK.Channels.Base
{
    // 摘要: 
    //     指定发送数据后的结果。
    public enum ResponseResult
    {
        /// 摘要: 
        ///     成功返回。
        Ok = 0,
        ///
        /// 摘要: 
        ///     超时。
        TimeOut = 1,
        ///
        /// 摘要: 
        ///     错误。
        Error = 2,
        ///
        /// 摘要: 
        ///     错误。
        Invalid = 3,
        ///
        /// 摘要: 
        ///     未知。
        Unknown = 4,
        Fail = 5
    }


    public enum ChannelControl
    {
        Init = 0,
        Open = 1,
        Close = 2,
        Send = 3,
        Receive = 4
    }

    public enum ChannelResult
    { 
        OK              = 0,
        Error           = 1,
        CanNotOpen      = 2,
        CanNotClose     = 3,
        InvalidParam    = 4,
        SendError       = 5,
        ReceiveError    = 6,
        Operating       = 7,
        UnknownError    = 10  
    }

    public delegate void ProcessResponse(ResponseResult Result, String Response);
    


    public abstract class abstractChannel:Subject
    {
        abstract public  Boolean Open();
        abstract public  Boolean Close();
        abstract public  Boolean Active();
        abstract public  Boolean SendCommand(String Command);

        abstract public  void LoadFromFile(string fileName);
        abstract public  void SaveToFile(string fileName);

    }

    public class BaseChannel : abstractChannel
    {
        public String Caption { get; set; }
        public String LastErrorMessage { get; set; }  //最后的消息（错误）

        public Boolean Responsed { get; set; }

        public  ProcessResponse processResponse{get;set;}  //返回值处理回调函数


        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            this.Notify(notifyEvent,  flag,  result,  message);
        }


        public override Boolean Open()
        {
            return true;
        }
        public override Boolean Close()
        {
            return true;
        }
        public override Boolean Active()
        {
            return true;
        }
        public override Boolean SendCommand(String Command)
        {
            return true;
        }

        public override void LoadFromFile(string fileName)
        {
            string[] list = IniFiles.GetAllSectionNames(fileName);
            if (!list.Contains(this.Caption))
            {
                this.SaveToFile(fileName);
            }
        }
        public override void SaveToFile(string fileName)
        {
            return; 
        }

        public void SetCallBackOnProcessResponse(ProcessResponse lpProcessResponse)
        {
            processResponse = lpProcessResponse;
        }
    
    }

    public class ChannelObserver : SubjectObserver
    { 


    }



}
