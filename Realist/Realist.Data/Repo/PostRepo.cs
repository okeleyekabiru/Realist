using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public  async Task Post(Post post)
        {
            await _context.Posts.AddAsync(post);

           
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await _context.Posts.Include(r => r.Photos).Include(r => r.Videos).Include(r => r.Comments)
                .ThenInclude(r => r.Replies).Include(r => r.User).ToListAsync();

        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
