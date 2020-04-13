using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Realist.Data.Infrastructure;
using Realist.Data.ViewModels;
using Realist.Data.Model;
using Realist.Data.Services;

namespace Realist.Data.Repo
{
    public class CommentRepo : IComment
    {
        private readonly DataContext _context;

        public CommentRepo(DataContext context)
        {
            _context = context;
        }

        public async Task Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);

        }

        public async Task<Comment> GetComment(string commentId)
        {
            return await _context.Comments.Where(e =>  e.Id.Equals(Guid.Parse(commentId))).FirstOrDefaultAsync();
        }

        public async Task<ReturnResult> Delete(Comment comment)
        {
            _context.Comment.Remove(comment);
            if (await SaveChanges())
            {
                return new ReturnResult
                {
                    Succeeded =  true
                };


            }
            return     new ReturnResult
            {
                Succeeded = false,
                Error = "An error occured when updating database"
            };
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ReturnResult> Update(Comment comment)
        {
            _context.Update(comment);
            if (await   SaveChanges())
            {
                
                return new ReturnResult{
                Succeeded = true
                };
            }
            return new ReturnResult{
        Error = "An error occured while updating database"
            };
        }
         public async Task<IEnumerable<Comment>> GetAllPostComment(string postId)
        {
            return await _context.Comments.Include(r => r.Replies).Where(r => r.PostId.Equals(Guid.Parse(postId)))
                .ToListAsync();
        }
    }
}
