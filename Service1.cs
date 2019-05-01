using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net.Core;
using System.Net.Mail;

namespace MyWinService
{
    public partial class ScheduledWinService : ServiceBase
    {
        System.Timers.Timer _timer;
        TimeSpan startTime;
        List<string> list = null;
        private static  ILog log;

        internal void start()
        {
            this.OnStart(null);

        }

        public ScheduledWinService()
        {
            InitializeComponent();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _timer = new System.Timers.Timer();
            list = new List<string>();
            startTime = TimeSpan.Parse("04:37:30");
            log.Info("Start log INFO...: init ctor");
        }

        protected override void OnStart(string[] args)
        {
            // For first time, set amount of seconds between current time and schedule time
            _timer.Enabled = true;
            _timer.Interval = GetNextInterval(startTime, DateTime.Now.TimeOfDay);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            log.Info("Start log INFO...: onstart");



        }

        protected override void OnStop()
        {
        }
        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Warn("Start log WARN...: elapsed");

            _timer.Interval = GetNextInterval(startTime, DateTime.Now.TimeOfDay);
            log.Error("Start log ERROR...: end elapsed ");
            log.Fatal("Start log FATAL... : fatal elapsed");

            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            MemoryAppender mappender = hierarchy.Root.GetAppender("MemoryAppender") as MemoryAppender;


        }

        protected double GetNextInterval(TimeSpan startTime, TimeSpan timeOfDay)
        {
            TimeSpan res;
            if (startTime < timeOfDay)
            {
                res = startTime.Add(new TimeSpan(23, 59, 59)).Subtract(timeOfDay);
            }
            else
            {
                res = startTime.Subtract(timeOfDay);
            }
            return res.TotalMilliseconds;
        }
    }
}
