using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
  public  class Photo
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; } = false;
        public DateTime UploadTime { get; set; }
        public string PublicId { get; set; }
        public string UserId { get; set; }
    }
}
