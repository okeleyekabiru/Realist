﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;
using Realist.Data.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Realist.Data.Repo
{
   public class ReplyRepo:IReply
    {
        private readonly DataContext _context;

        public ReplyRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<ReturnResult> Add(Reply reply)
        {
            await _context.Replies.AddAsync(reply);
            if (await  SaveChanges())
            {
                return new ReturnResult
                {
                    Succeeded = true
                };
            }
            return new ReturnResult
            {
                Succeeded = false,
                Error = "error uploading to database"
            };
        }


        public async Task<ReturnResult> Update(Reply reply)
        {
            var model =_context.Update(reply);
            if (await  SaveChanges())
            {
                return  new ReturnResult
                {
                    Succeeded = true
                };
            }

            return new ReturnResult
            {
                Succeeded = false,
                Error = "An error occured when updating database"
            };
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ReturnResult> Delete(Reply reply)
        {
            _context.Remove(reply);
            if (await  SaveChanges())
            {
                return new ReturnResult
                {
                    Succeeded = true

                };
            }

            return new ReturnResult
            {
                Succeeded = false,
                Error = "An error occured while updating entity"

            };
        }

        public async  Task<Reply> Get(string replyId){
             return await   _context.Replies.Include(r => r.Replies).Where(s => s.Id.Equals(Guid.Parse(replyId))).FirstOrDefaultAsync();
         }
    }
}
