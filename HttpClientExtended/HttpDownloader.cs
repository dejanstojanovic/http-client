using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExtended
{
    public delegate void HttpDownloadEventDelegate(HttpDownloadEventArgs args);
    public class HttpDownloader
    {
        public event HttpDownloadEventDelegate HttpDownloadEvent;
        public async Task DownloadAsync(Uri url, Stream destinationStream, int bufferSize = 1024)
        {
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    long contentLength = response.Content.Headers.ContentLength.Value;

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        await ReadStream(contentStream);
                        async Task ReadStream(Stream sourceStream)
                        {
                            var buffer = new byte[bufferSize];
                            var read = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                            if (read > 0)
                            {
                                await destinationStream.WriteAsync(buffer, 0, read);
                                if (HttpDownloadEvent != null)
                                {
                                    this.HttpDownloadEvent(new HttpDownloadEventArgs(size: contentLength, downloaded: destinationStream.Length));
                                }
                                await ReadStream(sourceStream);
                            }//if
                            else
                            {
                                await destinationStream.FlushAsync();
                            }//else
                        }//ReadStream
                    }//using
                }//using
            }//using
        }

    }
}
