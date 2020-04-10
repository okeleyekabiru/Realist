using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using Realist.Data.Model;

namespace Realist.Data.ViewModels
{
  public  class PostModel
    {
        public string Id { get; set; }
        [Required]
        public string Body { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile  Video { get; set; }
        public Category? Category { get; set; }
        public News? News { get; set; }
        public Articles? Articles { get; set; }
        public string VideoId { get; set; }
        public string ImageId { get; set; }

    }
}
