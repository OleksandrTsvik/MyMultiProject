using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Birthdays;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            Birthday birthday = await _context.Birthdays.FindAsync(request.Id);

            if (birthday == null)
            {
                return null;
            }

            _context.Birthdays.Remove(birthday);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete the birthday");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
