using SportShop.Windows.EditCreate;
using SportShop.Windows.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportShop
{
    public partial class ProductsWindow : Window
    {
        private SportShopEntities _db;

        private User _user;

        private const string recordsShown = "Показано записей";
        private const string unauthorizedUser = "Неавторизированный пользователь";

        private Order _order;

        public ProductsWindow(User user = null)
        {
            InitializeComponent();
            _db = new SportShopEntities();
            _user = user;
            InitializeOrder();

            ListProducts.ItemsSource = _db.Products.ToList();

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
                        itemsSource = _db.Products.Where(p => p.ProductMaxDiscountAmount < 10);
                        break;
                    }
                case 1:
                    {
                        itemsSource = _db.Products.Where(p => p.ProductMaxDiscountAmount > 10 && p.ProductMaxDiscountAmount < 15);
                        break;
                    }
                case 2:
                    {
                        itemsSource = _db.Products.Where(p => p.ProductMaxDiscountAmount > 15);
                        break;
                    }
                case 3:
                    {
                        itemsSource = _db.Products;
                        break;
                    }
            }
            ListProducts.ItemsSource = itemsSource.ToList();
            CounterList.Content = $"{recordsShown} {ListProducts.Items.Count} из {_db.Products.ToList().Count}";
        }

        private void OrderByFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<Product> itemsSource = new Product[0];
            switch (OrderByFilter.SelectedIndex)
            {
                case 0:
                    {
                        itemsSource = _db.Products;
                        break;
                    }
                case 1:
                    {
                        itemsSource = _db.Products.OrderBy(x => x.ProductCost);
                        break;
                    }
                case 2:
                    {
                        itemsSource = _db.Products.OrderByDescending(x => x.ProductCost);
                        break;
                    }
            }
            ListProducts.ItemsSource = itemsSource.ToList();
            CounterList.Content = $"{recordsShown} {ListProducts.Items.Count} из {_db.Products.ToList().Count}";
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListProducts.ItemsSource = _db.Products.Where(x => x.ProductName.Contains(Search.Text)).ToList();
            CounterList.Content = $"{recordsShown} { ListProducts.Items.Count } из { _db.Products.ToList().Count }";
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var product = (sender as Button)?.DataContext as Product;
            new EditCreateWindow(product, _user, true).Show();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new EditCreateWindow(new Product(), _user, false).Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as MenuItem)?.DataContext as Product;
            _db.OrderProducts.Add(new OrderProduct()
            {
                ProductID = product.ProductID,
                Product = product,
                OrderID = _order.OrderID,
                Order = _order,
                Count = 1
            });
            _db.SaveChanges();
            ShowOrderButton.Visibility= Visibility.Visible;
        }

        private void InitializeOrder()
        {
            _order = new Order() 
            { 
                User = _user,
                OrderGetCode = new Random().Next(100, 999)
            };
            if (_user != null)
            {
                _order.UserID = _user.UserID;
            }

            _order.OrderCreateDate = DateTime.Now;
            _order.OrderDeliveryDate = DateTime.Now.AddDays(1);

            _order.OrderStatusID = _db.OrderStatus
                .Where(item => item.OrderStatusName == "Новый")
                .FirstOrDefault().OrderStatusID;

            _order.PickupPointID = _db.PickupPoints
                .FirstOrDefault().PickupPointID;

            Order order = _db.Orders.Add(_order);
            _db.SaveChanges();
            _order = order;
        }

        private void ShowOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new OrderWindow(_order.OrderID).ShowDialog();
        }
    }
}
