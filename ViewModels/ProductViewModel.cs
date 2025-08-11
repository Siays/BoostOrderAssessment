using BoostOrderAssessment.Data;
using BoostOrderAssessment.Data.Entities;
using BoostOrderAssessment.Models;
using BoostOrderAssessment.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BoostOrderAssessment.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ProductDisplayModel> _products = new();
        private ObservableCollection<ProductDisplayModel> _allProducts = new();
        private string _searchText;

        public ObservableCollection<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilterProducts();
                    OnPropertyChanged();
                }
            }
        }

        private void FilterProducts()
        {
            Products.Clear();
            var filtered = string.IsNullOrWhiteSpace(_searchText)
                ? _allProducts
                : new ObservableCollection<ProductDisplayModel>(
                    _allProducts.Where(p => p.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)));

            foreach (var p in filtered) Products.Add(p);
        }

        public async Task LoadProductsAsync()
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            // Try fetching from API first
            var apiProducts = await ApiService.GetVariableProductsAsync();

            if (apiProducts?.Any() == true)
            {
                await SaveProductsToDatabase(apiProducts);
            }

            // Always load from DB
            LoadFromDatabase();
        }

        private async Task SaveProductsToDatabase(List<Product> apiProducts)
        {
            using var db = new AppDbContext();
            db.Products.RemoveRange(db.Products);
            db.Variations.RemoveRange(db.Variations);
            await db.SaveChangesAsync();

            foreach (var p in apiProducts)
            {
                DateTime.TryParse(p.DateModified, out DateTime parsedDate);

                var productEntity = new ProductEntity
                {
                    Id = p.Id,
                    Name = p.Name ?? "",
                    ImageUrl = p.Images?.FirstOrDefault()?.Src ?? "",
                    InStock = p.InStock,
                    DateModified = parsedDate
                };

                if (p.Variations != null && p.Variations.Any())
                {
                    foreach (var v in p.Variations)
                    {
                        decimal.TryParse(v.RegularPrice, out decimal parsedPrice);

                        int stockQty = 0;
                        if (v.Inventory != null && v.Inventory.Any())
                        {
                            stockQty = v.Inventory.Sum(i => i.StockQuantity);
                        }
                        else if (v.StockQuantity.HasValue)
                        {
                            stockQty = v.StockQuantity.Value;
                        }
                        else if (p.StockQuantity.HasValue)
                        {
                            stockQty = p.StockQuantity.Value;
                        }

                        productEntity.Variations.Add(new VariationEntity
                        {
                            Id = v.Id,
                            Sku = v.Sku ?? "",
                            RegularPrice = parsedPrice,
                            Uom = v.Attributes?.FirstOrDefault()?.Option ?? "Unit",
                            StockQuantity = Math.Max(0, stockQty)
                        });
                    }
                }

                db.Products.Add(productEntity);
            }

            await db.SaveChangesAsync();
        }

        private void LoadFromDatabase()
        {
            using var db = new AppDbContext();
            _allProducts.Clear();
            Products.Clear();

            var dbProducts = db.Products.ToList();

            foreach (var dbProduct in dbProducts)
            {
                var variations = db.Variations.Where(v => v.ProductEntityId == dbProduct.Id).ToList();
                var firstVariation = variations.FirstOrDefault();

                if (firstVariation == null) continue;

                var displayModel = new ProductDisplayModel
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name ?? "Unknown Product",
                    ImageUrl = string.IsNullOrEmpty(dbProduct.ImageUrl)
                        ? "/Assets/img_placeholder.png"
                        : dbProduct.ImageUrl,
                    FirstSku = firstVariation.Sku ?? $"SKU_{dbProduct.Id}",
                    PriceDisplay = FormatPrice(firstVariation.RegularPrice),
                    Units = variations
                        .Where(v => !string.IsNullOrEmpty(v.Uom))
                        .Select(v => v.Uom.ToUpper())
                        .Distinct()
                        .ToList()
                };

                displayModel.SetStockQuantity(firstVariation.StockQuantity);

                if (!displayModel.Units.Any())
                {
                    displayModel.Units.Add("UNIT");
                }

                // Only add if in stock
                if (displayModel.InStock)
                {
                    _allProducts.Add(displayModel);
                }
            }

            FilterProducts();
        }

        private string FormatPrice(decimal price) =>
            price > 0 ? $"RM{price:F2}" : "RM0.00";

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
