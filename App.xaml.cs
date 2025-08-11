using BoostOrderAssessment.Data;
using BoostOrderAssessment.Data.Entities;
using BoostOrderAssessment.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BoostOrderAssessment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var products = await ApiService.GetVariableProductsAsync();

                using (var db = new AppDbContext())
                {
                    db.Database.EnsureCreated();
                    db.Products.RemoveRange(db.Products);
                    db.SaveChanges();

                    foreach (var apiProduct in products)
                    {
                        var productEntity = new ProductEntity
                        {
                            Id = apiProduct.Id,
                            Name = apiProduct.Name,
                            ImageUrl = apiProduct.Images?.FirstOrDefault()?.Src,
                            InStock = apiProduct.InStock,
                            DateModified = DateTime.TryParse(apiProduct.DateModified, out var dt) ? dt : DateTime.MinValue
                        };

                        foreach (var v in apiProduct.Variations)
                        {
                            productEntity.Variations.Add(new VariationEntity
                            {
                                Id = v.Id,
                                Sku = v.Sku,
                                RegularPrice = decimal.TryParse(v.RegularPrice, out var price) ? price : 0,
                                Uom = v.Attributes?.FirstOrDefault()?.Option ?? "",
                                StockQuantity = v.StockQuantity ?? 0
                            });
                        }

                        db.Products.Add(productEntity);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }

            new BoostOrderAssessment.Views.MainWindow().Show();

        }


    }

}
