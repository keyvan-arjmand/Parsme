namespace Panel;

public class Upload
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Upload(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Uploadfile(IFormFile? file, string directory)
    {
        if (file == null) return string.Empty;
    
        var fullDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", directory);
    
        if (!Directory.Exists(fullDirectoryPath))
        {
            Directory.CreateDirectory(fullDirectoryPath);
        }

        var filePath = Path.Combine(fullDirectoryPath, file.FileName);
    
        // بررسی اینکه آیا فایلی با نام مشابه وجود دارد
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath); // حذف فایل در صورت وجود
        }

        using var f = System.IO.File.Create(filePath);
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