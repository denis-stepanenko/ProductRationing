using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductRationing
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();

        private CollectionView _productRelationsView;
        private List<ProductOperation> _operations;
        private CollectionView _operationsView;

        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();
        private readonly ProductRepo _productRepo = new ProductRepo();

        public MainWindow()
        {
            InitializeComponent();

            if (!new[]
            {
                "userasup20",
                "Shkir_IV",
                "Podgaevskaya_EA",
                "KhvorostinskayaKV",
                "Priemkina_VA",
                "Molchanova_OA",
                "StorchakTS",
                "ShevchenkoIM",
                "KatranovaOI",

                "UserOgt48",
                "UserOgt73",
                "UserOgt57",
                "UserOgt50",
                "userogt9",
                "UserOgt39",
                "UserOgt62",
                "UserOgt37",
                "UserOgt80",
                "UserOgt71",
                "Markizova_EYu",
                "userogt54",
                "brezhnevka"
            }.Contains(Environment.UserName, StringComparer.CurrentCultureIgnoreCase))
            {
                operationsMenuItem.IsEnabled = false;
                oldOperationsMenuItem.IsEnabled = false;
                unitsMenuItem.IsEnabled = false;
                groupsMenuItem.IsEnabled = false;
            }

            RefreshProducts();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                RefreshProducts();
                _timer.Stop();
            };
        }

        private void CodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void NameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        void RefreshProducts()
        {
            var items = _productRepo.FindProducts(codeFilterTextBox.Text, nameFilterTextBox.Text);
            itemsDataGrid.ItemsSource = items;
        }

        void RefreshProductRelations()
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            var items = _productRepo.GetProductEntries(product.Code);

            _productRelationsView = (CollectionView)CollectionViewSource.GetDefaultView(items);
            _productRelationsView.Filter = (x) =>
            {
                var item = x as ProductEntry;
                var department = departmentComboBox.SelectedValue?.ToString() ?? "";
                var departments = item.Route?.Split(' ');

                return
                item.Code.ToLower().Contains(productRelationCodeFilterTextBox.Text.ToLower())
                && (item.Name?.ToLower() ?? "").Contains(productRelationNameFilterTextBox.Text.ToLower())
                && (department == "" || (departments != null && departments.Contains(department)));
            };
            productRelationsDataGrid.ItemsSource = _productRelationsView;
        }

        void RefreshOperations()
        {
            var relation = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (relation == null) return;

            _operations = _productOperationRepo.GetAllByProductCode(relation.Code).ToList();

            laborTextBlock.Text = _operations.Where(x => x.Operation.Department.ToString().Contains(departmentComboBox.SelectedValue?.ToString() ?? ""))
                                       .Sum(x => x.Operation.Labor * (decimal)x.Count).ToString();

            _operationsView = (CollectionView)CollectionViewSource.GetDefaultView(_operations);
            _operationsView.Filter = (x) =>
            {
                var item = x as ProductOperation;
                return
                item.Operation.Department.ToString().Contains(departmentComboBox.SelectedValue?.ToString() ?? "")
                && item.Operation.Code.ToLower().Contains(operationCodeFilterTextBox.Text.ToLower())
                && item.Operation.Name.ToLower().Contains(operationNameFilterTextBox.Text.ToLower())
                && (item.Operation.Description ?? "").ToLower().Contains(operationDescriptionFilterTextBox.Text.ToLower())
                && item.Operation.Unit.Name.ToLower().Contains(operationUnitFilterTextBox.Text.ToLower())
                && item.Operation.Group.Name.ToLower().Contains(operationGroupFilterTextBox.Text.ToLower())
                && (item.Description ?? "").ToLower().Contains(operationProductOperationDescriptionFilterTextBox.Text.ToLower())
                && (item.Operation.BigOperation?.Name ?? "").ToLower().Contains(operationBigOperationFilterTextBox.Text.ToLower());
            };
            operationsDataGrid.ItemsSource = _operationsView;
        }

        void OpenOperation()
        {
            var item = operationsDataGrid.SelectedItem as ProductOperation;
            if (item == null) return;

            new ProductOperationWindow(item).ShowDialog();
            RefreshOperations();
        }

        private void operationAddButton_Click(object sender, RoutedEventArgs e)
        {
            var relation = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (relation == null) return;

            new AddOperationsWindow(relation.Code, departmentComboBox.Text).ShowDialog();
            RefreshOperations();
        }

        private void operationAddFromProductButton_Click(object sender, RoutedEventArgs e)
        {
            var relation = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (relation == null) return;

            new AddOperationsFromProductWindow(relation.Code).ShowDialog();
            RefreshOperations();
        }

        private void operationRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var items = operationsDataGrid.SelectedItems.Cast<ProductOperation>().ToList();
            if (items.Count() == 0) return;

            var dialog = MessageBox.Show("Удалить выбранные записи?", "Внимание", MessageBoxButton.YesNo);
            if (dialog != MessageBoxResult.Yes) return;

            items.ForEach(x => _productOperationRepo.Remove(x));

            RefreshOperations();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => RefreshProducts();

        private void itemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshProductRelations();
        private void productRelationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshOperations();

        private void operationEditButton_Click(object sender, RoutedEventArgs e) => OpenOperation();
        private void productRelationsRefreshButton_Click(object sender, RoutedEventArgs e) => RefreshProductRelations();

        private void productRelationCodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _productRelationsView?.Refresh();
        private void productRelationNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _productRelationsView?.Refresh();
        private void productRelationRouteFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _productRelationsView?.Refresh();
        private void productRelationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => _productRelationsView?.Refresh();

        private void operationDepartmentFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationCodeFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationNameFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationDescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationUnitFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationGroupFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationBigOperationFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();
        private void operationProductOperationDescriptionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => _operationsView?.Refresh();

        private void departmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _productRelationsView?.Refresh();
            _operationsView?.Refresh();
        }

        private void operationRefreshButton_Click(object sender, RoutedEventArgs e) => RefreshOperations();

        private void reportsMenuItem_Click(object sender, RoutedEventArgs e) => new ReportsWindow().Show();

        private void groupsMenuItem_Click(object sender, RoutedEventArgs e) => new GroupsWindow().Show();
        private void unitsMenuItem_Click(object sender, RoutedEventArgs e) => new UnitsWindow().Show();
        private void operationsMenuItem_Click(object sender, RoutedEventArgs e) => new OperationsWindow().Show();
        private void oldOperationsMenuItem_Click(object sender, RoutedEventArgs e) => new OldOperationsWindow().Show();

        private void operationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) => OpenOperation();

        private void operationsAndBigOperationsWithPZVPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            new OperationsAndBigOperationsWithPZVReportWindow(product).Show();
        }

        private void operationsAndBigOperationsWithPZVWithDescriptionPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            new OperationsAndBigOperationsWithPZVWithDescriptionReportWindow(product).Show();
        }

        private void productsAndOperationsPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            new ProductsAndOperationsReportWindow(product).Show();
        }

        private void operationsAndBigOperationsWithPZVForProductRelationPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (product == null) return;

            new OperationsAndBigOperationsWithPZVForProductRelationReportWindow(product).Show();
        }

        private void bigOperationsByProductPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            new BigOperationsByProductReportWindow(product).Show();
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            var items = operationsDataGrid.SelectedItems.OfType<ProductOperation>().ToList();
            if (items.Count() == 0) return;

            LocalClipboardService.Set(items);
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            var product = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (product == null) return;

            var items = LocalClipboardService.Get<ProductOperation>();

            items.ForEach(x => x.ProductCode = product.Code);

            items.ForEach(x => _productOperationRepo.Add(x));

            RefreshOperations();
        }

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = operationsDataGrid.SelectedItem as ProductOperation;
            if (item == null) return;

            var previousItem = _operations.ToArray().Reverse().SkipWhile(x => x != item).Skip(1).FirstOrDefault();
            if (previousItem == null) return;

            _productOperationRepo.Swap(item, previousItem);

            RefreshOperations();

            operationsDataGrid.SelectedIndex = _operations.FindIndex(x => x.Id == item.Id);
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = operationsDataGrid.SelectedItem as ProductOperation;
            if (item == null) return;

            var nextItem = _operations.SkipWhile(x => x != item).Skip(1).FirstOrDefault();
            if (nextItem == null) return;

            _productOperationRepo.Swap(item, nextItem);

            RefreshOperations();

            operationsDataGrid.SelectedIndex = _operations.FindIndex(x => x.Id == item.Id);
        }

        private void emptyProductsPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = itemsDataGrid.SelectedItem as Product;
            if (product == null) return;

            int? department = departmentComboBox.SelectedValue?.ToString() != null ? (int?)Convert.ToInt32(departmentComboBox.SelectedValue?.ToString()) : null;

            new EmptyProductsReportWindow(product.Code, department).Show();
        }

        private void operationsAndBigOperationsWithPZVForProductRelationForAssemblyDepartmentsPageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var product = productRelationsDataGrid.SelectedItem as ProductEntry;
            if (product == null) return;

            new OperationsAndBigOperationsWithPZVWithDescriptionForAssemblyDepartmentsReportWindow(product).Show();
        }
    }
}
