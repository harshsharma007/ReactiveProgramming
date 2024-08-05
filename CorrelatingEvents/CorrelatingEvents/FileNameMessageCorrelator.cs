using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace CorrelatingEvents
{
    public sealed class FileNameMessageCorrelator : IObservable<string>, IObserver<string>, IDisposable
    {
        private bool hasCompleted = false;
        private readonly NameValueCollection _correlations = new NameValueCollection();
        private readonly Func<string, string> _correlationKeyExtractor;
        private readonly List<IObserver<string>> _observersList = new List<IObserver<string>>();

        public FileNameMessageCorrelator(Func<string, string> correlationKeyExtractor)
        {
            _correlationKeyExtractor = correlationKeyExtractor;
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            _observersList.Add(observer);
            return null;
        }

        public void OnCompleted()
        {
            hasCompleted = true;

            foreach (var observer in _observersList)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            if (!hasCompleted)
            {
                foreach (var observer in _observersList)
                {
                    observer.OnError(error);
                }
            }
        }

        public void OnNext(string value)
        {
            if (hasCompleted) return;

            Console.WriteLine("Parsing message: {0}", value);
            var correlationId = _correlationKeyExtractor(value);

            if (correlationId == null) return;

            _correlations.Add(correlationId, value);

            if (_correlations.GetValues(correlationId).Count() == 2)
            {
                var fileData = _correlations.GetValues(correlationId).Select(File.ReadAllText).ToArray();
                var newValue = string.Join("|", fileData);

                foreach (var observer in _observersList)
                {
                    observer.OnNext(newValue);
                }
                _correlations.Remove(correlationId);
            }
        }

        public void Dispose()
        {
            OnCompleted();
        }
    }
}
