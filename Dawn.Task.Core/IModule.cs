using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Task.Core
{
    public interface IModule
    {
        void Execute(string[] args);
    }
}
