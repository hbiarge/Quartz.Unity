namespace Example
{
    using System;

    using Microsoft.Practices.Unity;

    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Triggers;
    using Quartz.Unity;

    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            container.RegisterType<IConfigManager, InMemoryConfigManager>();
            container.AddNewExtension<QuartzUnityExtension>();

            var scheduler = container.Resolve<IScheduler>();

            scheduler.ScheduleJob(
                new JobDetailImpl("TestJob", typeof(MyJob)),
                new CalendarIntervalTriggerImpl("TestTrigger", IntervalUnit.Second, 5));

            scheduler.Start();

            Console.ReadLine();
        }
    }
}
