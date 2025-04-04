using MediatR;
using SharedKernel;

namespace Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
