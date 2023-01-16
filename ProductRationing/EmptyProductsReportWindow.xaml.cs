using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using System.Windows;

namespace ProductRationing
{
    public partial class EmptyProductsReportWindow : Window
    {
        private readonly ProductOperationRepo _repo = new ProductOperationRepo();

        public EmptyProductsReportWindow(string code, int? department)
        {
            InitializeComponent();

            var items = _repo.GetEmptyProducts(code, department);

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.EmptyProducts.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", items));
            reportViewer.RefreshReport();
        }
    }
}
