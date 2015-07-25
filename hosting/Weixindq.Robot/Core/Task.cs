
using log4net.Repository.Hierarchy;

namespace Weixindq.Robot.Core
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;

    using Baike.Dataservice;
    using Baike.Pagebuild;

    /// <summary>
    /// 任务调度
    /// </summary>
    public class Task
    {
        #region fields

        public static Thread _thread;
        private readonly TimeSpan _timeSpan;

        #endregion

        #region .ctor

        public Task()
        {
            var interval = ConfigurationManager.AppSettings["interval"].ToString(CultureInfo.InvariantCulture);

            var date = DateTime.ParseExact(interval, "HH:mm:ss", null);

            _timeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
        }

        #endregion

        #region Medthods

        public void Run()
        {
            _thread = new Thread(new ThreadStart(this.RunJob)) { Name = "RunJob_Thread" };
            _thread.Start();
        }

        /// <summary>
        /// The run job.
        /// </summary>
        private void RunJob()
        {
            while (true)
            {
                try
                {
                    Logger.Info("wXmpService start");
                    ////采集
                    var wXmpService = new WXmpService(1);

                    wXmpService.Main();

                    Logger.Info("wXmpService end");

                    Logger.Info("ContentController start");
                    ////内容页
                    var contentController = new ContentController(1);
                    contentController.BuildAllContent();
                    ////列表页
                    Logger.Info("ListController start");
                    var listController = new ListController(1);
                    listController.BuildAllList();


                    Logger.Info("HomeController start");
                    ////首页
                    var homeController = new HomeController(1);
                    homeController.Index();

                    Logger.Info("end");
                }

                catch (Exception ex)
                {
                    ////包括记录异常的内部包含异常
                    while (ex != null)
                    {
                        ex = ex.InnerException;
                        Logger.Error(ex);
                    }
                }

                Thread.Sleep(this._timeSpan);
            }
        }

        /// <summary>
        /// The stop.
        /// </summary>
        public void Stop()
        {
            _thread.Abort();
        }

        #endregion
    }
}
