using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryCategories;

public class List
{
    public class Query : IRequest<Result<List<DictionaryCategoryDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<DictionaryCategoryDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<DictionaryCategoryDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<DictionaryCategoryDto> categories = await _context.DictionaryCategories
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName())
                .OrderBy(x => x.Position)
                .Select(x => new DictionaryCategoryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language,
                    Position = x.Position,
                    DateCreation = x.DateCreation,
                    CountItems = x.Items.Count
                })
                .ToListAsync();

            return Result<List<DictionaryCategoryDto>>.Success(categories);
        }
    }
}
