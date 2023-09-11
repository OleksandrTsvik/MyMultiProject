using Application.Core;
using Application.DictionaryItems.DTOs;
using Application.DictionaryItems.Validators;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class Edit
{

    public class Command : IRequest<Result<DictionaryItemDto>>
    {
        public EditDto EditDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.EditDto).SetValidator(new EditDtoValidator());
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
            EditDto editedDictionaryItem = request.EditDto;

            DictionaryItem dictionaryItem = await _context.DictionaryItems
                .FirstOrDefaultAsync(x => x.Id == editedDictionaryItem.Id &&
                    x.AppUser.UserName == _userAccessor.GetUserName());

            if (dictionaryItem == null)
            {
                return null;
            }

            dictionaryItem.Text = editedDictionaryItem.Text;
            dictionaryItem.Translation = editedDictionaryItem.Translation;
            dictionaryItem.Status = editedDictionaryItem.Status;

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<DictionaryItemDto>.Failure("Failed to update dictionary item");
            }

            return Result<DictionaryItemDto>.Success(dictionaryItem.ToDictionaryItemDto());
        }
    }
}
