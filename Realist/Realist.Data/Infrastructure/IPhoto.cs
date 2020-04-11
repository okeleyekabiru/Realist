using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
  public  interface  IPhoto
  {
      Task Delete(Photo photo);
      Task<Guid> UploadImageDb(Photo photo);
      Task<Photo> FindPhotoId(string postId , string photoId);
      void DeleteImageFromDb(Photo photo);
      Task Update(Photo photo);
      Task<bool> SaveChanges();
      Task<bool> FindUserImage(string userId);
  }
}
