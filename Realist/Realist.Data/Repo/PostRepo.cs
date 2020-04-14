using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;
using Realist.Data.ViewModels;

namespace Realist.Data.Repo
{
   
    public class PostRepo:IPost
    {
        private readonly DataContext _context;

        public PostRepo(DataContext context)
        {
            _context = context;
        }


     

        public  Task Delete(Post post)
        {
         
            _context.Posts.Remove(post);
        
return  Task.CompletedTask;
        }

        public  async Task Post(Post post)
        {
         
            await _context.Posts.AddAsync(post);
       

        }
        public async Task<long> GetCommentCount(string postId)
        {
            return await _context.Comments.Where(r => r.PostId.Equals(Guid.Parse(postId))).LongCountAsync();
        }

        public PagedList<Post> GetAll(PaginationModel page)
        {
            
          
            return  PagedList<Post>.ToPagedList(_context.Posts.Include(r => r.Photos).Include(r => r.Videos)
                ,page.PageNumber,page.PageSize);

        }

        public async Task<Post> Get(string postId)
        {
            return await _context.Posts.Include(r => r.Photos).Include(r => r.Videos)
               .Where(r => r.Id.Equals(Guid.Parse(postId))).FirstOrDefaultAsync();

        }

        public async Task<Post> GetPost(string postId)
        {
           var result = await _context.Posts.Include(r => r.Photos).Include(r => r.Videos).Where(r => r.Id.Equals(Guid.Parse(postId))).FirstOrDefaultAsync();
        return   result;
        }

        public Guid Update(Post post)
        {
            _context.Update(post);
            return post.Id;
        }

        public async Task<List<Keys>> GetKeys(Guid postId)
        {

           return await _context.Posts.Include(e => e.Photos).Include(e => e.Videos)
                .Where(r => r.Id.Equals(postId)).Select(e => new List<Keys>
                {
                 new Keys
                 {
                     PhotoId =  e.Photos.FirstOrDefault().PublicId,
                     VideoId =  e.Videos.FirstOrDefault().PublicId
                 }  

                }).FirstOrDefaultAsync();


        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
