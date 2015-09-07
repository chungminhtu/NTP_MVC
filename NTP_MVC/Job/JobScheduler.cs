using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Job
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail jobSMSSend = JobBuilder.Create<HTDTSMSJob>().Build();

            ITrigger triggerSMSSend = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 30))
                  )
                .Build();




            scheduler.ScheduleJob(jobSMSSend, triggerSMSSend);
            
        }
    }
}