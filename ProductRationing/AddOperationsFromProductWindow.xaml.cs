using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Validators;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class AddOperationsFromProductWindow : Window
    {
        private Product _product;
        private string _productCode;
        private CollectionView _itemsView;

        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public AddOperationsFromProductWindow(string productCode)
        {
            InitializeComponent();
            _productCode = productCode;
        }

        void Refresh()
        {
            if (_product == null) return;

            var items = _productOperationRepo.GetAllByProductCode(_product.Code);

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as ProductOperation;
                return
                item.Operation.Department.ToString().Contains(departmentFilterTextBox.Text)
                && item.Operation.Code.ToLower().Contains(codeFilterTextBox.Text.ToLower())
                && item.Operation.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower())
                && (item.Operation.Description ?? "").ToLower().Contains(descriptionFilterTextBox.Text.ToLower())
                && item.Operation.Unit.Name.ToLower().Contains(unitFilterTextBox.Text.ToLower())
                && item.Operation.Group.Name.ToLower().Contains(groupFilterTextBox.Text.ToLower());
            };
            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.Cast<ProductOperation>();
            if (items.Count() == 0) return;

            foreach (var item in items)
            {
                var newProductOperation = new ProductOperation
                {
                    ProductCode = _productCode,
                    OperationId = item.OperationId,
                    Count = item.Count
                };

                var results = new ProductOperationValidator().Validate(newProductOperation);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _productOperationRepo.Add(newProductOperation);
            }

            Close();
        }

        private void ProductSelectControl_Click()
        {
            var selectProductWindow = new SelectProductWindow();

            if (selectProductWindow.ShowDialog() == true)
            {
                _product = selectProductWindow.Product;
                productSelectControl.Text = $"{_product.Code}, {_product.Name}";
            }
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
        private void DepartmentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
        private void CodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
        private void DescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
        private void UnitFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
        private void GroupFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView?.Refresh();
    }
}