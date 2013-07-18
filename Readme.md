# Quartz.Unity #

Quartz.Unity contains several classes to integrate [Quartz.net](http://www.quartz-scheduler.net/) with [Unity](http://unity.codeplex.com/).

Implements a **UnityJobFactoy** that delegates to Unity the creation of Job instances and creates  **UnitySchedulerFactory** to utilize that *IJobFactory*.

There is also an **UnityContainerExtension** that registers the interface *ISchedulerFactory* to resolve a *UnitySchedulerfactory* as a Singleton (ContainerControlledLifetimemanager).