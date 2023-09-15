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

public class Edit
{
    public class Command : IRequest<Result<GrammarRuleDto>>
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
            EditDto editedRule = request.EditDto;

            GrammarRule rule = await _context.GrammarRules
                .FirstOrDefaultAsync(x => x.Id == editedRule.Id &&
                    x.AppUser.UserName == _userAccessor.GetUserName());

            if (rule == null)
            {
                return null;
            }

            rule.Title = editedRule.Title;
            rule.Description = editedRule.Description;
            rule.Language = editedRule.Language;
            rule.Status = editedRule.Status;

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<GrammarRuleDto>.Failure("Failed to update grammar rule");
            }

            return Result<GrammarRuleDto>.Success(rule.ToGrammarRuleDto());
        }
    }
}
