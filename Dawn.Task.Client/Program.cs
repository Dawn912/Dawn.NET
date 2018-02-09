using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Task.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            var command = args != null & args.Length > 0 ? args[0] : "";
            while (true)
            {
                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("请输入命令行:");
                    command = Console.ReadLine();
                }
                if (command == "quit" || command == "exit")
                {
                    return;
                }

                //执行Module下类的方法，例如Test.Test1
                MethodExecutor.Execute(command, args);

                command = string.Empty;
            }
        }
    }
}
