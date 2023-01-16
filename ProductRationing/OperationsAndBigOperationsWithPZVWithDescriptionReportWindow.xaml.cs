using AgileObjects.AgileMapper;
using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Dto;
using System.Collections.Generic;
using System.Windows;

namespace ProductRationing
{
    public partial class OperationsAndBigOperationsWithPZVWithDescriptionReportWindow : Window
    {
        private readonly ProductOperationRepo _repo = new ProductOperationRepo();

        public OperationsAndBigOperationsWithPZVWithDescriptionReportWindow(Product product)
        {
            InitializeComponent();

            var items = _repo.GetAllByProductCode(product.Code);
            var itemsDto = Mapper.Map(items).ToANew<IEnumerable<ProductOperationDto>>();

            var parameters = new ReportParameterCollection
            {
                new ReportParameter("Code", product.Code),
                new ReportParameter("Name", product.Name)
            };

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.OperationsAndBigOperationsWithPZVWithDescription.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", itemsDto));
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.RefreshReport();
        }
    }
}
