using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class SelectGroupWindow : Window
    {
        private CollectionView _itemsView;

        private readonly GroupRepo _groupRepo = new GroupRepo();

        public SelectGroupWindow()
        {
            InitializeComponent();
            Refresh();
        }

        public Group Group { get; set; }

        void Refresh()
        {
            var items = _groupRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Group;
                return item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void RefreshButton_Click_1(object sender, RoutedEventArgs e) => Refresh();

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();

        void Select()
        {
            Group = itemsDataGrid.SelectedItem as Group;
            DialogResult = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Select();
        private void ItemsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => Select();
    }
}
