
using log4net.Repository.Hierarchy;

namespace Baike.Robot.Core
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

                    ////Logger.Info("ImgService start");
                    ////ImgService imgservice = new ImgService(1);
                    ////imgservice.Main();

                    ////Logger.Info("ImgService end");

                    var main = new MainService(1);
                    main.Site();

                    var listController = new ListController(1);
                    listController.BuildAllList();

                    Logger.Info("ContentController start");
                    var contentController = new ContentController(1);
                    contentController.BuildAllContent();
                    Logger.Info("ContentController end");

                    var homeController = new HomeController(1);
                    homeController.Index();

                    //-------------------------------
                    Logger.Info("MainService2 end");
                    main = new MainService(2);
                    main.Site();

                    listController = new ListController(2);
                    listController.BuildAllList();

                    Logger.Info("ContentController start");
                    contentController = new ContentController(2);
                    contentController.BuildAllContent();
                    Logger.Info("ContentController end");

                    homeController = new HomeController(2);
                    homeController.Index();

                }

                catch (Exception ex)
                {
                    ////包括记录异常的内部包含异常
                    while (ex != null)
                    {
                        ex = ex.InnerException;
                        Log.service.Logger.Error(ex);
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
