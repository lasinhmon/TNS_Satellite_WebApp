namespace TNS_WebApp.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string PathLocal);
    }
}
