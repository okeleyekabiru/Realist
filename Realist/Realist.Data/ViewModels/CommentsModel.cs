using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Realist.Data.ViewModels
{
  public  class CommentsModel
    {
        
        [Required]
        public string Body { get; set; }
        [Required]
        public string PostId { get; set; }

        public string CommentId { get; set; }
       
    }
}
