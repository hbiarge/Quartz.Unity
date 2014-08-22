namespace Example
{
    using System;
    using System.Threading;
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

            Console.WriteLine("Registering types in Unity container...");
            container.RegisterType<IConfigManager, InMemoryConfigManager>();
            container.RegisterType<IDisposableResource, DisposableResource>(new HierarchicalLifetimeManager());
            container.AddNewExtension<QuartzUnityExtension>();

            Console.WriteLine("Resolving IScheduler instance...");
            var scheduler = container.Resolve<IScheduler>();

            Console.WriteLine("Scheduling job...");
            scheduler.ScheduleJob(
                new JobDetailImpl("TestJob", typeof(MyJob)),
                new CalendarIntervalTriggerImpl("TestTrigger", IntervalUnit.Second, 5));

            Console.WriteLine("Starting scheduler...");
            scheduler.Start();

            Console.ReadLine();
        }
    }
}
