using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductRationing
{
    public partial class SelectProductWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private ProductRepo _productRepo = new ProductRepo();

        public SelectProductWindow()
        {
            InitializeComponent();
            Refresh();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                Refresh();
                _timer.Stop();
            };
        }

        public Product Product { get; set; }

        void Refresh()
        {
            var items = _productRepo.FindProducts(codeFilterTextBox.Text, nameFilterTextBox.Text);
            itemsDataGrid.ItemsSource = items;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void CodeFilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void NameFilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        void Select()
        {
            Product = itemsDataGrid.SelectedItem as Product;
            if (Product == null) return;
            DialogResult = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Select();
        private void ItemsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Select();
    }
}
