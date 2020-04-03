using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
  public  interface  IPhoto
  {
      Task UploadImageDb(Photo photo);
      void DeleteImageFromDb(Photo photo);
      Task<bool> SaveChanges();
      Task<bool> FindUserImage(string userId);
  }
}
