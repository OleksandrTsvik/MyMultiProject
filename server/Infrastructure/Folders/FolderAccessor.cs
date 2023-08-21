using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Folders;

public class FolderAccessor : IFolderAccessor
{
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly IConfiguration _config;

    public FolderAccessor(IWebHostEnvironment appEnvironment, IConfiguration config)
    {
        _appEnvironment = appEnvironment;
        _config = config;
    }

    public string GetFolderPath(string folder)
    {
        string folderPath = Path.Combine(_appEnvironment.ContentRootPath, _config["UploadedDataFolder"], folder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return folderPath;
    }
}
