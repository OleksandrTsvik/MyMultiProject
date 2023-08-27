using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.DictionaryCategories;

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
            DictionaryCategory category = await _context.DictionaryCategories.FindAsync(request.Id);

            if (category == null)
            {
                return null;
            }

            _context.DictionaryCategories.Remove(category);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete the dictionary category");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
