namespace WebApp;

public class Upload
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Upload(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Uploadfile(IFormFile? file, string directory)
    {
        if (file == null) return "";
        if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Images\\" + directory))
        {
            Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Images\\" + directory);
        }

        var path = _webHostEnvironment.WebRootPath + "\\Images\\" + directory + "\\" + file.FileName;
        using var f = System.IO.File.Create(path);
        file.CopyTo(f);
        return file.FileName;
    }
}