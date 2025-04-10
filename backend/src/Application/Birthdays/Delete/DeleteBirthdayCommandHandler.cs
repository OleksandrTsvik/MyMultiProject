using Application.Common.Authentication;
using Application.Common.Messaging;
using Domain.Birthdays;
using SharedKernel;

namespace Application.Birthdays.Delete;

public sealed class DeleteBirthdayCommandHandler : ICommandHandler<DeleteBirthdayCommand>
{
    private readonly IUserContext _userContext;
    private readonly IBirthdayRepository _birthdayRepository;

    public DeleteBirthdayCommandHandler(IUserContext userContext, IBirthdayRepository birthdayRepository)
    {
        _userContext = userContext;
        _birthdayRepository = birthdayRepository;
    }

    public async Task<Result> Handle(DeleteBirthdayCommand request, CancellationToken cancellationToken)
    {
        await _birthdayRepository.DeleteAsync(request.BirthdayId, _userContext.UserId, cancellationToken);

        return Result.Success();
    }
}
