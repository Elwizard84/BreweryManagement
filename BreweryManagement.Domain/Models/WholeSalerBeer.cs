﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Models
{
    public class WholeSalerBeer
    {
        public WholeSaler WholeSaler { get; set; }
        public Beer Beer { get; set; }
        public int Quantity { get; set; }
    }
}