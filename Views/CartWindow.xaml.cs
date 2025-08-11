using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BoostOrderAssessment.ViewModels;

namespace BoostOrderAssessment.Views
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private CartViewModel cartViewModel = new CartViewModel();

        public CartWindow(CartViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(cartViewModel);
            cartWindow.Owner = this;
            cartWindow.ShowDialog();
        }
    }
}
