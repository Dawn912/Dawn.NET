using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Utils.Net
{
    public class APIResult
    {
        public APIResult()
        {
            this.ResponseText = String.Empty;
        }

        private string _responseText;
        /// <summary>
        /// 来自 Internet 资源的响应文本。
        /// </summary>
        public string ResponseText
        {
            get { return _responseText ?? String.Empty; }
            set { _responseText = value; }
        }
        /// <summary>
        /// 在请求第三方资源过程中发生的异常
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 指示在请求第三方资源过程中是否发生了异常
        /// </summary>
        public bool HasException
        {
            get
            {
                return this.Exception != null;
            }
        }
        /// <summary>
        /// 实际的请求url
        /// </summary>
        public string RequestUrl { get; set; }

        public string RequestData { get; set; }
        /// <summary>
        /// 调用第三方资源的总运行时间
        /// </summary>
        public TimeSpan Elapsed { get; set; }

        public override string ToString()
        {
            return ResponseText;
        }
    }
}
