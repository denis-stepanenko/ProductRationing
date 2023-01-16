using AgileObjects.AgileMapper;
using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProductRationing.Reports.Pages
{
    public partial class ProductsContainingOperationPage : Page
    {
        private Operation _operation;

        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public ProductsContainingOperationPage()
        {
            InitializeComponent();
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_operation == null) return;

            var items = _productOperationRepo.Find(_operation.Code);
            var itemsDto = Mapper.Map(items.Distinct()).ToANew<IEnumerable<ProductEntryOperationDto>>();

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.ProductsContainingOperation.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", itemsDto));
            reportViewer.RefreshReport();
        }

        private void ProductSelectControl_Click()
        {
            var selectOperationWindow = new SelectOperationWindow();
            if (selectOperationWindow.ShowDialog() == true)
            {
                _operation = selectOperationWindow.Operation;
                productSelectControl.Text = $"{_operation.Code}, {_operation.Name}";
            }
        }
    }
}
