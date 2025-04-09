using Application.Common.Messaging;
using Domain.Birthdays;
using SharedKernel;

namespace Application.Birthdays.Update;

public sealed class UpdateBirthdayCommandHandler : ICommandHandler<UpdateBirthdayCommand>
{
    private readonly IBirthdayRepository _birthdayRepository;

    public UpdateBirthdayCommandHandler(IBirthdayRepository birthdayRepository)
    {
        _birthdayRepository = birthdayRepository;
    }

    public async Task<Result> Handle(UpdateBirthdayCommand request, CancellationToken cancellationToken)
    {
        Birthday? birthday = await _birthdayRepository.GetByIdAsync(request.BirthdayId, cancellationToken);

        if (birthday is null)
        {
            return BirthdayErrors.NotFoundById(request.BirthdayId);
        }

        birthday.FullName = request.FullName;
        birthday.Date = request.Date;
        birthday.Note = request.Note;

        await _birthdayRepository.UpdateAsync(birthday, cancellationToken);

        return Result.Success();
    }
}
