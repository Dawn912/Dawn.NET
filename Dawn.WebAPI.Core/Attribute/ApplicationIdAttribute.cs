using Dawn.WebAPI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Dawn.Extensions.Json;

namespace Dawn.WebAPI.Core.Attribute
{
    /// <summary>
    /// 表示需要提供业务id的请求，QueryString中必须包含appid参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApplicationIdAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
         {
            var request = actionContext.Request;
            string appid = request.RequestUri.ParseQueryString().Get("appid");
            return !String.IsNullOrEmpty(appid);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            ActionInfo<object> result = new ActionInfo<object>()
            {
                message = "缺少参数appid",
                returncode = 103, //详见http://note.youdao.com/yws/public/redirect/share?id=38fe5a7f7e2e396a135bcb75eeff740f&type=false
            };
            string jsonData = result.ToJson();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            actionContext.Response.Content = content;
        }
    }
}