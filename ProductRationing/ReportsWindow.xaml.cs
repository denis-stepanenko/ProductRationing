using System;
using System.Windows;

namespace ProductRationing
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
        }

        private void ProductsAndOperationsHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\ProductsAndOperationsPage.xaml");
        private void ProductsAndOperationGroupsHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\ProductsAndOperationGroupsPage.xaml");
        private void ProductsContainingOperationHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\ProductsContainingOperationPage.xaml");
        private void SumByProductsHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\SumByProductsPage.xaml");
        private void SumByGroupsHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\SumByGroupsPage.xaml");
        private void SumByDepartmentsHyperlink_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\SumByDepartmentsPage.xaml");
        private void ProductsAndOperationGroupsWithPZV_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\ProductsAndOperationGroupsWithPZVPage.xaml");
        private void ProductsAndBigOperationsWithPZV_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\ProductsAndBigOperationsWithPZVPage.xaml");
        private void OperationsAndBigOperationsWithPZV_Click(object sender, RoutedEventArgs e) => Navigate(@"Reports\Pages\OperationsAndBigOperationsWithPZVPage.xaml");

        void Navigate(string uri)
        {
            var reportWindow = new ReportWindow();
            reportWindow.mainFrame.Source = new Uri(uri, UriKind.Relative);
            reportWindow.Show();
        }

    }
}
