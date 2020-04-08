using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;

namespace Realist.Data.ViewModels
{
  public  class PostModel
    {
        public string Body { get; set; }
        public Photo Photo { get; set; }
        public Videos Video { get; set; }
        public Category? Category { get; set; }
        public News? News { get; set; }
        public Articles? Articles { get; set; }
    }
}
