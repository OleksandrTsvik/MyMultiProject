namespace Application.Common.Interfaces;

public interface IFile
{
    string ContentType { get; }

    long Length { get; }

    string FileName { get; }

    void CopyTo(Stream target);

    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);

    Stream OpenReadStream();
}
