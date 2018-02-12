using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Entity
{
    public class CodeValuePair
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
    public class CodeValuePair<T> : CodeValuePair
    {
        public CodeValuePair() { }
        public CodeValuePair(int code, T value)
        {
            this.Code = code;
            this.Value = value;
        }
        public T Value { get; set; }
        public virtual bool HasValue
        {
            get
            {
                return this.Code == 0 && this.Value != null && !this.Value.Equals(default(T));
            }
        }
    }
}
