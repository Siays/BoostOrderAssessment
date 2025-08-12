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
        private MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.ProductViewModel.LoadProductsAsync();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(viewModel.CartViewModel);
            cartWindow.Owner = this;
            cartWindow.ShowDialog();
        }
    }
}