using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using BoostOrderAssessment.ViewModels;

namespace BoostOrderAssessment.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        private int _quantity = 1;
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
                if (value < 1) value = 1;
                if (value > _stockQuantity) value = _stockQuantity;

                _quantity = value;
                UpdateStockDisplay();
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(CanIncrease));
                OnPropertyChanged(nameof(CanDecrease));
            }
        }

        public bool CanIncrease => Quantity < _stockQuantity;
        public bool CanDecrease => Quantity > 1;

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
            if (Quantity > 1)
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
    }
}
