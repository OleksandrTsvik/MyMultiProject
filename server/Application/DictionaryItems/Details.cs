using Application.Core;
using Application.DictionaryItems.DTOs;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DictionaryItems;

public class Details
{

    public class Query : IRequest<Result<DictionaryItemDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<DictionaryItemDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<DictionaryItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            DictionaryItemDto item = await _context.DictionaryItems
                .Where(x => x.Id == request.Id)
                .Select(x => x.ToDictionaryItemDto())
                .FirstOrDefaultAsync();

            return Result<DictionaryItemDto>.Success(item);
        }
    }
}
