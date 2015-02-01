using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiKnockout.Model.DTO
{
    public class AlbumDTO
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
    }
}