using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ObservableInterface
{
    public class ConsoleIntegerProducer : IObservable<int>, IDisposable
    {
        private readonly List<IObserver<int>> _subscriberList = new List<IObserver<int>>();
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly Task _workerTask;

        public ConsoleIntegerProducer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = new CancellationToken();
            _workerTask = Task.Factory.StartNew(OnInnerWorker, _cancellationToken);
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (_subscriberList.Contains(observer))
            {
                throw new ArgumentException("The observer is already subscribed to this observable.");
            }

            Console.WriteLine("Subscribing for {0}", observer.GetHashCode());
            _subscriberList.Add(observer);
            return null;
        }

        private void OnInnerWorker()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var input = Console.ReadLine();

                foreach (var observer in _subscriberList)
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        break;
                    }
                    else if (input.Equals("Exit"))
                    {
                        _cancellationTokenSource.Cancel();
                        break;
                    }
                    else if (!int.TryParse(input, out int value))
                    {
                        observer.OnError(new FormatException("Unable to parse given value"));
                    }
                    else
                    {
                        observer.OnNext(value);
                    }
                }
            }
            _cancellationToken.ThrowIfCancellationRequested();
        }

        public void Dispose()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                while (!_workerTask.IsCanceled)
                {
                    Thread.Sleep(1000);
                }
            }

            _cancellationTokenSource.Dispose();
            _workerTask.Dispose();

            foreach (var observer in _subscriberList)
            {
                observer.OnCompleted();
            }
        }

        public void Wait()
        {
            while (!(_workerTask.IsCompleted || _workerTask.IsCanceled))
            {
                Thread.Sleep(1000);
            }
        }
    }
}
