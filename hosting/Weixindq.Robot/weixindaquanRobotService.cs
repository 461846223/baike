using System.ServiceProcess;

namespace HQ.Search.BuildIndex
{
    using Weixindq.Robot.Core;

    partial class weixindaquanRobotService : ServiceBase
    {
        private static Task _task;

        public weixindaquanRobotService()
        {
            InitializeComponent();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            _task = new Task();
            _task.Run();
        }

        protected override void OnStop()
        {
            _task.Stop();
        }
    }
}
