﻿using AgileObjects.AgileMapper;
using Microsoft.Reporting.WinForms;
using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Dto;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ProductRationing.Reports.Pages
{
    public partial class ProductsAndOperationGroupsWithPZVPage : Page
    {
        private Product _product;

        private readonly ProductOperationRepo _productOperationRepo = new ProductOperationRepo();

        public ProductsAndOperationGroupsWithPZVPage()
        {
            InitializeComponent();
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_product == null) return;

            var items = _productOperationRepo.GetAllForProductEntriesWithOnlyProductionDepartments(_product.Code);
            var itemsDto = Mapper.Map(items).ToANew<IEnumerable<ProductEntryOperationDto>>();

            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = "ProductRationing.Reports.ProductsAndOperationGroupsWithPZV.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Items", itemsDto));
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
