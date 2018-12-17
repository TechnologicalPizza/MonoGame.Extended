﻿using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace MonoGame.Extended.Testing
{
    public class ResourceCache : IResourceRequester
    {
        private AutoResetEvent _requestEvent;
        private ConcurrentQueue<ResourceRequest> _requests;
        private ConcurrentQueue<ResourceRequest> _priorityRequests;
        private ConcurrentDictionary<Uri, ResourceRequest> _responses;

        private Thread[] _threads;
        private Worker[] _downloadWorkers;

        public bool IsDisposed { get; private set; }
        public bool IsRunning { get; private set; }
        public ReadOnlyCollection<Worker> Threads { get; }

        public ResourceCache()
        {
            _requestEvent = new AutoResetEvent(false);
            _requests = new ConcurrentQueue<ResourceRequest>();
            _priorityRequests = new ConcurrentQueue<ResourceRequest>();
            _responses = new ConcurrentDictionary<Uri, ResourceRequest>();

            _threads = new Thread[1];
            _downloadWorkers = new Worker[_threads.Length];
            Threads = new ReadOnlyCollection<Worker>(_downloadWorkers);
            for (int i = 0; i < _threads.Length; i++)
            {
                int id = i + 1;
                _downloadWorkers[i] = new Worker(id);
                _threads[i] = new Thread(WorkerThread)
                {
                    Name = "ResourceCache Worker " + id
                };
            }
        }

        public void Start()
        {
            if (IsRunning)
                return;
            IsRunning = true;

            for (int i = 0; i < _threads.Length; i++)
            {
                Worker worker = _downloadWorkers[i];
                _threads[i].Start(worker);
            }
        }

        public IResponseStatus Request(string uri, string accept, bool prioritized, OnResponseDelegate onResponse, OnErrorDelegate onError)
        {
            throw new NotImplementedException();
        }

        public IResponseStatus Request(Uri uri, string accept, bool prioritized, OnResponseDelegate onResponse, OnErrorDelegate onError)
        {
            throw new NotImplementedException();
        }
        
        private bool DequeueRequest(out ResourceRequest request)
        {
            if (_priorityRequests.TryDequeue(out request))
                return true;

            if (_requests.TryDequeue(out request))
                return true;

            return false;
        }

        private void WorkerThread(object obj)
        {
            var worker = obj as Worker;
            while (IsRunning)
            {
                if (!_requestEvent.WaitOne(10))
                    continue;

                while (DequeueRequest(out ResourceRequest resourceRequest))
                {
                    var url = resourceRequest.Url;
                    _responses.TryAdd(url, resourceRequest);
                    worker.CurrentRequest = resourceRequest;

                    try
                    {
                        var headers = new WebHeaderCollection();
                        headers.Add(HttpRequestHeader.Accept, resourceRequest.Accept);

                        //var stream = new FileStream();
                        //resourceRequest.HandleOnResponse(stream, headers);
                    }
                    catch (Exception exc)
                    {
                        resourceRequest.OnError?.Invoke(url, exc);
                    }

                    worker.CurrentRequest = null;
                    _responses.TryRemove(url, out var finishedRequest);
                    finishedRequest.Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsRunning = false;

                if (IsRunning)
                {
                    if (_responses != null)
                    {
                        foreach (var response in _responses)
                            response.Value.Dispose();
                    }

                    if (_threads != null)
                    {
                        for (int i = 0; i < _threads.Length; i++)
                            _threads[i].Join();
                        _threads = null;
                    }
                }

                _requestEvent?.Dispose();
                _responses = null;

                IsDisposed = true;
            }
        }

        public class Worker
        {
            public int ID { get; }
            public IResponseStatus CurrentRequest { get; internal set; }

            internal Worker(int id)
            {
                ID = id;
            }
        }
    }
}
