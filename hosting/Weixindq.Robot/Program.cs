using System;
using System.ServiceProcess;

namespace HQ.Search.BuildIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                new weixindaquanRobotService().Start(args);
                Console.WriteLine("已经启动");
                Console.WriteLine("...");
                Console.Read();
            }
            else
            {
                ServiceBase.Run(new weixindaquanRobotService());
            }
        }
    }
}
