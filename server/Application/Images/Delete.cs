using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Images;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
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

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            Image image = user.Images.FirstOrDefault(x => x.Id == request.Id);

            if (image == null)
            {
                return null;
            }

            string deleteMessageError = _imageAccessor.DeleteImage(image.Name);

            if (deleteMessageError != null)
            {
                return Result<Unit>.Failure(deleteMessageError);
            }

            user.Images.Remove(image);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Problem deleting image from API");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
