using Application.Interfaces;
using Application.Photos.DTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;

    public PhotoAccessor(IOptions<CloudinarySettings> config)
    {
        Account account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
    {
        if (file.Length <= 0)
        {
            return null;
        }

        await using Stream stream = file.OpenReadStream();

        ImageUploadParams uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill")
        };

        ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new PhotoUploadResult
        {
            PublicId = uploadResult.PublicId,
            Url = uploadResult.SecureUrl.ToString()
        };
    }

    public async Task<string> DeletePhoto(string publicId)
    {
        DeletionParams deletionParams = new DeletionParams(publicId);
        DeletionResult result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result == "ok" ? result.Result : null;
    }
}
