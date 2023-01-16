using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Validators;
using System.Linq;
using System.Windows;

namespace ProductRationing
{
    public partial class ProductOperationWindow : Window
    {
        private ProductOperation _productOperation;

        private readonly ProductOperationRepo _repo = new ProductOperationRepo();

        public ProductOperationWindow(ProductOperation productOperation)
        {
            InitializeComponent();
            _productOperation = productOperation;

            departmentTextBlock.Text = productOperation.Operation.Department.ToString();
            codeTextBlock.Text = productOperation.Operation.Code;
            nameTextBlock.Text = productOperation.Operation.Name;
            laborTextBlock.Text = productOperation.Operation.Labor.ToString();
            descriptionTextBlock.Text = productOperation.Operation.Description;
            unitTextBlock.Text = productOperation.Operation.Unit.Name;
            groupTextBlock.Text = productOperation.Operation.Group.Name;
            bigOperationTextBlock.Text = productOperation.Operation.BigOperation?.Name;
            countNumericUpDown.Value = productOperation.Count;
            productOperationDescriptionTextBox.Text = productOperation.Description;
            difficultyGroupNumericUpDown.Value = productOperation.DifficultyGroup;
            machineTimeDecimalUpDown.Value = productOperation.MachineTime;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (difficultyGroupNumericUpDown.Value == null)
            {
                MessageBox.Show("Введите группу сложности");
                return;
            }

            if (machineTimeDecimalUpDown.Value == null)
            {
                MessageBox.Show("Введите машинное время");
                return;
            }

            _productOperation.Count = countNumericUpDown.Value ?? 0;
            _productOperation.Description = productOperationDescriptionTextBox.Text;
            _productOperation.DifficultyGroup = (int)difficultyGroupNumericUpDown.Value;
            _productOperation.MachineTime = (decimal)machineTimeDecimalUpDown.Value;

            var results = new ProductOperationValidator().Validate(_productOperation);
            string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

            if (!results.IsValid)
            {
                MessageBox.Show(errors);
                return;
            }

            _repo.Update(_productOperation);

            Close();
        }
    }
}