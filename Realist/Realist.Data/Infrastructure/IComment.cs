using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
   public interface IComment
   {
       Task Add(Comment comment);
       Task<bool> SaveChanges();
       Task<IEnumerable<Comment>> GetAllPostComment(string postId);
   }
}
