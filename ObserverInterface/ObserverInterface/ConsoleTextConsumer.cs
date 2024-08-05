using System;

namespace ObserverInterface
{
    public class ConsoleTextConsumer : IObserver<string>
    {
        private bool finished = false;

        public void OnCompleted()
        {
            if (finished)
            {
                OnError(new Exception("The consumer already finished it's lifecycle"));
                return;
            }

            finished = true;
            Console.WriteLine("<- End");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("<- Error");
            Console.WriteLine("<- {0}", error.Message);
        }

        public void OnNext(string value)
        {
            if (finished)
            {
                OnError(new Exception("The consumer finished its lifecycle"));
                return;
            }

            Console.WriteLine("-> {0}", value);
            Console.WriteLine("<- Ok");
        }
    }
}
