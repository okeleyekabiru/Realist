using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
 public interface  IVideo
 {
     Task<Guid> Post(Videos video);
     Task<Videos> GetVideoPublicId(string postId, string videid);
     Task Update(Videos videos);
     Task<bool> SaveChanges();
 }
}
