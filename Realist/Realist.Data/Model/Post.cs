using System;
using System.Collections.Generic;

namespace Realist.Data.Model
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? Updated { get; set; }
        public  User User{ get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string UserId { get; set; }
    }
}