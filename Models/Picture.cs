using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageFiligran.Models
{
    public class Picture : BaseHome
    {
        public string Title { get; set; }
        public string FiligranName { get; set; }
        public string ImageUrl { get; set; }
    }
}