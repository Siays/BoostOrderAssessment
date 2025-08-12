using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BoostOrderAssessment.Models
{
    public class ProductResponse
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_modified")]
        public string DateModified { get; set; } // string for safe parsing

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("manage_stock")]
        public bool ManageStock { get; set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; set; }

        [JsonProperty("stock_quantity")]
        public int? StockQuantity { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }

        [JsonProperty("variations")]
        public List<Variation> Variations { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Image
    {
        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("src_small")]
        public string SrcSmall { get; set; }

        [JsonProperty("src_medium")]
        public string SrcMedium { get; set; }

        [JsonProperty("src_large")]
        public string SrcLarge { get; set; }

        [JsonProperty("youtube_video_url")]
        public string YoutubeVideoUrl { get; set; }
    }

    public class Attribute
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("variation")]
        public bool Variation { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }
    }

    public class Variation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date_modified")]
        public string DateModified { get; set; } // string for safe parsing

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("regular_price")]
        public string RegularPrice { get; set; } // string for safe parsing

        [JsonProperty("manage_stock")]
        public bool ManageStock { get; set; }

        [JsonProperty("in_stock")]
        public bool InStock { get; set; }

        [JsonProperty("stock_quantity")]
        public int? StockQuantity { get; set; }

        [JsonProperty("attributes")]
        public List<VariationAttribute> Attributes { get; set; }

        [JsonProperty("inventory")]
        public List<Inventory> Inventory { get; set; }
    }

    public class VariationAttribute
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("option")]
        public string Option { get; set; }
    }

    public class Inventory
    {
        [JsonProperty("branch_id")]
        public int BranchId { get; set; }

        [JsonProperty("batch_id")]
        public int? BatchId { get; set; }

        [JsonProperty("stock_quantity")]
        public int StockQuantity { get; set; }

        [JsonProperty("physical_stock_quantity")]
        public int PhysicalStockQuantity { get; set; }
    }
}
