using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class SelectOperationWindow : Window
    {
        private CollectionView _itemsView;

        private readonly OperationRepo _operationRepo = new OperationRepo();

        public SelectOperationWindow()
        {
            InitializeComponent();
            Refresh();
        }

        public Operation Operation { get; set; }

        void Refresh()
        {
            var items = _operationRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Operation;
                return
                item.Department.ToString().Contains(departmentFilterTextBox.Text)
                && item.Code.ToLower().Contains(codeFilterTextBox.Text.ToLower())
                && item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower())
                && (item.Description ?? "").ToLower().Contains(descriptionFilterTextBox.Text.ToLower())
                && item.Unit.Name.ToLower().Contains(unitFilterTextBox.Text.ToLower())
                && item.Group.Name.ToLower().Contains(groupFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        void Select()
        {
            Operation = itemsDataGrid.SelectedItem as Operation;
            if (Operation == null) return;
            DialogResult = true;
        }

        private void ItemsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => Select();
        private void OkButton_Click(object sender, RoutedEventArgs e) => Select();

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void DepartmentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void CodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void DescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void UnitFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void GroupFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
    }
}
