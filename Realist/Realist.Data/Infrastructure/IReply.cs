using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Infrastructure
{
   public interface IReply
   {
       Task<ReturnResult> Add(Reply reply);
       Task<bool> SaveChanges();
   }
}
