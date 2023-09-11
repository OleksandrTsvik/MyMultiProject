using Application.DictionaryCategories.DTOs;
using Domain;

namespace Application.Mappers;

public static class DictionaryCategoryMappers
{
    public static DictionaryCategoryDto ToDictionaryCategoryDto(this DictionaryCategory category)
    {
        return new DictionaryCategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Language = category.Language,
            Position = category.Position,
            DateCreation = category.DateCreation,
            CountItems = category.Items.Count
        };
    }
}
