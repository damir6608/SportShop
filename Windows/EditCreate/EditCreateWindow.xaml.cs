using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SportShop.Windows.EditCreate
{
    /// <summary>
    /// Interaction logic for EditCreateWindow.xaml
    /// </summary>
    public partial class EditCreateWindow : Window
    {
        private readonly SportShopEntities _db = new SportShopEntities();

        private readonly User _user;

        private readonly Product _product;

        private bool _isEdit;
        private string _imagePath;
        private const string successSave = "Успешно сохранено!";
        private const string savingError = "Данную запись нельзя удалить!";

        public EditCreateWindow(Product product, User user, bool isEdit)
        {
            InitializeComponent();
            _user = user;
            ProductGrid.DataContext = product;
            _product = product;
            _isEdit = isEdit;
            ProductArticleNumberTextBox.IsEnabled = !_isEdit;
            CategoryComboBox.ItemsSource = _db.ProductCategories.Select(x => x.ProductCategoryName).ToList();
            ManufacturerComboBox.ItemsSource = _db.ProductManufacturers.Select(x => x.ProductManufacturerName).ToList();
            ProductSupplierComboBox.ItemsSource = _db.ProductSuppliers.Select(x => x.ProductSupplierName).ToList();
            UnitTypeCombobox.ItemsSource = _db.UnitTypes.Select(u => u.UnitTypeName).ToList();
        }

        private void ExitButtom_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(_user).Show();
        }

        private void SaveButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isEdit)
                {
                    var updatedProduct = _db.Products.Find(_product.ProductID);
                    updatedProduct.ProductName = _product.ProductName;
                    updatedProduct.ProductCategoryID = _db.ProductCategories.ToList().Find(c => c.ProductCategoryName == CategoryComboBox.SelectedValue.ToString()).ProductCategoryID;
                    updatedProduct.ProductManufacturerID = _db.ProductManufacturers.ToList().Find(m => m.ProductManufacturerName == ManufacturerComboBox.SelectedValue.ToString()).ProductManufacturerID;
                    updatedProduct.ProductMaxDiscountAmount = _product.ProductMaxDiscountAmount;
                    updatedProduct.ProductDiscountAmount = _product.ProductDiscountAmount;
                    updatedProduct.ProductCost = _product.ProductCost;
                    updatedProduct.ProductDescription = _product.ProductDescription;
                }
                else
                {
                    _product.ProductManufacturerID = _db.ProductManufacturers.ToList().Find(m => m.ProductManufacturerName == ManufacturerComboBox.SelectedValue.ToString()).ProductManufacturerID;
                    _product.ProductCategoryID = _db.ProductCategories.ToList().Find(c => c.ProductCategoryName == CategoryComboBox.SelectedValue.ToString()).ProductCategoryID;
                    _product.ProductSupplierID = _db.ProductSuppliers.ToList().Find(s => s.ProductSupplierName == ProductSupplierComboBox.SelectedValue.ToString()).ProductSupplierID;
                    _product.UnitTypeID = _db.UnitTypes.ToList().Find(u => u.UnitTypeName == UnitTypeCombobox.SelectedValue.ToString()).UnitTypeID;
                    _product.ProductPhoto = _imagePath;
                    _db.Products.Add(_product);
                }

                _db.SaveChanges();
                MessageBox.Show(successSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Picture = _product.ProductPhoto;
                OpenFileDialog opFD = new OpenFileDialog();
                opFD.ShowDialog();
                var imag = opFD.FileName;
                var s = System.IO.Directory.GetCurrentDirectory().Replace("\\bin\\Debug", string.Empty).Replace("\\", "/");
                string dest =@s + "/Photos/" + System.IO.Path.GetFileName(imag);
                Image image = new Image();
                var bi = new BitmapImage(new Uri(imag));
                Photo.Source = bi;
                var pr = _db.Products.ToList().Find(f => f.ProductID == _product.ProductID);
                if (pr == null)
                    _imagePath = opFD.SafeFileName;
                else
                {
                    pr.ProductPhoto = opFD.SafeFileName;
                    _db.SaveChanges();
                }

                ProductGrid.DataContext = pr;
                File.Copy(imag, dest);
            }
            catch
            {

            }
        }

        private void DeleteButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = _db.Products.ToList().Find(p => p.ProductID == ((sender as Button).DataContext as Product).ProductID);
                _db.Products.Remove(product);
                _db.SaveChanges();
                Hide();
                new ProductsWindow(_user).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(savingError + ex.Message);
            }
        }
    }
}