namespace PhotoAlbum
{
    using System.Collections.Generic;
    using System.IO;

    public class PhotoAlbumDataSource
    {
        private DirectoryInfo directoryInfo;

        public PhotoAlbumDataSource(string path)
        {
            this.directoryInfo = new DirectoryInfo(path);
        }

        public IEnumerable<FileInfo> Files
        {
            get
            {
                foreach (var fileInfo in this.directoryInfo.GetFiles("*.png"))
                {
                    yield return fileInfo;
                }

                foreach (var fileInfo in this.directoryInfo.GetFiles("*.jpg"))
                {
                    yield return fileInfo;
                }
            }
        }
    }
}