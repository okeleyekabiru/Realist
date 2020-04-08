using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
    public interface IPost
    {
        Task Post(Post post);
        PagedList<Post> GetAll(PaginationModel page);
        Task<Post> Get(string postId);
        Task<bool> SaveChanges();
    }
}
