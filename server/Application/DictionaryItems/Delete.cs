using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            DictionaryItem dictionaryItem = await _context.DictionaryItems
                .FirstOrDefaultAsync(x => x.Id == request.Id &&
                    x.AppUser.UserName == _userAccessor.GetUserName());

            if (dictionaryItem == null)
            {
                return null;
            }

            _context.DictionaryItems.Remove(dictionaryItem);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete the dictionary item");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
