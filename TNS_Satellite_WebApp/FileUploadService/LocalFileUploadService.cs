namespace TNS_WebApp.FileUploadService
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IHostEnvironment _environment;
        public LocalFileUploadService(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string PathLoal)
        {
            DirectoryInfo zDir = new DirectoryInfo(_environment.ContentRootPath + "\\" + PathLoal);
            if (!zDir.Exists)
            {
                zDir.Create();
            }
            var filePath = Path.Combine(_environment.ContentRootPath, PathLoal, file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
