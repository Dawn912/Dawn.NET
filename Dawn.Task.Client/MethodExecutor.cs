using Dawn.Task.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Task.Client
{
    public static class MethodExecutor
    {
        public static void Execute(string typeName, string[] args)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                Console.WriteLine("请输入类型名");
                return;
            }

            var assembly = Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;
            IModule obj = assembly.CreateInstance(string.Format("{0}.Module.{1}", assemblyName, typeName)) as IModule;

            if (null == obj)
            {
                Console.WriteLine("无法找到对应类型:{0}", typeName);
                return;
            }
            obj.Execute(args);
        }
    }
}
