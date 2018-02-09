using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Utils.Net
{
    public enum ContentType
    {
        /// <summary>
        /// multipart/form-data
        /// </summary>
        [Description("multipart/form-data")]
        FormData,
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        FormUrlEncoded,
        /// <summary>
        /// application/json
        /// </summary>
        [Description("application/json")]
        Raw,
    }
}
