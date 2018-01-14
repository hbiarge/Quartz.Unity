// Copyright 2014 Hugo Biarge
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Threading.Tasks;

namespace Quartz.Unity
{
    using System;
    using System.Globalization;

    using Quartz.Spi;

    using global::Unity;

    public class UnityJobFactory : IJobFactory
    {
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
            
            try
            {
                return new JobWrapper(bundle, container);
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


        #region Job Wrappers

        /// <summary>
        ///     Job execution wrapper.
        /// </summary>
        /// <remarks>
        ///     Creates nested lifetime scope per job execution and resolves Job from Autofac.
        /// </remarks>
        internal class JobWrapper : IJob
        {
            private readonly TriggerFiredBundle bundle;
            private readonly IUnityContainer unityContainer;

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            public JobWrapper(TriggerFiredBundle bundle, IUnityContainer unityContainer)
            {
                this.bundle = bundle ?? throw new ArgumentNullException(nameof(bundle));
                this.unityContainer = unityContainer ?? throw new ArgumentNullException(nameof(unityContainer));
            }

            protected IJob RunningJob { get; private set; }

            /// <summary>
            ///     Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
            ///     fires that is associated with the <see cref="T:Quartz.IJob" />.
            /// </summary>
            /// <remarks>
            ///     The implementation may wish to set a  result object on the
            ///     JobExecutionContext before this method exits.  The result itself
            ///     is meaningless to Quartz, but may be informative to
            ///     <see cref="T:Quartz.IJobListener" />s or
            ///     <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
            ///     execution.
            /// </remarks>
            /// <param name="context">The execution context.</param>
            /// <exception cref="SchedulerConfigException">Job cannot be instantiated.</exception>
            public Task Execute(IJobExecutionContext context)
            {
                var childContainer = unityContainer.CreateChildContainer();
                try
                {
                    RunningJob = (IJob)childContainer.Resolve(bundle.JobDetail.JobType);
                    return RunningJob.Execute(context);
                }
                catch (JobExecutionException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new JobExecutionException(string.Format(CultureInfo.InvariantCulture,
                        "Failed to execute Job '{0}' of type '{1}'",
                        bundle.JobDetail.Key, bundle.JobDetail.JobType), ex);
                }
                finally
                {
                    RunningJob = null;
                    childContainer.Dispose();
                }
            }
        }

        #endregion
    }
}
