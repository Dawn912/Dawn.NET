using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.IO;
using Dawn.Utils.Log;

namespace Dawn.WebAPI.Core.Attribute
{
    public class PostActionAttribute : ActionFilterAttribute
    {
        private static readonly ILog log = LogHelper.GetILogInstance("PostAction");
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.Method.Method == "POST")
            {
                string contentType = String.Empty;
                if (request.Content.Headers.ContentType != null)
                {
                    contentType = request.Content.Headers.ContentType.MediaType;
                }
                string url = request.RequestUri.AbsoluteUri;
                string ip = HttpContext.Current.Request.UserHostAddress;

                bool recordData = true; //是否记录postdata数据。
                string postData = String.Empty;
                if (request.RequestUri.ParseQueryString().Get("logdata") == "0")
                {
                    recordData = false;
                }
                if (recordData)
                {  
                    HttpRequest requestBase = HttpContext.Current.Request;                    
                    Task taskst = null;
                    switch (contentType)
                    {
                        /* 
                        case "multipart/form-data":
                            taskst = request.Content.ReadAsStringAsync().ContinueWith(task => { postData = task.Result; });
                            break;
                        case "application/json":
                            taskst = request.Content.ReadAsStringAsync().ContinueWith(task => { postData = task.Result; });
                            break;
                         */
                        case "application/x-www-form-urlencoded":
                            postData = requestBase.Form.ToString();
                            break;
                        default:
                            taskst = request.Content.ReadAsStringAsync().ContinueWith(task => { postData = task.Result; });
                            break;
                    }
                    if (taskst != null)
                    {
                        taskst.Wait();
                    }
                }

                string str = String.Format("cs-uri-stem:{0} contentType:{1} c-ip:{2} data:{3}"
                    , url
                    , contentType
                    , ip
                    , postData);
                log.Info(str);
            }
            base.OnActionExecuting(actionContext);
        }
    }
}