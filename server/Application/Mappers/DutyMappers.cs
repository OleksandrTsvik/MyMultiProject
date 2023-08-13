using Application.Duties;
using Domain;

namespace Application.Mappers;

public static class DutyMappers
{
    public static DutyDto ToDutyDto(this Duty duty)
    {
        return new DutyDto
        {
            Id = duty.Id,
            Position = duty.Position,
            Title = duty.Title,
            Description = duty.Description,
            IsCompleted = duty.IsCompleted,
            DateCreation = duty.DateCreation,
            DateCompletion = duty.DateCompletion,
            BackgroundColor = duty.BackgroundColor,
            FontColor = duty.FontColor
        };
    }

    public static void Update(this Duty duty, Duty updatedDuty)
    {
        duty.Position = updatedDuty.Position;
        duty.Title = updatedDuty.Title;
        duty.Description = updatedDuty.Description;
        duty.IsCompleted = updatedDuty.IsCompleted;
        duty.DateCreation = updatedDuty.DateCreation;
        duty.DateCompletion = updatedDuty.DateCompletion;
        duty.BackgroundColor = updatedDuty.BackgroundColor;
        duty.FontColor = updatedDuty.FontColor;
    }
}
