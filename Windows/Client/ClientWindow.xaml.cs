using System.Windows;

namespace SportShop
{
    public partial class ClientWindow : Window
    {
        private readonly User _user;

        public ClientWindow(User user)
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
