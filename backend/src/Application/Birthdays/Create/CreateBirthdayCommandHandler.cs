using Application.Common.Authentication;
using Application.Common.Messaging;
using Domain.Birthdays;
using SharedKernel;

namespace Application.Birthdays.Create;

public sealed class CreateBirthdayCommandHandler : ICommandHandler<CreateBirthdayCommand, Guid>
{
    private readonly IUserContext _userContext;
    private readonly IBirthdayRepository _birthdayRepository;

    public CreateBirthdayCommandHandler(IUserContext userContext, IBirthdayRepository birthdayRepository)
    {
        _userContext = userContext;
        _birthdayRepository = birthdayRepository;
    }

    public async Task<Result<Guid>> Handle(CreateBirthdayCommand request, CancellationToken cancellationToken)
    {
        var birthday = new Birthday
        {
            UserId = _userContext.UserId,
            FullName = request.FullName,
            Date = request.Date,
            Note = request.Note,
        };

        await _birthdayRepository.InsertAsync(birthday, cancellationToken);

        return birthday.Id;
    }
}
