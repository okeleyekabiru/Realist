using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;
using Realist.Data.ViewModels;

namespace Realist.Data.Repo
{
public    class BotRepo:IBot
    {
        private readonly DataContext _context;

        public BotRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<ReturnResult> Add(BotInfo bot)
        {
            await _context.AddAsync(bot);
            if (await  _context.SaveChangesAsync() > 0)
            {
                return  new ReturnResult
                {
                    Succeeded = true
                };
            }
            return new ReturnResult
            {
                Succeeded = false,
                Error = "An error occured while updating database"
            };
        }
    }
}
