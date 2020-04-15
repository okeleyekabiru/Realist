using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;
using Realist.Data.ViewModels;
using System.Threading.Tasks;
namespace Realist.Data.Infrastructure
{
   public interface IReply
   {
       Task<ReturnResult> Add(Reply reply);
        Task<Reply> Get(string replyId);
        Task<ReturnResult> Update(Reply reply);
       Task<bool> SaveChanges();
       Task<ReturnResult> Delete(Reply reply);
   }
}
