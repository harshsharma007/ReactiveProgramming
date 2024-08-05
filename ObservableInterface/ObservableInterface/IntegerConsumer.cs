using System;

namespace ObservableInterface
{
    public class IntegerConsumer : IObserver<int>
    {
        private readonly int _validDivider;
        private bool _finished = false;

        public IntegerConsumer(int validDivider)
        {
            _validDivider = validDivider;
        }

        public void OnCompleted()
        {
            if (_finished)
            {
                OnError(new Exception("This consumer already finished it's lifecycle"));
            }
            else
            {
                _finished = true;
                Console.WriteLine("{0}: End", GetHashCode());
            }
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("{0}: {1}", GetHashCode(), error.Message);
        }

        public void OnNext(int value)
        {
            if (_finished)
            {
                OnError(new Exception("This consumer finished it's lifecycle"));
            }
            else if (value % _validDivider == 0)
            {
                Console.WriteLine("{0}: {1} divisible by {2}", GetHashCode(), value, _validDivider);
            }
        }
    }
}
