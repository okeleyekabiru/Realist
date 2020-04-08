using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;

namespace Realist.Data.Repo
{
   public class PostRepo:IPost
    {
        private readonly DataContext _context;

        public PostRepo(DataContext context)
        {
            _context = context;
        }
        public  async Task<bool> Post(Post post)
        {
            await _context.Posts.AddAsync(post);

            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
