using Microsoft.AspNetCore.Components.Forms;

namespace Admin.Ui;

public class Uploader(IWebHostEnvironment webHostEnvironment)
{
    public async Task<string> UploadBrowserFile(IBrowserFile? file, string directory)
    {
        if (file == null)
        {
            return string.Empty;
        }

        try
        {
            var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Images", directory);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.Name);

            var maxAllowedSize = 10 * 1024 * 1024;
            if (file.Size > maxAllowedSize)
            {
                throw new InvalidOperationException("حجم فایل بیشتر از حد مجاز است.");
            }

            using var stream = file.OpenReadStream();
            using var fileStream = new FileStream(filePath,FileMode.Open);
            await stream.CopyToAsync(fileStream);

            return file.Name;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"خطا هنگام آپلود فایل: {ex.Message}");
            return string.Empty;
        }
    }
    
}