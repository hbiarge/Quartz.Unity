using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    using System;
    using Unity;
    using Unity.Lifetime;

    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Triggers;
    using Quartz.Unity;

    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new UnityContainer();

            Console.WriteLine("Registering types in Unity container...");
            container.RegisterType<IConfigManager, InMemoryConfigManager>();
            container.RegisterType<IDisposableResource, DisposableResource>(new HierarchicalLifetimeManager());
            container.AddNewExtension<QuartzUnityExtension>();

            Console.WriteLine("Resolving IScheduler instance...");
            var schedulerFactory = container.Resolve<ISchedulerFactory>();

            var scheduler = await schedulerFactory.GetScheduler(CancellationToken.None);

            Console.WriteLine("Scheduling job...");
            await scheduler.ScheduleJob(
                new JobDetailImpl("TestJob", typeof(MyJob)),
                new CalendarIntervalTriggerImpl("TestTrigger", IntervalUnit.Second, 5));

            Console.WriteLine("Starting scheduler...");
            await scheduler.Start();

            Console.WriteLine("60 seconds to run tasks");
            await Task.Delay(TimeSpan.FromSeconds(60));

            await scheduler.Shutdown();
        }
    }
}
