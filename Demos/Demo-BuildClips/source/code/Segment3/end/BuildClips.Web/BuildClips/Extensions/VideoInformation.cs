namespace BuildClips.Extensions
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class VideoInformation
    {
        private const string VideoTitleFormField = "title";
        private const string VideoDescriptionFormField = "description";
        private const string VideoFileUploadFormField = "videoFile";

        private const string ErrorMessageTempalte = "The field '{0}' is required.";
        private const string NoUploadedFileError = "A video file is required.";

        private VideoInformation()
        {
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public FileInformation FileInformation { get; set; }

        public static VideoInformation Parse(IEnumerable<HttpContent> contents)
        {
            var videoInforamtion = new VideoInformation();
            string title;
            string description;

            contents.TryGetFormFieldStringValue(VideoTitleFormField, out title);
            contents.TryGetFormFieldStringValue(VideoDescriptionFormField, out description);

            videoInforamtion.FileInformation = FileInformation.Parse(contents, VideoFileUploadFormField);
            videoInforamtion.Title = title;
            videoInforamtion.Description = description;

            return videoInforamtion;
        }

        public IEnumerable<string> GetErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(this.Title))
            {
                errors.Add(string.Format(ErrorMessageTempalte, "Title"));
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                errors.Add(string.Format(ErrorMessageTempalte, "Description"));
            }

            if (this.FileInformation == null)
            {
                errors.Add(NoUploadedFileError);
            }

            return errors;
        }
    }
}