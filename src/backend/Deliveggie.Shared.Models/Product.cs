using System;
using System.Collections.Generic;
using System.Text;

namespace Deliveggie.Shared.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public double PriceWithDeduction { get; set; }
    }
}
