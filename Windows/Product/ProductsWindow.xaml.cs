using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportShop
{
    public partial class ProductsWindow : Window
    {
        private SportShopEntities db;

        private User _user;

        private const string recordsShown = "Показано записей";
        private const string unauthorizedUser = "Неавторизированный пользователь";

        public ProductsWindow(User user = null)
        {
            InitializeComponent();
            db = new SportShopEntities();
            _user = user;

            ListProducts.ItemsSource = db.Products.ToList();

            ComboBoxFilterProductDiscountAmount.ItemsSource = new string[]
            {
                "0-10%", "10-15%", "15-∞%", "All ranges"
            };

            OrderByFilter.ItemsSource = new string[]
            {
                "Сброс", "По возрастанию", "По убыванию"
            };

            if (_user != null)
                UserFio.Content = string.Concat(_user.UserSurname, " ", _user.UserName, " ", _user.UserName);
            else
                UserFio.Content = unauthorizedUser;

            CounterList.Content = $"{recordsShown} { ListProducts.Items.Count } из { ListProducts.Items.Count }";
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new Authorization().Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void ComboBoxFilterProductDiscountAmount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<Product> itemsSource = new Product[0];
            switch (ComboBoxFilterProductDiscountAmount.SelectedIndex)
            {
                case 0:
                    {
                        itemsSource = db.Products.Where(p => p.ProductMaxDiscountAmount < 10);
                        break;
                    }
                case 1:
                    {
                        itemsSource = db.Products.Where(p => p.ProductMaxDiscountAmount > 10 && p.ProductMaxDiscountAmount < 15);
                        break;
                    }
                case 2:
                    {
                        itemsSource = db.Products.Where(p => p.ProductMaxDiscountAmount > 15);
                        break;
                    }
                case 3:
                    {
                        itemsSource = db.Products;
                        break;
                    }
            }
            ListProducts.ItemsSource = itemsSource.ToList();
            CounterList.Content = $"{recordsShown} {ListProducts.Items.Count} из {db.Products.ToList().Count}";
        }

        private void OrderByFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<Product> itemsSource = new Product[0];
            switch (OrderByFilter.SelectedIndex)
            {
                case 0:
                    {
                        itemsSource = db.Products;
                        break;
                    }
                case 1:
                    {
                        itemsSource = db.Products.OrderBy(x => x.ProductCost);
                        break;
                    }
                case 2:
                    {
                        itemsSource = db.Products.OrderByDescending(x => x.ProductCost);
                        break;
                    }
            }
            ListProducts.ItemsSource = itemsSource.ToList();
            CounterList.Content = $"{recordsShown} {ListProducts.Items.Count} из {db.Products.ToList().Count}";
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListProducts.ItemsSource = db.Products.Where(x => x.ProductName.Contains(Search.Text)).ToList();
            CounterList.Content = $"{recordsShown} { ListProducts.Items.Count } из { db.Products.ToList().Count }";
        }
    }
}
