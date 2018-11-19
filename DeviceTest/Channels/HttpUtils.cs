using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.IO;
using System.Configuration;
using JK.Program.Pattern;
using JK.Channels.Base;





namespace JK.Channels.HttpUtils
{

    

    public  class Controller : Subject
    {
        
        protected HttpClient httpclient { get; set; }
        public string baseUrl { get; set; }
        public string commandUrl { get; set; }

        public Boolean useBaseUrl { get; set; }
        public string URL 
        {
            get {
                if (this.useBaseUrl)
                {
                    return this.baseUrl + this.commandUrl;
                }
                else
                {
                    return this.commandUrl;
                }
            }
        
        }
        public int commandFlag { get; set; }            
        public List<KeyValuePair<string, string>> paramList { get; set; }

        public Thread processor;
        public Boolean updated { get; set; }
        public ResponseResult responseCode { get; set; }
        public string responseText { get; set; }
        public string lastMessage { get; set; }

        public Controller():
            base()
        {
            this.httpclient = new HttpClient();
            this.baseUrl = "";
            this.commandUrl = "";
            this.commandFlag = 0;
            this.useBaseUrl = true;
            
            this.paramList = new List<KeyValuePair<string, string>>();
            
            this.updated = false;
            this.responseCode = ResponseResult.Fail;
            this.responseText = "";
            this.lastMessage = "";

        }


        public Boolean active {
            get {
                return this.responseCode == ResponseResult.Ok;
            }
        }
        public override void ProcessResponse(int notifyEvent, string flag, object result, string message, object sender)
        {
            this.Notify( notifyEvent,  flag,  result,  message);
        }


        public virtual void GetResponseByPost()
        {
            this.responseText = "";
            this.responseCode = ResponseResult.Fail;

            try
            {
                if (this.URL != "")
                {
                    var content = new FormUrlEncodedContent(this.paramList);

                    HttpResponseMessage response = this.httpclient.PostAsync(this.URL, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Stream myResponseStream = response.Content.ReadAsStreamAsync().Result;
                        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                        this.responseText = myStreamReader.ReadToEnd();
                        myStreamReader.Close();
                        myResponseStream.Close();
                        this.responseCode = ResponseResult.Ok;
                        this.lastMessage = "HTTP POST获取返回数据成功";
                    }
                }

            }
            catch (System.ArgumentNullException)
            {
                this.lastMessage = "HTTP参数为空";
                this.responseCode = ResponseResult.Fail;
            }
            catch (System.AggregateException)
            {
                this.lastMessage = "HTTP执行失败";

                this.responseCode = ResponseResult.Fail;

            }
            this.updated = true;
            this.NotifyResponseData();        
        
        }

        public virtual void GetResponseByGet()
        {
            this.responseText = "";
            this.responseCode = ResponseResult.Fail;
            
            try
            {
                if (this.URL != "")
                {
                    string fullUrl = this.URL;
                    string paramText = this.BuildParam(this.paramList);
                    if (paramText!="")
                    {
                        fullUrl += "?"+ paramText;
                    }
                    
                    HttpResponseMessage response = this.httpclient.GetAsync(fullUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Stream myResponseStream = response.Content.ReadAsStreamAsync().Result;
                        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                        this.responseText = myStreamReader.ReadToEnd();
                        myStreamReader.Close();
                        myResponseStream.Close();
                        this.responseCode = ResponseResult.Ok;
                        this.lastMessage = "HTTP GET获取返回数据成功";
                    }
                }

            }
            catch (System.ArgumentNullException )
            {
                this.lastMessage = "HTTP参数为空";
                this.responseCode = ResponseResult.Fail;
            }
            catch (System.AggregateException )
            {
                this.lastMessage = "HTTP执行失败" ;
                
                this.responseCode = ResponseResult.Fail;

            }
            this.updated = true;
            this.NotifyResponseData();
        }


        private string BuildParam(List<KeyValuePair<string, string>> paramArray)
        {
            string url = "";

            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&", item.Key, item.Value);
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;

            }
            return url;
        }

        public virtual void  GetAsync()
        {
            this.GetResponseByGet();
        }
        public virtual void Get()
        {
            this.processor = new Thread(new ThreadStart(this.GetResponseByGet));
            this.processor.IsBackground = true;
            this.processor.Start();          
        }
        public virtual void Terminate()
        {
            this.processor.Abort();
        }

        public virtual void PostAsync()
        {
            this.GetResponseByPost();
        }

        public virtual void Post()
        {
            this.processor = new Thread(new ThreadStart(this.GetResponseByPost));
            this.processor.IsBackground = true;
            this.processor.Start();       
        }
        public void NotifyResponseData()
        {
            this.Notify(this.commandFlag, this.responseCode.ToString(), this.responseText, this.lastMessage);                   
        }
    }




    public class HttpController : Controller
    {
        public HttpController():
            base()
        {
    
        }

    }

    public class FileController:Controller
    {
        public override void GetResponseByPost()
        {
        

         /*   HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");//设置请求头

            
            MultipartFormDataContent mulContent = new MultipartFormDataContent();//创建用于可传递文件的容器

            string path = "D:\\white.png";

            // 读文件流
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            HttpContent fileContent = new StreamContent(fs);//为文件流提供的HTTP容器
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");//设置媒体类型
            mulContent.Add(fileContent, "myFile", "white.png");//这里第二个参数是表单名，第三个是文件名。如果接收的时候用表单名来获取文件，那第二个参数就是必要的了 
            mulContent.Add(new StringContent("253"), "id"); //普通的表单内容用StringContent
            HttpResponseMessage response = client.PostAsync(new Uri(""), mulContent).Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
          */
        }



    }


    




}
