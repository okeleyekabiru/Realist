using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;

namespace Realist.Data.Model
{
    public class Post
    {
    
        public Post()
        {
            DatePosted = DateTime.Now;
        }
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime? Updated { get; set; }
        public  User User{ get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string UserId { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public  ICollection<Videos> Videos { get; set; }
        public Category? Category { get; set; }
        public  News? News { get; set; }
        public Articles? Articles { get; set; }

        

    }
}