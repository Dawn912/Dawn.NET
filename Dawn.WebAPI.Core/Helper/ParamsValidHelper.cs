using Dawn.WebAPI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Dawn.WebAPI.Core.Helper
{
    // <summary>
    /// 参数验证类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParamsValidHelper<T>
    {
        public ParamsValidHelper() { }

        public ParamsValidHelper(int returnCode, string message)
        {
            this._returnCode = returnCode;
            this._message = message;
        }
        private string _message;
        private int _returnCode;
        public string Message
        {
            get
            {
                return _message;
            }
        }
        public int ReturnCode
        {
            get
            {
                return _returnCode;
            }
        }

        private readonly string parameterLostMsg = "缺少必要的请求参数:";
        private readonly int parameterLostCode = 101;

        private readonly string parameterFormatMsg = "请求参数格式错误:";
        private readonly int parameterFormatCode = 102;

        public static ParamsValidHelper<T> Instance
        {
            get
            {
                return new ParamsValidHelper<T>();
            }
        }

        #region 验证方法
        public ParamsValidHelper<T> ValidAppId(string _appId)
        {
            return QuickValid(() =>
            {
                if (string.IsNullOrEmpty(_appId))
                {
                    _message = "缺少参数appid";
                    _returnCode = 103;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidNotNull(string name, string value)
        {
            return QuickValid(() =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    _message = string.Concat(parameterLostMsg, name);
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidNotNull(string name, int value)
        {
            return QuickValid(() =>
            {
                if (value <= 0)
                {
                    _message = string.Concat(parameterLostMsg, name);
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }
        public ParamsValidHelper<T> ValidNotNull(string name, long value)
        {
            return QuickValid(() =>
            {
                if (value <= 0)
                {
                    _message = string.Concat(parameterLostMsg, name);
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        /// <summary>
        /// 多个参数不能同时为0
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public ParamsValidHelper<T> ValidNotNull(Dictionary<string, int> dic)
        {
            return QuickValid(() =>
            {
                if (!dic.Any(e => e.Value > 0))
                {
                    _message = string.Format("{0}{1}", parameterLostMsg, string.Join("||", dic.Keys.ToList()));
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        /// <summary>
        /// 多个参数不能同时为0或者空
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public ParamsValidHelper<T> ValidNotNull(Dictionary<string, string> dic)
        {
            return QuickValid(() =>
            {
                if (dic.Count(e => String.IsNullOrEmpty(e.Value) || e.Value == "0") == dic.Count)
                {
                    _message = String.Format("{0}{1}", parameterLostMsg, String.Join("||", dic.Keys));
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidNotNull(string name, object value)
        {
            return QuickValid(() =>
            {
                if (value == null)
                {
                    _message = string.Concat(parameterLostMsg, name);
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        /// <summary>
        /// 验证参数值为列表中的一个
        /// </summary>
        /// <typeparam name="TEntity">列表类型</typeparam>
        /// <param name="vale">参数值</param>
        /// <param name="list">值列表</param>
        /// <returns></returns>
        public ParamsValidHelper<T> ValidInclude<TEntity>(TEntity vale, IEnumerable<TEntity> list)
        {
            return QuickValid(() =>
            {
                if (!list.Contains(vale))
                {
                    _message = string.Format("{0}{1}", parameterFormatMsg, string.Join("||", list));
                    _returnCode = parameterFormatCode;
                }
                return this;
            });
        }

        /// <summary>
        /// 根据业务逻辑验证 
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="func">匿名方法</param>
        /// <returns>验证不通过返回参数名称</returns>
        public ParamsValidHelper<T> Valid(string name, Func<bool> func)
        {
            return QuickValid(() =>
            {
                if (!func())
                {
                    _message = string.Format("{0}{1}", parameterFormatMsg, name);
                    _returnCode = parameterFormatCode;
                }
                return this;
            });
        }
        public ParamsValidHelper<T> Valid(Func<bool> func, int code, string message)
        {
            return QuickValid(() =>
            {
                if (!func())
                {
                    _message = message;
                    _returnCode = code;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidMobileNumber(string phoneNumber)
        {
            return QuickValid(() =>
            {
                Regex regex = new Regex(@"^1\d{10}$");
                if (!regex.IsMatch(phoneNumber))
                {
                    _message = string.Format("{0}{1}", parameterFormatMsg, "电话号码格式不正确");
                    _returnCode = parameterFormatCode;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidLengthRange(string name, string text, int min, int max)
        {
            return QuickValid(() =>
            {
                //一个&#128532;表情算一个字符
                Regex regex = new Regex(@"&#\d+;");
                string str = regex.Replace(text, "x");
                if (str.Length < min || str.Length > max)
                {
                    _message = parameterFormatMsg + String.Format("{0}长度必须在{1}~{2}之间", name, min, max);
                    _returnCode = parameterFormatCode;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidObjectParam(object obj)
        {
            return QuickValid(() =>
            {
                if (obj == null)
                {
                    _message = parameterLostMsg;
                    _returnCode = parameterLostCode;
                }
                return this;
            });
        }

        public ParamsValidHelper<T> ValidDbDate(string name, DateTime value)
        {
            return Valid(name, () => value >= SqlDateTime.MinValue.Value);
        }

        #endregion

        /// <summary>
        /// 验证是否通过
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return _returnCode == 0 && string.IsNullOrEmpty(_message);
        }

        private ParamsValidHelper<T> QuickValid(Func<ParamsValidHelper<T>> func)
        {
            if (!IsValid())
            {
                return this;
            }
            return func();
        }

        /// <summary>
        /// 验证不通过时返回无效信息
        /// </summary>
        /// <returns></returns>
        public ActionInfo<T> ShowInValidMessage()
        {
            return new ActionInfo<T>() { message = this._message, returncode = this._returnCode };
        }

        public static ActionInfo<T> PostParamNull
        {
            get
            {
                return new ActionInfo<T>() { message = "缺少必要的请求参数", returncode = 101 };
            }
        }
    }
}