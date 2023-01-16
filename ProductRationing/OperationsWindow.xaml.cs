using AgileObjects.AgileMapper;
using AgileObjects.AgileMapper.Extensions;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class OperationsWindow : Window
    {
        private CollectionView _itemsView;

        private readonly OperationRepo _operationRepo = new OperationRepo();

        public OperationsWindow()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            var items = _operationRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = e =>
            {
                var item = e as Operation;
                return
                item.Department.ToString().Contains(departmentFilterTextBox.Text)
                && item.Code.ToLower().Contains(codeFilterTextBox.Text.ToLower())
                && item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower())
                && (item.Description ?? "").ToLower().Contains(descriptionFilterTextBox.Text.ToLower())
                && item.Unit.Name.ToLower().Contains(unitFilterTextBox.Text.ToLower())
                && item.Group.Name.ToLower().Contains(groupFilterTextBox.Text.ToLower())
                && item.Rank.ToString().Contains(rankFilterTextBox.Text)
                && (item.Profession?.Code ?? "").ToLower().Contains(professionCodeFilterTextBox.Text.ToLower())
                && (item.Profession?.Name ?? "").ToLower().Contains(professionNameFilterTextBox.Text.ToLower())
                && (item.TechProcessType?.Code ?? "").ToLower().Contains(codeOfOperationTypeFilterTextBox.Text.ToLower())
                && (item.CodifierCode ?? "").ToLower().Contains(codifierCodeFilterTextBox.Text.ToLower())
                && (item.CodifierName ?? "").ToLower().Contains(codifierNameFilterTextBox.Text.ToLower())
                && (item.CodifierGroupCode ?? "").ToLower().Contains(codifierGroupCodeFilterTextBox.Text.ToLower())
                && (item.CodifierGroupName ?? "").ToLower().Contains(codifierGroupNameFilterTextBox.Text.ToLower())
                && (item.Description2 ?? "").ToLower().Contains(description2FilterTextBox.Text.ToLower())
                && (item.TechProOperationName ?? "").ToLower().Contains(techProOperationNameFilterTextBox.Text.ToLower())
                && (item.MaterialName ?? "").ToLower().Contains(materialFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new OperationWindow(null).ShowDialog();
            Refresh();
        }

        void Open()
        {
            var item = (Operation)itemsDataGrid.SelectedItem;
            if (item == null) return;

            new OperationWindow(item).ShowDialog();
            Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) => Open();
        private void ItemsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => Open();

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.Cast<Operation>();
            if (items.Count() == 0) return;

            var dialog = MessageBox.Show("Удалить выбранные записи?", "Внимание", MessageBoxButton.YesNo);
            if (dialog != MessageBoxResult.Yes) return;

            if (items.Any(x => _operationRepo.IsOperationAlreadyUsedInSomeProducts(x)))
            {
                MessageBox.Show("Одна из удаляемых операций уже используется в каком-то продукте");
                return;
            }

            items.ForEach(x => _operationRepo.Remove(x));

            Refresh();
        }

        private void duplicateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsDataGrid.SelectedItem as Operation;
            if (item == null) return;

            var clone = Mapper.DeepClone(item);

            _operationRepo.Add(clone);

            Refresh();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void DepartmentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void CodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void DescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void UnitFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void GroupFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void rankFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void professionCodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void professionNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codeOfOperationTypeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codifierCodeTypeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codifierNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codifierGroupNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void description2FilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void techProOperationNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codifierCodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void codifierGroupCodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void materialFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();

        private void getProductsInWhichOperationIsUsedButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsDataGrid.SelectedItem as Operation;
            if (item == null) return;

            var text = string.Join("\n", _operationRepo.GetProductsInWhichOperationIsUsed(item));
            Clipboard.SetDataObject(text);
            MessageBox.Show("Список скопирован в буфер обмена");

        }
    }
}
