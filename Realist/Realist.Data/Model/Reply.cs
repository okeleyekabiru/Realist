using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
 public   class Reply
    {
        public Reply()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? Updated { get; set; }
        public Guid CommentId { get; set; }
        public  Comment Comment { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public Guid? ReplyId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Guid? UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

    }
}
