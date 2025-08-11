using BoostOrderAssessment.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoostOrderAssessment.Data.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }                 // API product ID
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool InStock { get; set; }
        public DateTime DateModified { get; set; }

        // Navigation property: one product can have many variations
        public ICollection<VariationEntity> Variations { get; set; } = new List<VariationEntity>();

        public string DisplayImageUrl => string.IsNullOrWhiteSpace(ImageUrl)
        ? "pack://application:,,,/Assets/img_placeholder.png"
        : ImageUrl;

        public decimal? FirstVariationPrice => Variations?.FirstOrDefault()?.RegularPrice;
        public int? FirstVariationStock => Variations?.FirstOrDefault()?.StockQuantity;
    }
}

