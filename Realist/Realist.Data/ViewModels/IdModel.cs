using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Realist.Data.ViewModels
{
    public class IdModel
    {
      [Required]
        public string PostId { get; set; }
        [Required]
        public string CommentId {get;set;}

    }
}