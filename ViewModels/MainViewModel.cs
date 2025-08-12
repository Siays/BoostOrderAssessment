using BoostOrderAssessment.ViewModels;

public class MainViewModel
{
    public ProductViewModel ProductViewModel { get; } = new();
    public CartViewModel CartViewModel { get; } = new();
}