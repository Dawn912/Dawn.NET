using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.WebAPI.Core.Contracts
{
    [DataContract]
    public class ActionInfo<T>
    {
        /// <summary>
        /// 公共返回码
        /// </summary>
        [DataMember(Order = 1)]
        public int returncode { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [DataMember(Order = 2)]
        public string message { get; set; }

        /// <summary>
        /// 结果对象
        /// </summary>
        [DataMember(Order = 3)]
        public ActionData data { get; set; }

        public void SetRowCount()
        {
            try
            {
                data.rowcount = data.list.Count();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 返回单个实体的结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ActionInfo<T> SingleResult(T obj)
        {
            ActionInfo<T> result = new ActionInfo<T>()
            {
                message = String.Empty,
                data = new ActionInfo<T>.ActionData()
                {
                    list = new List<T>()
                    {
                        obj
                    },
                    pagecount = 1,
                    pageindex = 1,
                    rowcount = 1,
                },
            };
            return result;
        }
        public static ActionInfo<T> SingleResult(T obj, Dictionary<int, string> dic, int code)
        {
            ActionInfo<T> result = SingleResult(obj);
            result.returncode = code;
            string message = String.Empty;
            if (dic.ContainsKey(code))
            {
                message = dic[code];
            }
            result.message = message;
            return result;
        }

        /// <summary>
        /// 不分页的结果
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ActionInfo<T> ListResult(IEnumerable<T> list)
        {
            if (list == null)
            {
                return EmptyResult();
            }
            ActionInfo<T> result = new ActionInfo<T>()
            {
                message = String.Empty,
                data = new ActionInfo<T>.ActionData()
                {
                    list = list,
                    pagecount = 1,
                    pageindex = 1,
                    rowcount = list.Count(),
                },
            };
            return result;
        }

        /// <summary>
        /// 分页的结果
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static ActionInfo<T> PagingResult(IEnumerable<T> list, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var part = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            ActionInfo<T> result = new ActionInfo<T>()
            {
                message = String.Empty,
                data = new ActionInfo<T>.ActionData()
                {
                    list = part,
                    pagecount = (list.Count() + pageSize - 1) / pageSize,
                    pageindex = pageIndex,
                    rowcount = list.Count(),
                },
            };
            return result;
        }

        public static ActionInfo<T> EmptyResult(string message = "", int code = 0)
        {
            ActionInfo<T> result = new ActionInfo<T>()
            {
                message = message,
                returncode = code,
                data = new ActionInfo<T>.ActionData()
                {
                    list = new List<T>(),
                    pagecount = 1,
                    pageindex = 1,
                    rowcount = 0,
                },
            };
            return result;
        }

        /// <summary>
        /// 记录实体
        /// </summary>
        [DataContract]
        public class ActionData
        {
            /// <summary>
            /// 总行数
            /// </summary>
            [DataMember(Order = 1)]
            public int rowcount { get; set; }

            /// <summary>
            /// 总页数
            /// </summary>
            [DataMember(Order = 2)]
            public int pagecount { get; set; }

            /// <summary>
            /// 当前页数
            /// </summary>
            [DataMember(Order = 3)]
            public int pageindex { get; set; }

            /// <summary>
            /// 详细信息列表
            /// </summary>
            [DataMember(Order = 4)]
            public IEnumerable<T> list { get; set; }
        }
    }
}
