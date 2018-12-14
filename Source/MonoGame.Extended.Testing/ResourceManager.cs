﻿using System;

namespace MonoGame.Extended.Testing
{
    public class ResourceManager : IResourceRequester
    {
        private ResourceDownloader _downloader;
        
        public bool IsDisposed => _downloader.IsDisposed;
        public bool IsRunning => _downloader.IsRunning;

        public ResourceDownloader Downloader => _downloader;

        public ResourceManager()
        {
            _downloader = new ResourceDownloader();
            _downloader.Start();
        }

        public IResponseStatus Request(string uri, OnResponseDelegate onResponse, OnErrorDelegate onError)
        {
            return _downloader.Request(uri, onResponse, onError);
        }

        public IResponseStatus Request(Uri uri, OnResponseDelegate onResponse, OnErrorDelegate onError)
        {
            return _downloader.Request(uri, onResponse, onError);
        }

        public void Dispose()
        {
            _downloader.Dispose();
        }
    }
}