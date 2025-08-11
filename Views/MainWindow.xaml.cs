using BoostOrderAssessment.Data;
using BoostOrderAssessment.Data.Entities;
using BoostOrderAssessment.Models;
using BoostOrderAssessment.Services;
using BoostOrderAssessment.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace BoostOrderAssessment.Views
{
    public partial class MainWindow : Window
    {
        private ProductViewModel viewModel = new ProductViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadProductsAsync();
        }

       
    }
}