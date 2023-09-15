using Application.Core;
using Application.GrammarRules.DTOs;
using Application.GrammarRules.Validators;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class Create
{
    public class Command : IRequest<Result<GrammarRuleDto>>
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

    public class Handler : IRequestHandler<Command, Result<GrammarRuleDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<GrammarRuleDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            CreateDto addedRule = request.CreateDto;

            GrammarRule rule = new GrammarRule
            {
                Title = addedRule.Title,
                Description = addedRule.Description,
                Language = addedRule.Language,
                Status = addedRule.Status,
                Position = -1,
                DateCreation = DateTime.UtcNow,
                AppUser = user
            };

            _context.GrammarRules.Add(rule);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<GrammarRuleDto>.Failure("Failed to create grammar rule");
            }

            return Result<GrammarRuleDto>.Success(rule.ToGrammarRuleDto());
        }
    }
}
