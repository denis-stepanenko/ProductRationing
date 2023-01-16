using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using System.Windows;

namespace ProductRationing
{
    public partial class BigOperationsByProductReportWindow : Window
    {
        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public BigOperationsByProductReportWindow(Product product)
        {
            InitializeComponent();

            var items = _productOperationRepo.GetAllforProductEntriesWithoutEmptyProducts(product.Code);

            var parameters = new ReportParameterCollection
                {
                    new ReportParameter("Code", product.Code),
                    new ReportParameter("Name", product.Name)
                };

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.BigOperationsByProduct.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", items));
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.RefreshReport();
        }
    }
}
