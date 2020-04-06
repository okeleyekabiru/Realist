using System;
using System.Collections.Generic;
using System.Text;

namespace Realist.Data.ViewModels
{
  public  class UserReturnModel
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
