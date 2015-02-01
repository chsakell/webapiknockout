﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiKnockout.Model.DTO
{
    public class TrackDTO
    {
        public int TrackId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Milliseconds { get; set; }
        public decimal UnitPrice { get; set; }
    }
}