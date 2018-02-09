using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Utils.Net
{
    public enum ReturnCode
    {
        /// <summary>
        /// 提交数据库操作失败
        /// </summary>
        [Description("提交数据库操作失败")]
        SqlException = 10001,

        /// <summary>
        /// 第三方接口调用异常
        /// </summary>
        [Description("第三方接口调用异常")]
        ExternalException = 20001,
        /// <summary>
        /// 第三方接口返回错误
        /// </summary>
        [Description("第三方接口返回错误")]
        ExternalError = 20002,
        /// <summary>
        /// 第三方接口响应正文解析失败
        /// </summary>
        [Description("第三方接口响应正文解析失败")]
        ExternalDeserializeError = 20003,
    }
}
