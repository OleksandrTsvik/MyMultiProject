using Application.Birthdays;
using Application.Birthdays.DTOs;
using Domain;

namespace Application.Mappers;

public static class BirthdayMappers
{
    public static BirthdayDto ToBirthdayDto(this Birthday birthday)
    {
        return new BirthdayDto
        {
            Id = birthday.Id,
            FullName = birthday.FullName,
            Date = birthday.Date,
            Note = birthday.Note,
            Age = BirthdayUtil.GetAgeFromBirthday(birthday.Date),
            DaysUntilBirthday = BirthdayUtil.GetDaysUntilBirthday(birthday.Date)
        };
    }
}
