using Application.Photos;
using Domain;

namespace Application.Mappers;

public static class PhotoMappers
{
    public static PhotoDto ToPhotoDto(this Photo photo)
    {
        return new PhotoDto
        {
            Id = photo.Id,
            Url = photo.Url,
            IsMain = photo.IsMain
        };
    }

    public static List<PhotoDto> ToListPhotoDto(this ICollection<Photo> photos)
    {
        return photos.Select(x => x.ToPhotoDto()).ToList();
    }
}
