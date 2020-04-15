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
  public  class UserInfoRepo:IUserInfo
    {
        private readonly DataContext _context;

        public UserInfoRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<ReturnResult> Add(UserInfo userInfo)
        {
          await  _context.UserInfo.AddAsync(userInfo);
          if (await _context.SaveChangesAsync() > 0)
          {
              return new ReturnResult
              {
                  Succeeded = true
              };
          }
          
            return new ReturnResult
            {
                Succeeded = false,
                Error = "An error occured while uploading to database"
            };
        }

        public async Task<UserInfo> Get(string deviceName, string ipAddress)
        {
            return await _context.UserInfo.Where(r => r.DeviceName.Equals(deviceName) && r.UserIpHost.Equals(ipAddress))
                .FirstOrDefaultAsync();
        }
    }
}
