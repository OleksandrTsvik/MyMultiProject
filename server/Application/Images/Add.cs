using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Images;

public class Add
{
    public class Command : IRequest<Result<ImageDto>>
    {
        public IFormFile Image { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<ImageDto>>
    {
        private readonly DataContext _context;
        private readonly IImageAccessor _imageAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IImageAccessor imageAccessor,
            IUserAccessor userAccessor)
        {
            _context = context;
            _imageAccessor = imageAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Result<ImageDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            ImageUploadResult imageUploadResult = await _imageAccessor.AddImage(request.Image);

            if (imageUploadResult == null)
            {
                return Result<ImageDto>.Failure("Failed to upload image");
            }

            Image image = new Image
            {
                Name = imageUploadResult.Name,
                DateCreation = DateTime.UtcNow,
                AppUser = user
            };

            _context.Images.Add(image);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<ImageDto>.Failure("Failed to add image");
            }
            
            return Result<ImageDto>.Success(image.ToImageDto(imageUploadResult));
        }
    }
}
