// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Bili.ViewModels.Uwp
{
    internal class HttpRandomAccessStream : IRandomAccessStreamWithContentType
    {
        private readonly HttpClient _client;
        private readonly Uri _requestedUri;
        private IInputStream _inputStream;
        private ulong _size;
        private string _etagHeader;
        private string _lastModifiedHeader;

        // No public constructor, factory methods instead to handle async tasks.
        private HttpRandomAccessStream(HttpClient client, Uri uri)
        {
            _client = client;
            _requestedUri = uri;
            Position = 0;
        }

        public ulong Position { get; private set; }

        public string ContentType { get; private set; } = string.Empty;

        public bool CanRead => true;

        public bool CanWrite => false;

        public ulong Size
        {
            get => _size;
            set => throw new NotImplementedException();
        }

        public static IAsyncOperation<HttpRandomAccessStream> CreateAsync(HttpClient client, Uri uri)
        {
            var randomStream = new HttpRandomAccessStream(client, uri);

            return AsyncInfo.Run(async (cancellationToken) =>
            {
                await randomStream.SendRequesAsync().ConfigureAwait(false);
                return randomStream;
            });
        }

        public IRandomAccessStream CloneStream() => this;

        public IInputStream GetInputStreamAt(ulong position)
            => _inputStream;

        public IOutputStream GetOutputStreamAt(ulong position)
            => throw new NotImplementedException();

        public void Seek(ulong position)
        {
            if (Position != position)
            {
                if (_inputStream != null)
                {
                    _inputStream.Dispose();
                    _inputStream = null;
                }

                Debug.WriteLine("Seek: {0:N0} -> {1:N0}", Position, position);
                Position = position;
            }
        }

        public void Dispose()
        {
            if (_inputStream != null)
            {
                _inputStream.Dispose();
                _inputStream = null;
            }

            if (_client != null)
            {
                _client?.Dispose();
            }
        }

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            return AsyncInfo.Run<IBuffer, uint>(async (cancellationToken, progress) =>
            {
                progress.Report(0);

                try
                {
                    if (_inputStream == null)
                    {
                        await SendRequesAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }

                var result = await _inputStream.ReadAsync(buffer, count, options).AsTask(cancellationToken, progress).ConfigureAwait(false);

                // Move position forward.
                Position += result.Length;
                Debug.WriteLine("requestedPosition = {0:N0}", Position);

                return result;
            });
        }

        public IAsyncOperation<bool> FlushAsync()
            => throw new NotImplementedException();

        public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
            => throw new NotImplementedException();

        private async Task SendRequesAsync()
        {
            HttpRequestMessage request = null;
            request = new HttpRequestMessage(HttpMethod.Get, _requestedUri);

            request.Headers.Add("Range", string.Format("bytes={0}-", Position));

            if (!string.IsNullOrEmpty(_etagHeader))
            {
                request.Headers.Add("If-Match", _etagHeader);
            }

            if (!string.IsNullOrEmpty(_lastModifiedHeader))
            {
                request.Headers.Add("If-Unmodified-Since", _lastModifiedHeader);
            }

            var response = await _client.SendRequestAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead).AsTask().ConfigureAwait(false);

            if (response.Content.Headers.ContentType != null)
            {
                this.ContentType = response.Content.Headers.ContentType.MediaType;
            }

            _size = response.Content?.Headers?.ContentLength ?? 0;

            if (response.StatusCode != HttpStatusCode.PartialContent && Position != 0)
            {
                throw new Exception("HTTP server did not reply with a '206 Partial Content' status.");
            }

            if (string.IsNullOrEmpty(_etagHeader) && response.Headers.ContainsKey("ETag"))
            {
                _etagHeader = response.Headers["ETag"];
            }

            if (string.IsNullOrEmpty(_lastModifiedHeader) && response.Content.Headers.ContainsKey("Last-Modified"))
            {
                _lastModifiedHeader = response.Content.Headers["Last-Modified"];
            }

            if (response.Content.Headers.ContainsKey("Content-Type"))
            {
                ContentType = response.Content.Headers["Content-Type"];
            }

            _inputStream = await response.Content.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false);
        }
    }
}
