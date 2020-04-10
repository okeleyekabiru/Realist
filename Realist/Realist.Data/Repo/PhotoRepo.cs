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
 public   class PhotoRepo:IPhoto
    {
        private readonly DataContext _context;

        public PhotoRepo(DataContext context)
        {
            _context = context;
        }
        public async Task UploadImageDb(Photo photo)
        {
            await _context.Photos.AddAsync(photo);
        }

        public async Task<string> FindPhotoId(string postId, string photoId)
        {
            return await _context.Photos.Where(r => r.PostId.Equals(Guid.Parse(postId)) && r.PublicId.Equals(photoId))
                .Select(r => r.PublicId).FirstOrDefaultAsync();
        }

        public void DeleteImageFromDb(Photo photo)
        {
            _context.Photos.Remove(photo);
        }

        public Task Update(Photo photo)
        {
            _context.Entry(photo).State = EntityState.Modified;
                return Task.CompletedTask;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> FindUserImage(string userId)
        {
            return await _context.Photos.AnyAsync(r => r.UserId.Equals(userId));
        }
    }
}
