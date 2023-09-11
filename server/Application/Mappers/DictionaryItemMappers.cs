using Application.DictionaryItems.DTOs;
using Domain;

namespace Application.Mappers;

public static class DictionaryItemMappers
{
    public static DictionaryItemDto ToDictionaryItemDto(this DictionaryItem dictionaryItem)
    {
        return dictionaryItem.ToDictionaryItemDto(-1);
    }

    public static DictionaryItemDto ToDictionaryItemDto(this DictionaryItem dictionaryItem, int position)
    {
        return new DictionaryItemDto
        {
            Id = dictionaryItem.Id,
            Text = dictionaryItem.Text,
            Translation = dictionaryItem.Translation,
            Status = dictionaryItem.Status,
            DateCreation = dictionaryItem.DateCreation,
            Position = position
        };
    }
}
