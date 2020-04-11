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
using Realist.Data.ViewModels;

namespace Realist.Data.Repo
{
  public  class VideoRepo:IVideo
    {
        private readonly DataContext _context;


        public VideoRepo(DataContext context)
        {
            _context = context;
           
        }
        public async Task<Guid> Post(Videos video)
        { video.Id = Guid.NewGuid();
            video.DateUploaded = DateTime.Now;
            await _context.AddAsync(video);
            return video.Id;
        }

        public  Task Delete(Videos video)
        {
            _context.Videos.Remove(video);
            return  Task.CompletedTask;
        }

        public async Task<Videos> GetVideoPublicId(string postId, string videoId)
        {
            return await _context.Videos.Where(r => r.PublicId.Equals(videoId) && r.PostId.Equals(Guid.Parse(postId)))
               .FirstOrDefaultAsync();
        }

        public Task Update(Videos videos)
        {
            _context.Update(videos);
            return  Task.CompletedTask;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
