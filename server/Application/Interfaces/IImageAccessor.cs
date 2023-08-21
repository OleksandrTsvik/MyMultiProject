using Application.Images;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IImageAccessor
{
    Task<byte[]> GetImage(string name);
    Task<ImageUploadResult> AddImage(IFormFile image);
    string DeleteImage(string name);
}
