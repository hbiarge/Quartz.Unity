namespace Example
{
    using System;

    public class DisposableResource : IDisposableResource
    {
        public void DoSomething()
        {
            Console.WriteLine("Doing something with disposable resource...");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing disposable resource");
        }
    }
}