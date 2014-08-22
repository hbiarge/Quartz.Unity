namespace Example
{
    using System;

    public interface IDisposableResource : IDisposable
    {
        void DoSomething();
    }
}