using _0_Framework.Application;

namespace ServiceHost;

public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploader(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Upload(IFormFile file,string path)
    {
        if (file is null) return "";

        var direcoryPath = $"{_webHostEnvironment.WebRootPath}//ProductPictures//{path}";

        if (!Directory.Exists(direcoryPath))
        {
            Directory.CreateDirectory(direcoryPath);
        }

        var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
        var filePath = $"{direcoryPath}//{fileName}";
        using var output = File.Create(filePath);
        file.CopyTo(output);
        return $"{path}/{fileName}";
    }
}