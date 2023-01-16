using AgileObjects.AgileMapper.Extensions;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class GroupsWindow : Window
    {
        private CollectionView _itemsView;
        private readonly GroupRepo _repo = new GroupRepo();

        public GroupsWindow()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            var items = _repo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Group;
                return item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new GroupWindow(null).ShowDialog();
            Refresh();
        }

        void Open()
        {
            var item = (Group)itemsDataGrid.SelectedItem;
            if (item == null) return;

            new GroupWindow(item).ShowDialog();
            Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) => Open();
        private void ItemsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => Open();

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.Cast<Group>();
            if (items.Count() == 0) return;

            var dialog = MessageBox.Show("Удалить выбранные записи?", "Внимание", MessageBoxButton.YesNo);
            if (dialog != MessageBoxResult.Yes) return;

            items.ForEach(x => _repo.Remove(x));

            Refresh();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        private void NameFilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => _itemsView.Refresh();
    }
}
