using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;

namespace Realist.Data.ViewModels
{
   public class ReplyViewModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public ICollection<ReplyViewModel> Replies { get; set; }
    }
}
