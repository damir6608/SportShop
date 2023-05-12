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

namespace SportShop.Windows.Order
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private SportShopEntities _db;
        private const string unauthorizedUser = "Неавторизированный пользователь";
        public OrderWindow(int orderId)
        {
            InitializeComponent();
            _db = new SportShopEntities();
            var order = _db.Orders.Where(i => i.OrderID == orderId).FirstOrDefault();
            ListProducts.ItemsSource = order.OrderProducts.Select(item => item.Product);

            if (order.User != null)
                UserFio.Content = string.Concat(order.User.UserSurname, " ", order.User.UserName, " ", order.User.UserName);
            else
                UserFio.Content = unauthorizedUser;
        }
    }
}
