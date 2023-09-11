using Application.Images.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Images;

public class ImageAccessor : IImageAccessor
{
    private const string ImagesFolder = "images";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFolderAccessor _folderAccessor;

    public ImageAccessor(IHttpContextAccessor httpContextAccessor, IFolderAccessor folderAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _folderAccessor = folderAccessor;
    }

    public string GetImagePath(string name)
    {
        return Path.Combine(_folderAccessor.GetFolderPath(ImagesFolder), name);
    }

    public async Task<ImageUploadResult> AddImage(IFormFile image)
    {
        if (image == null || image.Length <= 0)
        {
            return null;
        }

        string imageExtension = Path.GetExtension(image.FileName);
        string imageName = Guid.NewGuid().ToString() + imageExtension;
        string imagePath = Path.Combine(_folderAccessor.GetFolderPath(ImagesFolder), imageName);

        using (FileStream stream = File.Create(imagePath))
        {
            await image.CopyToAsync(stream);
        }

        return new ImageUploadResult
        {
            Name = imageName,
            Url = GetImageUrl(imageName)
        };
    }

    public string DeleteImage(string name)
    {
        string imagePath = Path.Combine(_folderAccessor.GetFolderPath(ImagesFolder), name);

        // if (!File.Exists(imagePath))
        // {
        //     return "Image not found";
        // }

        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }

        return null;
    }

    private string GetImageUrl(string name)
    {
        HttpRequest request = _httpContextAccessor.HttpContext.Request;

        return $"{request.Scheme}://{request.Host}{request.PathBase}/api/images/{name}";
    }
}
