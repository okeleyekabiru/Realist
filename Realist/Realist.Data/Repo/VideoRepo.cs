using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;

namespace Realist.Data.Repo
{
  public  class VideoRepo:IVideo
    {
        private readonly DataContext _context;


        public VideoRepo(DataContext context)
        {
            _context = context;
           
        }
        public async Task Post(Videos video)
        {
            await _context.AddAsync(video);
        }

        public async Task<string> GetVideoPublicId(string postId, string videoId)
        {
            return await _context.Videos.Where(r => r.PublicId.Equals(videoId) && r.PostId.Equals(Guid.Parse(postId)))
                .Select(r => r.PublicId).FirstOrDefaultAsync();
        }

        public Task Update(Videos videos)
        {
            _context.Entry(videos).State = EntityState.Modified;
            return  Task.CompletedTask;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
