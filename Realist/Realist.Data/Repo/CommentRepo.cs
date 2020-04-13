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

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Comment>> GetAllPostComment(string postId)
        {
            return await _context.Comments.Include(r => r.Replies).Where(r => r.PostId.Equals(Guid.Parse(postId)))
                .ToListAsync();
        }
    }
}
