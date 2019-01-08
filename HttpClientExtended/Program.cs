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
                    url: new Uri("https://media-files.talkdeskapp.com/calls/CA5f09fbfaad001a4cad22f178dfbdd4dd/recordings/0.mp3"),
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
