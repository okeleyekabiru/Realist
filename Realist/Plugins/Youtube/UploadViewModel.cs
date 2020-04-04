using System;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Youtube
{
    public class UploadViewModel
    {
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public string[] VideoTags { get; set; } = new string[10];
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string CategoryId { get; set; }
    }
}

