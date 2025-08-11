using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostOrderAssessment.Data.Entities
{
    public class VariationEntity
    {
        public int Id { get; set; }                 // API variation ID
        public int ProductEntityId { get; set; }    // Foreign key
        public ProductEntity Product { get; set; }  // Navigation back to product

        public string Sku { get; set; }
        public decimal RegularPrice { get; set; }
        public string Uom { get; set; }
        public int StockQuantity { get; set; }
    }
}

