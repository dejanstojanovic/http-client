using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExtended
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int bufferSize = 1024;
            var downloader = new HttpDownloader();

            downloader.HttpDownloadEvent += Downloader_HttpDownloadEvent;

            using (var destination = new FileStream(@"D:\Temp\my-file.mp3", FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true)) {
                await downloader.DownloadAsync(
                    url: new Uri("http://mediafile.somedomain.com/my-file.mp3"),
                    destinationStream: destination,
                    bufferSize: bufferSize
                    );
                }

            Console.ReadLine();
        }

        private static void Downloader_HttpDownloadEvent(HttpDownloadEventArgs args)
        {
            Console.WriteLine($"{Math.Round(args.Progress, 2)}%");
        }

    }
}
