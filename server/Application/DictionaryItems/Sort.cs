using Application.Core;
using Application.DictionaryItems.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class Sort
{
    public class Command : IRequest<Result<Unit>>
    {
        public List<SortDto> SortDtos { get; set; }
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
            List<Guid> itemIds = request.SortDtos.Select(s => s.ItemId).ToList();
            List<Guid> categoryIds = request.SortDtos.Select(s => s.CategoryId).ToList();

            List<DictionaryCategoryItem> dictionaryCategoryItems = await _context.DictionaryCategoryItems
                .Where(x => x.DictionaryItem.AppUser.UserName == _userAccessor.GetUserName() &&
                    itemIds.Contains(x.DictionaryItemId) &&
                    categoryIds.Contains(x.DictionaryCategoryId))
                .ToListAsync();

            foreach (DictionaryCategoryItem dictionaryCategoryItem in dictionaryCategoryItems)
            {
                SortDto sortedItem = request.SortDtos.FirstOrDefault(x =>
                    x.ItemId == dictionaryCategoryItem.DictionaryItemId &&
                    x.CategoryId == dictionaryCategoryItem.DictionaryCategoryId);

                if (sortedItem != null)
                {
                    dictionaryCategoryItem.Position = sortedItem.Position;
                }
            }

            bool result = await _context.SaveChangesAsync() >= 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to sort dictionary items");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
