using AgileObjects.AgileMapper.Extensions;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductRationing
{
    public partial class UnitsWindow : Window
    {
        private CollectionView _itemsView;

        private readonly UnitRepo _unitRepo = new UnitRepo();

        public UnitsWindow()
        {
            InitializeComponent();
            Refresh();
        }

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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new UnitWindow(null).ShowDialog();
            Refresh();
        }

        void Open()
        {
            var item = (Unit)itemsDataGrid.SelectedItem;
            if (item == null) return;

            new UnitWindow(item).ShowDialog();
            Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) => Open();
        private void ItemsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Open();

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.Cast<Unit>();
            if (items.Count() == 0) return;

            var dialog = MessageBox.Show("Удалить выбранные записи?", "Внимание", MessageBoxButton.YesNo);
            if (dialog != MessageBoxResult.Yes) return;

            items.ForEach(x => _unitRepo.Remove(x));

            Refresh();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
    }
}
