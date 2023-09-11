using Application.Images.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IImageAccessor
{
    string GetImagePath(string name);
    Task<ImageUploadResult> AddImage(IFormFile image);
    string DeleteImage(string name);
}
