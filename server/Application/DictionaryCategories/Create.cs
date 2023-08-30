using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryCategories;

public class Create
{
    public class Command : IRequest<Result<DictionaryCategoryDto>>
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

    public class Handler : IRequestHandler<Command, Result<DictionaryCategoryDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<DictionaryCategoryDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            CreateDto addedCategory = request.CreateDto;

            DictionaryCategory category = new DictionaryCategory
            {
                Title = addedCategory.Title,
                Language = addedCategory.Language,
                Position = -1,
                DateCreation = DateTime.UtcNow,
                AppUser = user
            };

            _context.DictionaryCategories.Add(category);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<DictionaryCategoryDto>.Failure("Failed to create dictionary category");
            }

            return Result<DictionaryCategoryDto>.Success(category.ToDictionaryCategoryDto());
        }
    }
}
