using Application.Interfaces;

namespace Infrastructure.Repositories;

public class FileService:IFileService
{
    public string AddImage(string imageFile, string path, string imageName)
    {
        if (string.IsNullOrEmpty(imageFile))
            return "";

        var base64EncodedBytes = Convert.FromBase64String(imageFile);

        string filePath = Path.Combine("wwwroot/image", path);

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