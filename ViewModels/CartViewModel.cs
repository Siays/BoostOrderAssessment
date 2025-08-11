using BoostOrderAssessment.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class CartViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CartItemModel> CartItems { get; set; } = new();

    public ICommand RemoveFromCartCommand { get; }
    public ICommand ClearCartCommand { get; }

    public decimal CartTotal => CartItems.Sum(i => i.LineTotal);
    public int ItemCount => CartItems.Count;
    public string SummaryText => $"Total ({ItemCount})";

    public CartViewModel()
    {
        RemoveFromCartCommand = new RelayCommand<CartItemModel>(RemoveFromCart);
        ClearCartCommand = new RelayCommand(_ => ClearCart());

    }

    private void RemoveFromCart(CartItemModel item)
    {
        CartItems.Remove(item);
        OnPropertyChanged(nameof(CartTotal));
        OnPropertyChanged(nameof(ItemCount));
        OnPropertyChanged(nameof(SummaryText));
    }

    private void ClearCart()
    {
        CartItems.Clear();
        OnPropertyChanged(nameof(CartTotal));
        OnPropertyChanged(nameof(ItemCount));
        OnPropertyChanged(nameof(SummaryText));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
