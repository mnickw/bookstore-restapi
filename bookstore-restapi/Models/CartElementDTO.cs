﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Models
{
    public class CartElementDTO
    {
        public string UserId { get; set; }
        public long BookId { get; set; }
        public int BookQnty { get; set; }
    }
}