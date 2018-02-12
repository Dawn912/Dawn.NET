using Dawn.WebAPI.Core;
using Dawn.WebAPI.Core.Attribute;
using Dawn.WebAPI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Dawn.WebAPI.Client.Controllers
{
    [ApplicationId]
    public class DemoController : BaseController
    {
        [Route("demo/Hello")]
        [HttpGet]
        public ActionInfo<object> Hello()
        {
            return ActionInfo<object>.EmptyResult("Hello, my friend");
        }
    }
}