using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Extensions
{
    public static class EnumExts
    {
        /// <summary>
        /// 获取枚举描述信息（需使用[Description]）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
