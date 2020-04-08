using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.Model
{
  public  class BotInfo
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Producer { get; set; }
        public string ProducerUrl { get; set; }
        public string Url { get; set; }
        public User User { get; set; }
    }
}
