using Application.Core;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.DictionaryCategories;

public class Edit
{

    public class Command : IRequest<Result<DictionaryCategoryDto>>
    {
        public EditDto EditDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.EditDto).SetValidator(new CreateDtoValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<DictionaryCategoryDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<DictionaryCategoryDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            EditDto editedCategory = request.EditDto;

            DictionaryCategory category = await _context.DictionaryCategories.FindAsync(editedCategory.Id);

            if (category == null)
            {
                return null;
            }

            category.Title = editedCategory.Title;
            category.Language = editedCategory.Language;
            category.Position = editedCategory.Position;

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<DictionaryCategoryDto>.Failure("Failed to update dictionary category");
            }

            return Result<DictionaryCategoryDto>.Success(category.ToDictionaryCategoryDto());
        }
    }
}
