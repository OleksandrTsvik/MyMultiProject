using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Dictionary;

public class Quantity
{
    public class Query : IRequest<Result<QuantityDto>> { }

    public class Handler : IRequestHandler<Query, Result<QuantityDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<QuantityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            string userName = _userAccessor.GetUserName();

            QuantityDto quantity = new QuantityDto
            {
                CountCategories = await _context.DictionaryCategories.CountAsync(x => x.AppUser.UserName == userName),
                CountRules = await _context.GrammarRules.CountAsync(x => x.AppUser.UserName == userName)
            };

            return Result<QuantityDto>.Success(quantity);
        }
    }
}
