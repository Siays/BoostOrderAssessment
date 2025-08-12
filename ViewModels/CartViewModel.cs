using BoostOrderAssessment.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

public class CartViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CartItemModel> CartItems { get; } = new();

    public ICommand RemoveFromCartCommand { get; }
    public ICommand ClearCartCommand { get; }
    public ICommand AddToCartCommand { get; }

    public decimal CartTotal => CartItems.Sum(i => i.LineTotal);
    public int ItemCount => CartItems.Count;
    public string SummaryText => $"Total ({ItemCount})";
    public int CartCount => CartItems.Sum(x => x.Quantity);

    public CartViewModel()
    {
        CartItems.CollectionChanged += CartItems_CollectionChanged;

        RemoveFromCartCommand = new RelayCommand<CartItemModel>(RemoveFromCart);
        ClearCartCommand = new RelayCommand(_ => ClearCart());
        AddToCartCommand = new RelayCommand<ProductDisplayModel>(AddToCart);
    }

    private void RemoveFromCart(CartItemModel item)
    {
        if (item == null) return;

        CartItems.Remove(item);
        NotifyTotalsChanged();
    }

    private void ClearCart()
    {
        CartItems.Clear();
        NotifyTotalsChanged();
    }

    private void AddToCart(ProductDisplayModel product)
    {
        if (product == null) return;

        var existing = CartItems.FirstOrDefault(x => x.ProductId == product.Id);
        int requestedQuantity = product.Quantity > 0 ? product.Quantity : 1;
        int alreadyInCart = existing?.Quantity ?? 0;
        int availableToAdd = product.Quantity - alreadyInCart;

        if (availableToAdd <= 0) return;

        int quantityToAdd = requestedQuantity > availableToAdd ? availableToAdd : requestedQuantity;

        if (existing != null)
        {
            existing.Quantity += quantityToAdd;
        }
        else
        {
            CartItems.Add(new CartItemModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantityToAdd,
                ImageUrl = product.ImageUrl,
                Sku = product.FirstSku,
                Product = product
            });
        }

        NotifyTotalsChanged();
    }

    private void NotifyTotalsChanged()
    {
        OnPropertyChanged(nameof(CartCount));
        OnPropertyChanged(nameof(CartTotal));
        OnPropertyChanged(nameof(ItemCount));
        OnPropertyChanged(nameof(SummaryText));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void CartItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => NotifyTotalsChanged();
}
