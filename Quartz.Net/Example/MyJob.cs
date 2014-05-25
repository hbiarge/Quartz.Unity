namespace Example
{
    using System;

    using Quartz;

    public class MyJob : IJob
    {
        private readonly IConfigManager configManager;

        public MyJob(IConfigManager configManager)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException("configManager");
            }

            this.configManager = configManager;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.Write("Job fired! ");
            Console.WriteLine("ConfigManager type is {0}", configManager.GetValue("type"));
        }
    }
}