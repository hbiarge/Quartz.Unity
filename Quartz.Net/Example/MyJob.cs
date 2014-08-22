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
            if (configManager == null)
            {
                throw new ArgumentNullException("configManager");
            }

            if (diposableResource == null)
            {
                throw new ArgumentNullException("diposableResource");
            }

            this.configManager = configManager;
            this.diposableResource = diposableResource;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("---------------");
            Console.Write("Job fired! ");
            Console.WriteLine("ConfigManager type is {0}", configManager.GetValue("type"));
            diposableResource.DoSomething();
        }
    }
}