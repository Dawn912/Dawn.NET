using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutoMapper;

namespace Dawn.Utils.Configuration
{
    public static class AppSettings
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static T GetValue<T>(string key)
        {
            return GetValue(key, default(T));
        }
        public static T GetValue<T>(string key, T defaultValue)
        {
            T obj;
            string str = Get(key);
            try
            {
                obj = Mapper.Map<T>(str);
            }
            catch (AutoMapperMappingException)
            {
                obj = defaultValue;
            }
            return obj;
        }
    }
}
