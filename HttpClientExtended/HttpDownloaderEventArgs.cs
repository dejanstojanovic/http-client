namespace HttpClientExtended
{
    public class HttpDownloadEventArgs
    {
        public long Size { get; private set; }
        public long Downloaded { get; private set; }
        public double Progress { get; private set; }

        public HttpDownloadEventArgs(long size, long downloaded)
        {
            this.Size = size;
            this.Downloaded = downloaded;
            this.Progress = ((double)downloaded / size) * 100;
        }

    }
}
