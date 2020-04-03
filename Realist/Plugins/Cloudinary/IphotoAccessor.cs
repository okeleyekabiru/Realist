using Microsoft.AspNetCore.Http;

namespace Plugins
{
  public  interface IPhotoAccessor
    {
        PhotoUpLoadResult AddPhoto(IFormFile file);
        string DeletePhoto(string publicId);
    }
}

