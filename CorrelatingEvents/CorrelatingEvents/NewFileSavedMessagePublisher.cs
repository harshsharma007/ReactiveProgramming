using System;
using System.Collections.Generic;
using System.IO;

namespace CorrelatingEvents
{
    public sealed class NewFileSavedMessagePublisher : IObservable<string>, IDisposable
    {
        private readonly FileSystemWatcher _watcher;
        private readonly List<IObserver<string>> _subscriberList = new List<IObserver<string>>();

        public NewFileSavedMessagePublisher(string path)
        {
            this._watcher = new FileSystemWatcher(path);
            this._watcher.Created += OnFileCreated;
            this._watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            foreach (var observer in _subscriberList)
            {
                observer.OnNext(e.FullPath);
            }
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            _subscriberList.Add(observer);
            return null;
        }

        public void Dispose()
        {
            this._watcher.EnableRaisingEvents = false;
            this._watcher.Created -= OnFileCreated;
            this._watcher.Dispose();

            foreach (var observer in _subscriberList)
            {
                observer.OnCompleted();
            }
        }
    }
}
