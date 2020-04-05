using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Realist.Data.Model
{
  public  class Videos
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UserId { get; set; }
        public string 
            
            PublicId { get; set; }
        public Guid PostId { get; set; }
        
    }
}
