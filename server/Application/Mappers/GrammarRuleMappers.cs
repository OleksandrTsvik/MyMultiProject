using Application.GrammarRules.DTOs;
using Domain;

namespace Application.Mappers;

public static class GrammarRuleMappers
{
    public static GrammarRuleDto ToGrammarRuleDto(this GrammarRule rule)
    {
        return new GrammarRuleDto
        {
            Id = rule.Id,
            Title = rule.Title,
            Description = rule.Description,
            Language = rule.Language,
            Status = rule.Status,
            Position = rule.Position,
            DateCreation = rule.DateCreation
        };
    }
}
