using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Extensions.Json
{

    public static class JsonObjectExts
    {
        /// <summary>
        /// json转换设置
        /// </summary>
        private static JsonSerializerSettings _jsonSettings;

        /// <summary>
        /// json时间格式
        /// </summary>
        private static IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
        /// <summary>
        /// 构造函数
        /// </summary>
        static JsonObjectExts()
        {

            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = NullValueHandling.Include;
            _jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>  
        public static string ToJson(this object obj)
        {
            try
            {
                string res = JsonConvert.SerializeObject(obj, _jsonSettings);
                return (string.IsNullOrEmpty(res) || res == "\"\"") ? string.Empty : res;
            }
            catch (Exception)
            {
                return string.Empty;
            }


        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <param name="jsonSettings">json转换设置</param>
        /// <returns></returns>  
        public static string ToJson(this object obj, JsonSerializerSettings jsonSettings)
        {
            try
            {
                string res = JsonConvert.SerializeObject(obj, jsonSettings);
                return (string.IsNullOrEmpty(res) || res == "\"\"") ? string.Empty : res;
            }
            catch (Exception)
            {
                return string.Empty;
            }


        }


        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。反序列化为DataTable时各个数据项不能为空
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {

            try
            {
                if (string.IsNullOrEmpty(json))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

    }
}
