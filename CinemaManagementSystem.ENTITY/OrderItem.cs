﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.ENTITY
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; } //navigation property
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
         
        
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
