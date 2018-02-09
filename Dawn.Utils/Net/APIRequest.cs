using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using Dawn.Utils.Log;
using Dawn.Extensions.Enum;
using Dawn.Extensions.Json;

namespace Dawn.Utils.Net
{
    public abstract class APIRequest
    {
        protected static readonly ILog log = LogHelper.GetILogInstance();

        /// <summary>
        /// 三方接口域名
        /// </summary>
        protected virtual string APIDomain { get { return String.Empty; } }
        private Dictionary<string, object> GetParamDic(object param)
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            if (param != null)
            {
                IDictionary dic = param as IDictionary;
                if (dic != null)
                {
                    foreach (object key in dic.Keys)
                    {
                        p.Add(key.ToString(), dic[key]);
                    }
                }
                else
                {
                    foreach (PropertyInfo property in param.GetType().GetProperties())
                    {
                        p.Add(property.Name, property.GetValue(param, null));
                    }
                }
            }
            return p;
        }

        private string GetParamString(object param)
        {
            return GetParamString(param, true);
        }
        private string GetParamString(object param, bool isUrlEncode)
        {
            if (param == null)
            {
                return String.Empty;
            }
            if (param.GetType() == typeof(String))
            {
                string str = param.ToString();
                if (isUrlEncode)
                {
                    str = HttpUtility.UrlEncode(str);
                }
                return str;
            }

            Dictionary<string, object> p = GetParamDic(param);
            StringBuilder s = new StringBuilder();
            foreach (var d in p)
            {
                string val = (d.Value ?? String.Empty).ToString();
                if (isUrlEncode)
                {
                    val = HttpUtility.UrlEncode(val);
                }
                s.AppendFormat("&{0}={1}", d.Key, val);
            }
            return s.ToString().TrimStart('&');
        }
        private string GetFormatUrl(string domain, string route)
        {
            if (String.IsNullOrEmpty(route))
            {
                return domain;
            }

            string url = String.Empty;
            domain = domain.TrimEnd('/');
            route = route.TrimStart('/');
            url = String.Format("{0}/{1}", domain, route);
            return url;
        }
        /// <summary>
        /// 标准的第三方接口调用异常格式化字符串
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual string GetExceptionFormat(string message)
        {
            string errorMessage = String.Format("{0}:{1}", ReturnCode.ExternalException.GetDescription(), message);
            return errorMessage;
        }
        protected virtual void BeforeRequesting(HttpWebRequest webRequest) { }
        protected virtual void AfterRequested(HttpWebRequest webRequest) { }
        protected virtual APIResult Post(string domain, string route, ContentType contentType, object param)
        {
            return this.Post(domain, route, contentType, param, null);
        }
        protected virtual APIResult Post(string domain, string route, ContentType contentType, object param, Action<HttpWebRequest> actRequest)
        {
            APIResult result = new APIResult();
            string url = GetFormatUrl(domain, route);
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = contentType.GetDescription();
            webRequest.Method = "POST";
            string postData = String.Empty;
            switch (contentType)
            {
                case ContentType.FormData:
                    break;
                case ContentType.FormUrlEncoded:
                    postData = GetParamString(param, false);
                    break;
                case ContentType.Raw:
                    postData = (param ?? String.Empty).ToJson();
                    break;
                default:
                    break;
            }
            byte[] bs = Encoding.UTF8.GetBytes(postData);
            webRequest.ContentLength = bs.Length;
            if (actRequest != null)
            {
                actRequest(webRequest);
            }
            BeforeRequesting(webRequest);
            using (Stream reqStream = webRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }
            try
            {
                result.RequestUrl = url;
                result.RequestData = postData;
                log.Info(String.Format("APIRequest POST:{0},data:{1}", url, postData));
                Stopwatch watch = Stopwatch.StartNew();
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    watch.Stop();
                    result.Elapsed = watch.Elapsed;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        result.ResponseText = sr.ReadToEnd();
                    }
                }
                AfterRequested(webRequest);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                log.Error(url, ex);
            }
            return result;
        }

        protected virtual APIResult Get(string domain, string route, object param)
        {
            return Get(domain, route, param, ContentType.Raw, Encoding.UTF8, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="route"></param>
        /// <param name="param">请求参数</param>
        /// <param name="header">头文件参数</param>
        /// <returns></returns>
        protected virtual APIResult Get(string domain, string route, object param, object header)
        {
            return Get(domain, route, param, header, ContentType.Raw, Encoding.UTF8, null);
        }

        protected virtual APIResult Get(string domain, string route, object param, ContentType contentType, Encoding encoding, Action<HttpWebRequest> actRequest)
        {
            APIResult result = new APIResult();
            string url = GetFormatUrl(domain, route);
            if (param != null)
            {
                string query = GetParamString(param);
                if (!String.IsNullOrEmpty(query))
                {
                    url = String.Format("{0}?{1}", url, query.TrimStart('/', '?'));
                }
            }
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = contentType.GetDescription();
            webRequest.Method = "GET";
            if (actRequest != null)
            {
                actRequest(webRequest);
            }

            try
            {
                result.RequestUrl = url;
                log.Info(String.Format("APIRequest GET:{0}", url));
                BeforeRequesting(webRequest);
                Stopwatch watch = Stopwatch.StartNew();
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    watch.Stop();
                    result.Elapsed = watch.Elapsed;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        result.ResponseText = sr.ReadToEnd();
                    }
                }
                AfterRequested(webRequest);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                log.Error(url, ex);
            }
            return result;
        }
        /// <summary>
        /// 增加头文件
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="route"></param>
        /// <param name="param"></param>
        /// <param name="header"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <param name="actRequest"></param>
        /// <returns></returns>
        protected virtual APIResult Get(string domain, string route, object param, object header, ContentType contentType, Encoding encoding, Action<HttpWebRequest> actRequest)
        {
            APIResult result = new APIResult();
            string url = GetFormatUrl(domain, route);
            if (param != null)
            {
                string query = GetParamString(param);
                if (!String.IsNullOrEmpty(query))
                {
                    url = String.Format("{0}?{1}", url, query.TrimStart('/', '?'));
                }
            }
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ContentType = contentType.GetDescription();
            webRequest.Method = "GET";
            if (header != null)
            {
                Dictionary<string, object> p = GetParamDic(header);
                foreach (var item in p)
                {
                    webRequest.Headers.Add(item.Key, item.Value.ToString());
                }
                log.Info(p.ToJson());
            }
            if (actRequest != null)
            {
                actRequest(webRequest);
            }

            try
            {
                result.RequestUrl = url;
                log.Info(String.Format("APIRequest GET:{0}", url));
                BeforeRequesting(webRequest);
                Stopwatch watch = Stopwatch.StartNew();
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    watch.Stop();
                    result.Elapsed = watch.Elapsed;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        result.ResponseText = sr.ReadToEnd();
                    }
                }
                AfterRequested(webRequest);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                log.Error(url, ex);
            }
            return result;
        }
    }
}
