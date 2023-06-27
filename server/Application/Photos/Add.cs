using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Add
{
    public class Command : IRequest<Result<Photo>>
    {
        public IFormFile File { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Photo>>
    {
        private readonly DataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            PhotoUploadResult photoUploadResult = await _photoAccessor.AddPhoto(request.File);

            if (photoUploadResult == null)
            {
                return Result<Photo>.Failure("Failed to upload photo");
            }

            Photo photo = new Photo
            {
                Id = photoUploadResult.PublicId,
                Url = photoUploadResult.Url
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Photo>.Failure("Failed to add photo");
            }

            return Result<Photo>.Success(photo);
        }
    }
}