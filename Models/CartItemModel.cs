using BoostOrderAssessment.Models;
using System.ComponentModel;

public class CartItemModel : INotifyPropertyChanged
{
    private int _quantity;

    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }

    public string ImageUrl { get; set; } = "";
    public string Sku { get; set; } = "";

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(LineTotal));
            }
        }
    }

    public decimal LineTotal => UnitPrice * Quantity;

    public ProductDisplayModel Product { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
