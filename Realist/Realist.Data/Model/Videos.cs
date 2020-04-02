using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
  public  class Videos
    {
        public Guid Id { get; set; }
        public  string Url { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UserId { get; set; }
    }
}
