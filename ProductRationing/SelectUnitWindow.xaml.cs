using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductRationing
{
    public partial class SelectUnitWindow : Window
    {
        private CollectionView _itemsView;

        private readonly UnitRepo _unitRepo = new UnitRepo();

        public SelectUnitWindow()
        {
            InitializeComponent();
            Refresh();
        }

        public Unit Unit { get; set; }

        void Refresh()
        {
            var items = _unitRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Unit;
                return item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();

        void Select()
        {
            Unit = itemsDataGrid.SelectedItem as Unit;
            DialogResult = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Select();
        private void ItemsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Select();
    }
}
