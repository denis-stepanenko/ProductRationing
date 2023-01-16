using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Validators;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductRationing
{
    public partial class AddOperationsWindow : Window
    {
        private string _productCode;
        private string _department;
        private CollectionView _itemsView;

        private readonly OperationRepo _operationRepo = new OperationRepo();
        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public AddOperationsWindow(string productCode, string department)
        {
            InitializeComponent();
            _department = department;
            _productCode = productCode;
            Refresh();
        }

        public Unit Unit { get; set; }

        void Refresh()
        {
            var items = _operationRepo.GetAll();

            _itemsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _itemsView.Filter = (e) =>
            {
                var item = e as Operation;
                return
                item.Department.ToString().Contains(_department.ToString())
                && item.Code.ToLower().Contains(codeFilterTextBox.Text.ToLower())
                && item.Name.ToLower().Contains(nameFilterTextBox.Text.ToLower())
                && (item.Description ?? "").ToLower().Contains(descriptionFilterTextBox.Text.ToLower())
                && item.Unit.Name.ToLower().Contains(unitFilterTextBox.Text.ToLower())
                && item.Group.Name.ToLower().Contains(groupFilterTextBox.Text.ToLower());
            };

            itemsDataGrid.ItemsSource = _itemsView;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var items = itemsDataGrid.SelectedItems.Cast<Operation>();
            if (items.Count() == 0) return;

            foreach (var item in items)
            {
                var newProductOperation = new ProductOperation
                {
                    ProductCode = _productCode,
                    OperationId = item.Id,
                    Count = (double)countNumericUpDown.Value
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void DepartmentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void CodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void DescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void UnitFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
        private void GroupFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _itemsView.Refresh();
    }
}
