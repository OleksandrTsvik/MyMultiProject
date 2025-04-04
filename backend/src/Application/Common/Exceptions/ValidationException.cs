using Application.Common.Models;

namespace Application.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyCollection<ValidationError> Errors { get; }

    public ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation Error.")
    {
        Errors = errors;
    }
}
