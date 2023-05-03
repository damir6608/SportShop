using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SportShop
{
    public partial class Authorization : Window
    {
        private readonly SportShopEntities _db; 

        private IEnumerable<User> _users;

        private const string errorMessage = "Введен неверный логин или пароль!";

        public Authorization()
        {
            InitializeComponent();
            _db = new SportShopEntities();
            _users = new List<User>(_db.Users.ToList());
        }

        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(var user in _users)
            {
                if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 1)
                {
                    Hide();
                    new ClientWindow(user).ShowDialog();
                    return;
                }
                else if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 2)
                {
                    Hide();
                    new ClientWindow(user).ShowDialog();
                    return;
                }
                else if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 3)
                {
                    Hide();
                    new ClientWindow(user).ShowDialog();
                    return;
                }
            }
            Hide();
            MessageBox.Show(errorMessage);
            new Captcha().ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow().Show(); 
        }
    }
}