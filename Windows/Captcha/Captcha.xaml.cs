using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SportShop
{
    public partial class Captcha : Window
    {
        private readonly SportShopEntities _db;

        private readonly IEnumerable<User> _users;

        private const string errorEndingAttempts = "Вы заблокированы на 10 секунд!";

        public Captcha()
        {
            InitializeComponent();
            _db = new SportShopEntities();
            _users = new List<User>(_db.Users.ToList());

            GetCaptcha();
        }

        public void GetCaptcha()
        {
            StringBuilder allowchar = new StringBuilder(); ;

            allowchar.Append("A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z");
            allowchar.Append("a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z");
            allowchar.Append("1,2,3,4,5,6,7,8,9,0");

            char[] a = { ',' };
            String[] ar = allowchar.ToString().Split(a);
            StringBuilder pwd = new StringBuilder();
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[r.Next(0, ar.Length)];
                pwd.Append(temp);
            }

            CaptchaText.Text = pwd.ToString();
        }

        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            if(CaptchaUser.Text.ToString() == CaptchaText.Text.ToString())
            {
                foreach (var user in _users)
                {
                    if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 1)
                    {
                        new ClientWindow(user).ShowDialog();
                        return;
                    }
                    else if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 2)
                    {
                        new ClientWindow(user).ShowDialog();
                        return;
                    }
                    else if(user.UserLogin == UserLogin.Text && user.UserPassword == UserPassword.Text && user.RoleID == 3)
                    {
                        new ClientWindow(user).ShowDialog();
                        return;
                    }
                }
            }
            else
            {
                GetCaptcha();
                UserPassword.Text = "";
                MessageBox.Show(errorEndingAttempts);

                BlockUser();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(UnblockUser);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();
            }
        }

        private void BlockUser()
        {
            UserLogin.IsEnabled = false;
            UserPassword.IsEnabled = false;
            CaptchaUser.IsEnabled = false;
        }

        private void UnblockUser(object sender, EventArgs e)
        {
            UserLogin.IsEnabled = true;
            UserPassword.IsEnabled = true;
            CaptchaUser.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow().Show();
        }
    }
}
