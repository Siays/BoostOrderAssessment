using BoostOrderAssessment.Data;
using BoostOrderAssessment.Data.Entities;
using BoostOrderAssessment.Models;
using BoostOrderAssessment.Services;
using BoostOrderAssessment.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static BoostOrderAssessment.Views.CartWindow;


namespace BoostOrderAssessment.Views
{
    public partial class MainWindow : Window
    {
        private ProductViewModel viewModel = new ProductViewModel();
        private CartViewModel cartViewModel = new CartViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadProductsAsync();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(cartViewModel); // Pass the viewModel here
            cartWindow.Owner = this;
            cartWindow.ShowDialog();
        }
    }
}