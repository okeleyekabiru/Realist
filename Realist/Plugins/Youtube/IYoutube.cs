using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Plugins.Youtube
{
   public interface IYoutube
   {
       Task<UploadVideoResult> UploadVideo(UploadViewModel uploadViewModel,IFormFile video);
       Task<List<UploadVideoResult>> RetreiveVideo(string videoId);
       Task<UploadVideoResult> UpdateVideo(string videoId, IFormFile video, UploadViewModel uploadViewModel);
       Task<bool> DeleteVideo(string id);
      string CopyToFolder(IFormFile video);
      void DeleteFromFolder(string path);
      Task CreatePlayList();
   }
}
