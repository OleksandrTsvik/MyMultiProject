using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Images;

public class Details
{
    public class Query : IRequest<Result<string>>
    {
        public string Name { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<string>>
    {
        private readonly DataContext _context;
        private readonly IImageAccessor _imageAccessor;

        public Handler(DataContext context, IImageAccessor imageAccessor)
        {
            _context = context;
            _imageAccessor = imageAccessor;
        }

        public async Task<Result<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            Image image = await _context.Images
                .FirstOrDefaultAsync(x => x.Name == request.Name);

            if (image == null)
            {
                return null;
            }

            string imagePath = _imageAccessor.GetImagePath(image.Name);

            return Result<string>.Success(imagePath);
        }
    }
}
