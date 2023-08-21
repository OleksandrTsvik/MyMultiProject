using Application.Images;
using Domain;

namespace Application.Mappers;

public static class ImageMappers
{
    public static ImageDto ToImageDto(this Image image, ImageUploadResult imageUploadResult)
    {
        return new ImageDto
        {
            Id = image.Id,
            Name = image.Name,
            Url = imageUploadResult.Url
        };
    }
}
