using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
   public class Comment
    {
        public Comment()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? Updated { get; set; }
       public  ICollection<Reply> Replies { get; set; }
       public Guid PostId { get; set; }
       public string ReplyId { get; set; }

    }
}
