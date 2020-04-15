using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Infrastructure
{
  public  interface IBot
  {
      Task<ReturnResult> Add(BotInfo bot);
  }
}
