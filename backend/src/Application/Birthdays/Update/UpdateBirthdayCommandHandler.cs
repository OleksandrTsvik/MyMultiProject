using Application.Common.Authentication;
using Application.Common.Messaging;
using Domain.Birthdays;
using SharedKernel;

namespace Application.Birthdays.Update;

public sealed class UpdateBirthdayCommandHandler : ICommandHandler<UpdateBirthdayCommand>
{
    private readonly IUserContext _userContext;
    private readonly IBirthdayRepository _birthdayRepository;

    public UpdateBirthdayCommandHandler(IUserContext userContext, IBirthdayRepository birthdayRepository)
    {
        _userContext = userContext;
        _birthdayRepository = birthdayRepository;
    }

    public async Task<Result> Handle(UpdateBirthdayCommand request, CancellationToken cancellationToken)
    {
        Birthday? birthday = await _birthdayRepository.GetByIdAndUserIdAsync(
            request.BirthdayId,
            _userContext.UserId,
            cancellationToken);

        if (birthday is null)
        {
            return BirthdayErrors.NotFound();
        }

        birthday.FullName = request.FullName;
        birthday.Date = request.Date;
        birthday.Note = request.Note;

        await _birthdayRepository.UpdateAsync(birthday, cancellationToken);

        return Result.Success();
    }
}
