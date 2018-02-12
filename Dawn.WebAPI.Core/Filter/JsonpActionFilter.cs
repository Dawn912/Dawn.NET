using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace Dawn.WebAPI.Core.Filter
{
    public sealed class JsonpActionFilter : ActionFilterAttribute
    {
        private const string _callbackQueryParameter = "_callback";
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            string callback;

            if (context.Response != null && context.Response.Content != null && context.Response.StatusCode == HttpStatusCode.OK && IsJsonp(out callback))
            {
                var jsonBuilder = new StringBuilder(callback);
                jsonBuilder.AppendFormat("({0})", context.Response.Content.ReadAsStringAsync().Result);
                context.Response.Content = new StringContent(jsonBuilder.ToString());
            }

            base.OnActionExecuted(context);
        }

        private bool IsJsonp(out string callback)
        {
            callback = HttpContext.Current.Request.QueryString[_callbackQueryParameter];

            return !string.IsNullOrEmpty(callback);
        }
    }
}