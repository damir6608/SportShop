﻿using System;
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

namespace SportShop
{
    public partial class AdminWindow : Window
    {
        private readonly User _user;

        public AdminWindow(User user)
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
