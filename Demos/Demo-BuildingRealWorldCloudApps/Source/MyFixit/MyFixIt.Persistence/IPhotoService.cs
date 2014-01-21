namespace MyFixIt.Persistence
{
    using System.Web;

    public interface IPhotoService
    {
        void CreateAndConfigure();

        string UploadPhoto(HttpPostedFileBase photoToUpload);
    }
}