using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Text;
using Dawn.WebAPI.Core.Contracts;
using log4net;
using Dawn.Utils.Log;

namespace Dawn.WebAPI.Core.Attribute
{

    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        protected static readonly ILog log = LogHelper.GetILogInstance();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var expection = actionExecutedContext.Exception;
            log.Fatal(expection.Message, expection);

            var error = ActionInfo<object>.EmptyResult(expection.Message, (int)HttpStatusCode.InternalServerError);
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError,
                  error);
            base.OnException(actionExecutedContext);
        }
    }
}