using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
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

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
