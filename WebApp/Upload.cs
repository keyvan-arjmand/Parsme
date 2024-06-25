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
    public string AddImage(string imageFile, string path, string imageName)
    {
        if (string.IsNullOrEmpty(imageFile))
            return "";

        var base64EncodedBytes = Convert.FromBase64String(imageFile);

        string filePath = Path.Combine("wwwroot", path);

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string fileName = imageName + ".jpg";

        var address = Path.Combine(filePath, fileName);

        File.WriteAllBytes(address, base64EncodedBytes);

        return fileName;

    }

    public void RemoveImage(string imageName, string path)
    {
        var pathCombine = Path.Combine("wwwroot", path, imageName);

        if (imageName != "noimage.jpg" && imageName != "DefaultIngPic.png" && imageName != "o2fit image.jpg")
        {
            if (File.Exists(pathCombine))
            {
                File.Delete(pathCombine);
            }
        }

    }

    public async Task<string> GetImageAsBase64Async(string path)
    {
        var imageBytes = await File.ReadAllBytesAsync($"wwwroot/{path}");
        return Convert.ToBase64String(imageBytes);
    }
}