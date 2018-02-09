using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dawn.Extensions.Linq;

namespace Dawn.Extensions.Data
{
    public static class DataExts
    {
        public static List<T> ToList<T>(this DataSet ds)
        {
            List<T> list = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                list = Mapper.DynamicMap<List<T>>(ds.CreateDataReader());                
            }
            return list.EmptyIfNull();
        }
    }
}
