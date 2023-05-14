using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly SportShop.Order _order;
        public OrderWindow(int orderId)
        {
            InitializeComponent();
            _db = new SportShopEntities();
            var order = _db.Orders.Where(i => i.OrderID == orderId).FirstOrDefault();
            _order = order;
            ListProducts.ItemsSource = order.OrderProducts.Select(item => item.Product);

            if (order.User != null)
                UserFio.Content = string.Concat(order.User.UserSurname, " ", order.User.UserName, " ", order.User.UserName);
            else
                UserFio.Content = unauthorizedUser;
        }

        private void SavePDF_Click(object sender, RoutedEventArgs e)
        {
            var orders = _order;
            var app = new Microsoft.Office.Interop.Excel.Application
            {
                SheetsInNewWorkbook = 1
            };
            var workbook = app.Workbooks.Add(Type.Missing);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = app.Worksheets.Item[1];
            worksheet.Name = "Card";

            worksheet.Cells[1][1] = "Order number";
            worksheet.Cells[1][2] = "Product list";
            worksheet.Cells[1][3] = "Total cost";

            worksheet.Cells[2][1] = orders.OrderID;

            var fullProductList = string.Empty;
            fullProductList = orders.PickupPoint.Address;
            worksheet.Cells[2][2] = fullProductList;
            worksheet.Cells[2][3] = orders.OrderDeliveryDate;

            worksheet.Columns.AutoFit();

            app.Visible = true;

            app.Application.ActiveWorkbook.SaveAs(@Directory.GetCurrentDirectory().Replace("bin\\Debug", "") + @"test.xlsx");

            var excelDocument = app.Workbooks.Open(@Directory.GetCurrentDirectory().Replace("bin\\Debug", "") + @"test.xlsx");
            excelDocument.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, @Directory.GetCurrentDirectory().Replace("bin\\Debug", "") + @"test.pdf");
            excelDocument.Close(false, "", false);
            app.Quit();
        }
    }
}
