using Application.Core;
using Application.DictionaryItems.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class List
{

    public class Query : IRequest<Result<List<DictionaryItemDto>>>
    {
        public DictionaryItemParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<DictionaryItemDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<DictionaryItemDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<DictionaryItemDto> items = await _context.DictionaryCategoryItems
                .Where(x => x.DictionaryCategoryId == request.Params.CategoryId &&
                    x.DictionaryItem.AppUser.UserName == _userAccessor.GetUserName())
                .OrderBy(x => x.Position)
                .Select(x => new DictionaryItemDto
                {
                    Id = x.DictionaryItem.Id,
                    Text = x.DictionaryItem.Text,
                    Translation = x.DictionaryItem.Translation,
                    Status = x.DictionaryItem.Status,
                    DateCreation = x.DictionaryItem.DateCreation,
                    Position = x.Position
                })
                .ToListAsync();

            return Result<List<DictionaryItemDto>>.Success(items);
        }
    }
}
