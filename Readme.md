# Quartz.Unity #

Quartz.Unity contains several classes to integrate [Quartz.net](http://www.quartz-scheduler.net/) with [Unity](http://unity.codeplex.com/).

Implements a **UnityJobFactoy** that delegates to Unity the creation of Job instances and creates  **UnitySchedulerFactory** to utilize that *IJobFactory*.

There is also an **UnityContainerExtension** that registers the interface *ISchedulerFactory* to resolve a *UnitySchedulerfactory* as a Singleton (ContainerControlledLifetimemanager).

**Usage**

Add the package to your project.

Where you configure your **IUnityContainer** add this line:

    Container.AddNewExtension<QuartzUnityExtension>();

In the class where you will use Quartz, you must inject an instance of IScheduler. When Unity resolves the IScheduler instance, it will be able to crete Job instances based in your container. You can cretate Jobs with explicit dependencies declared in the Job constructor that will be automatically resolved by Unity when the Job is created. 

That's all!

**Internals**

This Unity Extension registers the types **ISchedulerFactory** and **IScheduler** in your container like this:

    Container.RegisterType<ISchedulerFactory, UnitySchedulerFactory>(new ContainerControlledLifetimeManager());
    Container.RegisterType<IScheduler>(new InjectionFactory(c => c.Resolve<ISchedulerFactory>().GetScheduler()));

So the **ISchedulerFactory** is a Singleton managed by the container.
