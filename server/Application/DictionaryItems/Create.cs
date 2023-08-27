using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class Create
{
    public class Command : IRequest<Result<DictionaryItemDto>>
    {
        public CreateDto CreateDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CreateDto).SetValidator(new CreateDtoValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<DictionaryItemDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<DictionaryItemDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            CreateDto addedItem = request.CreateDto;

            if (!await _context.DictionaryCategories.AnyAsync(x => x.Id == addedItem.CategoryId))
            {
                return Result<DictionaryItemDto>.Failure("Failed to create dictionary item, not found dictionary category");
            }

            DictionaryItem dictionaryItem = new DictionaryItem
            {
                Text = addedItem.Text,
                Translation = addedItem.Translation,
                Status = addedItem.Status,
                DateCreation = DateTime.UtcNow,
                AppUser = user
            };

            _context.DictionaryItems.Add(dictionaryItem);

            DictionaryCategoryItem dictionaryCategoryItem = new DictionaryCategoryItem
            {
                DictionaryCategoryId = addedItem.CategoryId,
                DictionaryItemId = dictionaryItem.Id,
                Position = -1
            };

            _context.DictionaryCategoryItems.Add(dictionaryCategoryItem);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<DictionaryItemDto>.Failure("Failed to create dictionary item");
            }

            return Result<DictionaryItemDto>.Success(dictionaryItem.ToDictionaryItemDto(dictionaryCategoryItem.Position));
        }
    }
}
