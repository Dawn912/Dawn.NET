using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using log4net;
using Dawn.WebAPI.Core.Helper;
using Dawn.Utils.Log;
using Dawn.WebAPI.Core.Contracts;

namespace Dawn.WebAPI.Core
{
    public abstract class BaseController : ApiController
    {
        protected static readonly ILog log = LogHelper.GetILogInstance();
        private static readonly string appIdName = "appid";

        #region code-message
        private static readonly Dictionary<int, string> DefaultDicResult = new Dictionary<int, string>() 
        {
            {10001, "提交数据库操作失败"},
            {10002, "指定的数据不存在"},
        };
        protected virtual string GetMessage(int code)
        {
            return GetMessage(DefaultDicResult, code);
        }
        protected virtual string GetMessage(Dictionary<int, string> dic, int code)
        {
            string message = String.Empty;
            if (dic.ContainsKey(code))
            {
                message = dic[code];
            }
            return message;
        }
        #endregion

        protected virtual string AppId
        {
            get
            {
                string result = Request.RequestUri.ParseQueryString().Get(appIdName);
                return result;
            }
        }

        protected virtual ActionInfo<object> NoneQueryAction(object param, Func<ParamsValidHelper<object>> verify, Func<int> biz, Dictionary<int, string> msgDic)
        {
            if (param == null)
            {
                return ParamsValidHelper<object>.PostParamNull;
            }
            var valid = verify();
            if (!valid.IsValid())
            {
                return valid.ShowInValidMessage();
            }

            var result = ActionInfo<object>.EmptyResult();
            int code = biz();
            if (code != 0)
            {
                string message = GetMessage(msgDic, code);
                result.returncode = code;
                result.message = message;
            }
            return result;
        }

        /* PairResult<T>
        protected virtual ActionInfo<T> PairResult<T>(Dawn.Entity.Api.CodeValuePair<T> pair)
        {
            var result = ActionInfo<T>.EmptyResult();
            if (pair.Code == 0)
            {
                result = ActionInfo<T>.SingleResult(pair.Value);
            }
            else
            {
                result.returncode = pair.Code;
                result.message = pair.Message;
            }
            return result;
        }
        */
    }
}