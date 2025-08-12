using System.Windows;
using System.Windows.Controls;


namespace BoostOrderAssessment.Views
{
    public partial class CartWindow : Window
    {      
        public CartWindow(CartViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }
    }
}
