using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace Realist.Data.Infrastructure
{
 public interface  IVideo
 {
     Task<bool> Post(Video video);
 }
}
