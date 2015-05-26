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

            IJobDetail jobSMSStatistic = JobBuilder.Create<HTDTSMSStatisticJob>().Build();

            ITrigger triggerSMSStatistic = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(16, 06))
                  )
                .Build();

            IJobDetail jobHTDTGeneratePatientDue = JobBuilder.Create<HTDTGeneratePatientDue>().Build();

            ITrigger triggerHTDTGeneratePatientDue = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(14, 41))
                  )
                .Build();

            IJobDetail jobGeneratePatientReminder = JobBuilder.Create<HTDTPreparePatientReminder>().Build();

            ITrigger triggerGeneratePatientReminder = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(14, 58))
                  )
                .Build();

            IJobDetail jobDrugReminder = JobBuilder.Create<HTDTDrugReminderJob>().Build();

            ITrigger triggerDrugReminder = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(15, 40))
                  )
                .Build();

            IJobDetail jobExamReminder = JobBuilder.Create<HTDTExamReminderJob>().Build();

            ITrigger triggerExamReminder = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(19, 30))
                  )
                .Build();

            

            //scheduler.ScheduleJob(jobSMSStatistic, triggerSMSStatistic);
            //scheduler.ScheduleJob(jobHTDTGeneratePatientDue, triggerHTDTGeneratePatientDue);
            //scheduler.ScheduleJob(jobGeneratePatientReminder, triggerGeneratePatientReminder);
            //scheduler.ScheduleJob(jobDrugReminder, triggerDrugReminder);
            //scheduler.ScheduleJob(jobExamReminder, triggerExamReminder);
            
        }
    }
}