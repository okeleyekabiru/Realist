using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Infrastructure
{
    public interface IPost
    {
       Task<long> GetCommentCount(string postId);

       Task Delete(Post post);
        Task Post(Post post);
        PagedList<Post> GetAll(PaginationModel page);
        Task<Post> Get(string postId);
        Task<Post> GetPost(string postId);
         Guid Update(Post post);
         Task<List<Keys>> GetKeys(Guid postId);
         
        Task<bool> SaveChanges();
    }
}
