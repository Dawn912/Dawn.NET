using AutoMapper;
using Dawn.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Dawn.WebAPI.Client
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.AssertConfigurationIsValid();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            if (ex != null)
            {
                string message = ex.Message ?? String.Empty;
                //地址不存在的情况不需要记录日志
                if (message.StartsWith("未找到路径") && message.EndsWith("IController。"))
                {
                    return;
                }
            }
            LogHelper.GetILogInstance().Fatal("Application_Error捕获到异常信息", ex);
        }
    }
}
