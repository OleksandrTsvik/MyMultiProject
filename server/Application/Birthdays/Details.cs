using Application.Birthdays.DTOs;
using Application.Core;
using Application.Mappers;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Birthdays;

public class Details
{
    public class Query : IRequest<Result<BirthdayDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<BirthdayDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<BirthdayDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            Birthday birthday = await _context.Birthdays
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync();

            return Result<BirthdayDto>.Success(birthday.ToBirthdayDto());
        }
    }
}
