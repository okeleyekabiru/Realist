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
        public async Task<Guid> UploadImageDb(Photo photo)
        {
            photo.Id = Guid.NewGuid();
            photo.UploadTime = DateTime.Now;
            await _context.Photos.AddAsync(photo);
            return photo.Id;
        }

        public async Task<Photo> FindPhotoId(string postId, string photoId)
        {
            return await _context.Photos.Where(r => r.PostId.Equals(Guid.Parse(postId)) && r.PublicId.Equals(photoId))
                .FirstOrDefaultAsync();
        }

        public void DeleteImageFromDb(Photo photo)
        {
            _context.Photos.Remove(photo);
        }

        public Task Update(Photo photo)
        {
            _context.Attach(photo);
            _context.Entry(photo).State = EntityState.Modified;
            // _context.Update(photo);
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
