﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace MonoGame.Extended.Testing
{
    static class ResourceDownloader
    {
        public delegate void TextureReadyDelegate(Texture2D texture);

        private static bool _running = true;

        private static object _textureRequestLock;
        private static Queue<TextureRequest> _textureRequests;
        private static Thread[] _textureRequestHandlers;

        static ResourceDownloader()
        {
            _textureRequestLock = new object();
            _textureRequests = new Queue<TextureRequest>();
            _textureRequestHandlers = new Thread[2];
            for (int i = 0; i < _textureRequestHandlers.Length; i++)
            {
                _textureRequestHandlers[i] = new Thread(TextureRequestHandler);
                _textureRequestHandlers[i].Name = "Texture Handler " + (i + 1);
                _textureRequestHandlers[i].Start();
            }
        }

        private static void TextureRequestHandler()
        {
            while (_running)
            {
                while (_textureRequests.Count > 0)
                {
                    TextureRequest textureRequest;
                    lock (_textureRequestLock)
                    {
                        if (_textureRequests.Count > 0)
                            textureRequest = _textureRequests.Dequeue();
                        else
                            continue;
                    }

                    try
                    {
                        ProcessTextureRequest(textureRequest);
                    }
                    catch (TimeoutException timeoutExc)
                    {
                        Console.WriteLine(textureRequest.Url + ": Request timed out");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(textureRequest.Url + ": " + exc);
                    }
                }
                Thread.Sleep(10);
            }
        }

        private static void ProcessTextureRequest(TextureRequest request)
        {
            var headRequest = WebRequest.CreateHttp(request.Url);
            headRequest.Method = "HEAD";
            using (var response = headRequest.GetResponse())
            {
                switch (response.ContentType.ToLower())
                {
                    case "image/jpg":
                    case "image/jpeg":
                    case "image/png":
                    case "image/bmp":
                    case "image/gif":
                        break;

                    default:
                        return;
                }
            }

            var contentRequest = WebRequest.CreateHttp(request.Url);
            using (var response = contentRequest.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                var tex = Texture2D.FromStream(request.GraphicsDevice, stream);
                if (tex == null) // add a better error handler
                    throw new Exception("Could not load image: " + request.Url);

                request.OnTexture.Invoke(tex);

                // hmm, we need a cache for textures
                //var fs = new System.IO.FileStream(System.IO.Path.GetFileName(textureRequest.Url), System.IO.FileMode.Create))
                //    stream.CopyTo(fs);
            }
        }

        public static void RequestTexture(string url, GraphicsDevice device, TextureReadyDelegate onTexture)
        {
            _textureRequests.Enqueue(new TextureRequest(url, device, onTexture));
        }

        public static void Unload()
        {
            _running = false;
            lock (_textureRequests)
                _textureRequests.Clear();
        }

        struct TextureRequest
        {
            public readonly string Url;
            public readonly GraphicsDevice GraphicsDevice;
            public readonly TextureReadyDelegate OnTexture;

            public TextureRequest(string url, GraphicsDevice device, TextureReadyDelegate onTexture)
            {
                Url = url;
                GraphicsDevice = device;
                OnTexture = onTexture;
            }
        }
    }
}