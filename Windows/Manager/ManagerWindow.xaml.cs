using System.Windows;

namespace SportShop
{
    public partial class ManagerWindow : Window
    {
        private readonly User _user;

        public ManagerWindow(User user)
        {
            InitializeComponent();
            _user = user;
            UserFio.Content = string.Concat(_user.UserSurname, " ", _user.UserName, " ", _user.UserPatronymic);
        }
        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(_user).Show();
        }
    }
}
