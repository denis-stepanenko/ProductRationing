using AgileObjects.AgileMapper;
using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Dto;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ProductRationing.Reports.Pages
{
    public partial class OperationsAndBigOperationsWithPZVPage : Page
    {
        private Product _product;

        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public OperationsAndBigOperationsWithPZVPage()
        {
            InitializeComponent();
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_product == null) return;

            var items = _productOperationRepo.GetAllByProductCode(_product.Code);
            var itemsDto = Mapper.Map(items).ToANew<IEnumerable<ProductOperationDto>>();

            var parameters = new ReportParameterCollection
            {
                new ReportParameter("Code", _product.Code),
                new ReportParameter("Name", _product.Name)
            };

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.OperationsAndBigOperationsWithPZV.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", itemsDto));
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.RefreshReport();
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
    }
}
