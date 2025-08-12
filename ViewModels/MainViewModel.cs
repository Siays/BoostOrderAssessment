using BoostOrderAssessment.ViewModels;
using MaterialDesignThemes.Wpf;

public class MainViewModel
{
    public ProductViewModel ProductViewModel { get; } = new();
    public CartViewModel CartViewModel { get; } = new();

    public SnackbarMessageQueue SnackbarMessageQueue { get; }

    public MainViewModel()
    {
        SnackbarMessageQueue = new SnackbarMessageQueue();

        // Assign SnackbarMessageQueue to CartViewModel so it can enqueue messages
        CartViewModel.SnackbarMessageQueue = SnackbarMessageQueue;
    }
}
