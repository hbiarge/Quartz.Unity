using System.Threading.Tasks;

namespace Example
{
    using System;

    using Quartz;

    public class MyJob : IJob
    {
        private readonly IConfigManager configManager;
        private readonly IDisposableResource diposableResource;

        public MyJob(
            IConfigManager configManager,
            IDisposableResource diposableResource)
        {
            this.configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
            this.diposableResource = diposableResource ?? throw new ArgumentNullException(nameof(diposableResource));
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("---------------");
            Console.Write("Job fired! ");
            Console.WriteLine("ConfigManager type is {0}", configManager.GetValue("type"));
            diposableResource.DoSomething();

            return Task.FromResult<object>(null);
        }
    }
}