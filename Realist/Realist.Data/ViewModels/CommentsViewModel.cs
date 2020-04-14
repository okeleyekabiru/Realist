using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;

namespace Realist.Data.ViewModels
{
 public   class CommentsViewModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? Updated { get; set; }
        public ICollection<ReplyViewModel> Replies { get; set; }


    }
}
