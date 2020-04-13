using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;

namespace Realist.Data.ViewModels
{
  public  class PostViewModel
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Videos> Videos { get; set; }
        public Category? Category { get; set; }
        public News? News { get; set; }
        public Articles? Articles { get; set; }
    }

}
