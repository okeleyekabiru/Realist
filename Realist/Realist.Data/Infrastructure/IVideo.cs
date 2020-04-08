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
     Task Post(Videos video);
     Task<bool> SaveChanges();
 }
}
