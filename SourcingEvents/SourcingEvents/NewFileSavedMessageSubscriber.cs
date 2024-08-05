using System;

namespace SourcingEvents
{
    public class NewFileSavedMessageSubscriber : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("-> End");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("-> {0}", error.Message);
        }

        public void OnNext(string value)
        {
            Console.WriteLine("-> {0}", value);
        }
    }
}
