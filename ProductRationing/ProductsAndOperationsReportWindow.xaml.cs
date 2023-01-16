using AgileObjects.AgileMapper;
using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Dto;
using System.Collections.Generic;
using System.Windows;

namespace ProductRationing
{
    public partial class ProductsAndOperationsReportWindow : Window
    {
        private readonly ProductOperationRepo _repo = new ProductOperationRepo();

        public ProductsAndOperationsReportWindow(Product product)
        {
            InitializeComponent();

            var items = _repo.GetAllForProductEntriesWithOnlyProductionDepartments(product.Code);
            var itemsDto = Mapper.Map(items).ToANew<IEnumerable<ProductEntryOperationDto>>();

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.ProductsAndOperations.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", itemsDto));
            reportViewer.RefreshReport();
        }
    }
}
