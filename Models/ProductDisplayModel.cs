using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using BoostOrderAssessment.ViewModels;

namespace BoostOrderAssessment.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        private int _quantity = 0;
        private int _stockQuantity;
        private string _stockDisplay = "";

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public bool InStock => _stockQuantity > 0;
        public string FirstSku { get; set; } = "";
        public string PriceDisplay { get; set; } = "";
        public decimal Price { get; set; }
        public List<string> Units { get; set; } = new();
        public Dictionary<string, string> UnitSkus { get; set; } = new();


        private string _sku = "";
        public string Sku
        {
            get => _sku;
            private set
            {
                if (_sku != value)
                {
                    _sku = value;
                    OnPropertyChanged(nameof(Sku));
                }
            }
        }

        public string StockDisplay
        {
            get => _stockDisplay;
            private set
            {
                _stockDisplay = value;
                OnPropertyChanged(nameof(StockDisplay));
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 1) value = 0;
                if (value > _stockQuantity) value = _stockQuantity;

                _quantity = value;
                UpdateStockDisplay();
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(CanIncrease));
                OnPropertyChanged(nameof(CanDecrease));
            }
        }

        public bool CanIncrease => Quantity < _stockQuantity;
        public bool CanDecrease => Quantity > 0;

        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }

        public ProductDisplayModel()
        {
            IncreaseCommand = new RelayCommand(_ => Increase(), _ => CanIncrease);
            DecreaseCommand = new RelayCommand(_ => Decrease(), _ => CanDecrease);
        }

        public void Increase()
        {
            if (Quantity < _stockQuantity)
                Quantity++;
        }

        public void Decrease()
        {
            if (Quantity > 0)
                Quantity--;
        }

        public void SetStockQuantity(int stockQty)
        {
            _stockQuantity = stockQty;
            UpdateStockDisplay();
            OnPropertyChanged(nameof(InStock));
            OnPropertyChanged(nameof(CanIncrease));
        }

        private void UpdateStockDisplay()
        {
            StockDisplay = _stockQuantity > 0
                ? $"Stock: {_stockQuantity - Quantity}"
                : "Out of stock";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _selectedUnit;
        public Dictionary<string, decimal> UnitPrices { get; set; } = new();

        public string SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                if (_selectedUnit != value)
                {
                    _selectedUnit = value;

                    if (UnitPrices.TryGetValue(value, out var newPrice))
                    {
                        Price = newPrice;
                        PriceDisplay = $"RM{newPrice:F2}";
                        OnPropertyChanged(nameof(Price));
                        OnPropertyChanged(nameof(PriceDisplay));
                    }

                    if (UnitSkus.TryGetValue(value, out var newSku))
                    {
                        UpdateSku(newSku);
                    }
                    else
                    {
                        UpdateSku(FirstSku);
                    }

                    OnPropertyChanged(nameof(SelectedUnit));
                }
            }
        }

        public void UpdateSku(string newSku)
        {
            if (_sku != newSku)
            {
                _sku = newSku;
                OnPropertyChanged(nameof(Sku));
            }
        }
    }
}
