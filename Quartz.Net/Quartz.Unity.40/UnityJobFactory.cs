namespace Quartz.Unity
{
    using System;
    using System.Globalization;

    using Common.Logging;

    using Microsoft.Practices.Unity;

    using Quartz.Spi;

    public class UnityJobFactory : IJobFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UnityJobFactory));
        private readonly IUnityContainer container;

        static UnityJobFactory()
        {
        }

        public UnityJobFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;

            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug(string.Format(
                        CultureInfo.InvariantCulture,
                        "Producing instance of Job '{0}', class={1}", new object[] { jobDetail.Key, jobType.FullName }));
                }

                return this.container.Resolve(jobType) as IJob;
            }
            catch (Exception ex)
            {
                throw new SchedulerException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Problem instantiating class '{0}'", new object[] { jobDetail.JobType.FullName }), ex);
            }
        }

        public void ReturnJob(IJob job)
        {
            // Nothing here. Unity does not maintain a handle to container created instances.
        }
    }
}
