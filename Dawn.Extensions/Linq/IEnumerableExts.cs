using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Extensions.Linq
{
    public static class IEnumerableExts
    {
        /// <summary>
        /// 指示集合不为null，并且包含至少一个以上的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool NotEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
        /// <summary>
        /// 当序列为null时，返回空元素序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                source = Enumerable.Empty<T>();
            }
            return source.ToList();
        }

        public static Dictionary<TKey, TSource> GroupToDictionary<TSource, TKey>(this IEnumerable<TSource> source
            , Func<TSource, TKey> keySelector
            , Func<IGrouping<TKey, TSource>, IOrderedEnumerable<TSource>> orderBy)
        {
            Dictionary<TKey, TSource> dic = source
                .GroupBy(keySelector)
                .Select(g =>
                {
                    if (orderBy != null)
                    {
                        return orderBy(g).First();
                    }
                    else
                    {
                        return g.First();
                    }
                })
                .ToDictionary(keySelector);
            return dic;
        }
    }
}
