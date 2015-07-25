using System.ServiceProcess;

namespace HQ.Search.BuildIndex
{
    using Baike.Robot.Core;

    partial class BaikeRobotService : ServiceBase
    {
        private static Task _task;

        public BaikeRobotService()
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
