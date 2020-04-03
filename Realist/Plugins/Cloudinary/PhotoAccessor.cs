using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Plugins.Cloudinary;

namespace Plugins
{
public    class PhotoAccessor:IPhotoAccessor
    {
       
   
            private readonly CloudinaryDotNet.Cloudinary _cloudinary;

            public PhotoAccessor(IOptions<CloudinarySettings> config)
            {
                var acc = new Account
                    (config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
                _cloudinary = new CloudinaryDotNet.Cloudinary(acc);
            }
            public PhotoUpLoadResult AddPhoto(IFormFile file)
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParamns = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParamns);
                    }
                }

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }
                return new PhotoUpLoadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUri.AbsoluteUri
                };
            }

            public string DeletePhoto(string publicId)
            {
                var deleteParams = new DeletionParams(publicId);
                var result = _cloudinary.Destroy(deleteParams);
                return result.Result == "ok" ? result.Result : null;
            }
        }
}

