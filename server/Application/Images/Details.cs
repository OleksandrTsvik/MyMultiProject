using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Images;

public class Details
{
    public class Query : IRequest<Result<byte[]>>
    {
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<byte[]>>
    {
        private readonly DataContext _context;
        private readonly IImageAccessor _imageAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IImageAccessor imageAccessor, IUserAccessor userAccessor)
        {
            _context = context;
            _imageAccessor = imageAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Result<byte[]>> Handle(Query request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            Image image = user.Images.FirstOrDefault(x => x.Name == request.Name);

            if (image == null)
            {
                return null;
            }

            byte[] imageBytes = await _imageAccessor.GetImage(image.Name);

            return Result<byte[]>.Success(imageBytes);
        }
    }
}
