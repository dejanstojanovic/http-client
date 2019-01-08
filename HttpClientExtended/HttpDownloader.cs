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
                        var read = 1;
                        while (read > 0)
                        {
                            var buffer = new byte[bufferSize];
                            read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                            await destinationStream.WriteAsync(buffer, 0, read);
                            if (HttpDownloadEvent != null)
                            {
                                this.HttpDownloadEvent(new HttpDownloadEventArgs(size: contentLength, downloaded: destinationStream.Length));
                            }//if
                        }//while
                        await destinationStream.FlushAsync();
                    }//using
                }//using
            }//using
        }

    }
}
