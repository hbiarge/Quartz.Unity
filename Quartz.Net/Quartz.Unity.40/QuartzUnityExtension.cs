namespace Quartz.Unity
{
    using Microsoft.Practices.Unity;

    public class QuartzUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ISchedulerFactory, UnitySchedulerFactory>(new ContainerControlledLifetimeManager());
        }
    }
}
