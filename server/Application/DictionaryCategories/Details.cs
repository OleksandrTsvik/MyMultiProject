using Application.Core;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryCategories;

public class Details
{

    public class Query : IRequest<Result<DictionaryCategoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<DictionaryCategoryDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<DictionaryCategoryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            DictionaryCategoryDto item = await _context.DictionaryCategories
                .Where(x => x.Id == request.Id)
                .Select(x => x.ToDictionaryCategoryDto())
                .FirstOrDefaultAsync();

            return Result<DictionaryCategoryDto>.Success(item);
        }
    }
}
