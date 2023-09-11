using Application.Core;
using Application.DictionaryCategories.DTOs;
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
            DictionaryCategoryDto category = await _context.DictionaryCategories
                .Where(x => x.Id == request.Id)
                .Select(x => new DictionaryCategoryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language,
                    Position = x.Position,
                    DateCreation = x.DateCreation,
                    CountItems = x.Items.Count
                })
                .FirstOrDefaultAsync();

            return Result<DictionaryCategoryDto>.Success(category);
        }
    }
}
