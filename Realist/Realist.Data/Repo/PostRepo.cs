using System;
using System.Collections.Generic;
using System.Linq;
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

        public PagedList<Post> GetAll(PaginationModel page)
        {
            
          
            return  PagedList<Post>.ToPagedList(_context.Posts.Include(r => r.Photos).Include(r => r.Videos).Include(r => r.Comments)
                .ThenInclude(r => r.Replies),page.PageNumber,page.PageSize);

        }

        public async Task<Post> Get(string postId)
        {
            return await _context.Posts.Where(r => r.Id.Equals(Guid.Parse(postId))).Include(r => r.Photos).Include(r => r.Videos)
                .Include(r => r.Comments).ThenInclude(r => r.Replies).FirstOrDefaultAsync();

        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
