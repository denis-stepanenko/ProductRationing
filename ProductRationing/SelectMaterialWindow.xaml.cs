using ProductRationing.DAL.Data;
using ProductRationing.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductRationing
{
    public partial class SelectMaterialWindow : Window
    {
        private CollectionView _itemsView;
        private MaterialRepo _materialRepo = new MaterialRepo();

        public SelectMaterialWindow()
        {
            InitializeComponent();
            Refresh();
        }

        public Material Material { get; set; }

        void Refresh()
        {
            var items = _materialRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Material;
                return
                item.Code.ToLower().Contains(codeFilterTextBox.Text.ToLower()) &&
                item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void RefreshButton_Click_1(object sender, RoutedEventArgs e) => Refresh();

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        private void codeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();

        void Select()
        {
            Material = itemsDataGrid.SelectedItem as Material;
            DialogResult = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Select();
        private void ItemsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Select();
    }
}