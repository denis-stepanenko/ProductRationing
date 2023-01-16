using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Models;
using ProductRationing.Validators;
using System;
using System.Linq;
using System.Windows;

namespace ProductRationing
{
    public partial class OperationWindow : Window
    {
        private Operation _operation;
        private Unit _unit;
        private Group _group;
        private BigOperation _bigOperation;

        private ProfessionRepo _professionRepo = new ProfessionRepo();
        private TechProcessTypeRepo _techProcessTypeRepo = new TechProcessTypeRepo();
        private readonly OperationRepo _operationRepo = new OperationRepo();

        public OperationWindow(Operation operation)
        {
            InitializeComponent();
            _operation = operation;

            var professions = _professionRepo.GetAll();
            professionsComboBox.ItemsSource = professions;

            var types = _techProcessTypeRepo.GetAll();
            typesComboBox.ItemsSource = types;

            if (operation != null)
            {
                departmentComboBox.Text = operation.Department.ToString();
                codeTextBox.Text = operation.Code;
                nameTextBox.Text = operation.Name;
                laborDecimalUpDown.Value = operation.Labor;
                descriptionTextBox.Text = operation.Description;
                unitSelectControl.Text = operation.Unit.Name;
                _unit = operation.Unit;
                groupSelectControl.Text = operation.Group.Name;
                _group = operation.Group;
                bigOperationSelectControl.Text = operation.BigOperation?.Name;
                materialNameTextBox.Text = operation.MaterialName;
                _bigOperation = operation.BigOperation;
                rankNumericUpDown.Value = operation.Rank;

                professionsComboBox.SelectedItem = professions.FirstOrDefault(x => x.Id == operation.ProfessionId);
                typesComboBox.SelectedItem = types.FirstOrDefault(x => x.Id == operation.TechProcessTypeId);

                codifierCodeTextBox.Text = operation.CodifierCode;
                codifierNameTextBox.Text = operation.CodifierName;
                codifierGroupCodeTextBox.Text = operation.CodifierGroupCode;
                codifierGroupNameTextBox.Text = operation.CodifierGroupName;
                description2TextBox.Text = operation.Description2;
                techProOperationNameTextBox.Text = operation.TechProOperationName;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_operation == null)
            {
                var newOperation = new Operation
                {
                    Department = Convert.ToInt32(departmentComboBox.Text),
                    Name = nameTextBox.Text,
                    Labor = laborDecimalUpDown.Value ?? 0,
                    Description = descriptionTextBox.Text,
                    UnitId = _unit?.Id ?? 0,
                    GroupId = _group?.Id ?? 0,
                    BigOperationId = _bigOperation?.Id ?? 0,
                    MaterialName = materialNameTextBox.Text,
                    Rank = (int)rankNumericUpDown.Value,

                    ProfessionId = (professionsComboBox.SelectedItem as Profession)?.Id,
                    TechProcessTypeId = (typesComboBox.SelectedItem as TechProcessType)?.Id,

                    CodifierCode = codifierCodeTextBox.Text,
                    CodifierName = codifierNameTextBox.Text,
                    CodifierGroupCode = codifierGroupCodeTextBox.Text,
                    CodifierGroupName = codifierGroupNameTextBox.Text,
                    Description2 = description2TextBox.Text,
                    TechProOperationName = techProOperationNameTextBox.Text,
                };

                var results = new OperationValidator().Validate(newOperation);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _operationRepo.Add(newOperation);
            }
            else
            {
                _operation.Department = Convert.ToInt32(departmentComboBox.Text);
                _operation.Name = nameTextBox.Text;
                _operation.Labor = laborDecimalUpDown.Value ?? 0;
                _operation.Description = descriptionTextBox.Text;
                _operation.UnitId = _unit?.Id ?? 0;
                _operation.GroupId = _group?.Id ?? 0;
                _operation.BigOperationId = _bigOperation?.Id ?? 0;
                _operation.MaterialName = materialNameTextBox.Text;
                _operation.Rank = (int)rankNumericUpDown.Value;

                _operation.ProfessionId = (professionsComboBox.SelectedItem as Profession)?.Id;
                _operation.TechProcessTypeId = (typesComboBox.SelectedItem as TechProcessType)?.Id;

                _operation.CodifierCode = codifierCodeTextBox.Text;
                _operation.CodifierName = codifierNameTextBox.Text;
                _operation.CodifierGroupCode = codifierGroupCodeTextBox.Text;
                _operation.CodifierGroupName = codifierGroupNameTextBox.Text;
                _operation.Description2 = description2TextBox.Text;
                _operation.TechProOperationName = techProOperationNameTextBox.Text;

                var results = new OperationValidator().Validate(_operation);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _operationRepo.Update(_operation);
            }

            Close();
        }

        private void UnitSelectControl_Click()
        {
            var selectUnitWindow = new SelectUnitWindow();
            if (selectUnitWindow.ShowDialog() == true)
            {
                _unit = selectUnitWindow.Unit;
                unitSelectControl.Text = _unit?.Name;
            }
        }

        private void GroupSelectControl_Click()
        {
            var selectGroupWindow = new SelectGroupWindow();
            if (selectGroupWindow.ShowDialog() == true)
            {
                _group = selectGroupWindow.Group;
                groupSelectControl.Text = _group?.Name;
            }
        }

        private void UnitSelectControl_Clear()
        {
            _unit = null;
            unitSelectControl.Text = "";
        }

        private void GroupSelectControl_Clear()
        {
            _group = null;
            groupSelectControl.Text = "";
        }

        private void bigOperationSelectControl_Click()
        {
            var selectBigOperationWindow = new SelectBigOperationWindow();
            if (selectBigOperationWindow.ShowDialog() == true)
            {
                _bigOperation = selectBigOperationWindow.BigOperation;
                bigOperationSelectControl.Text = _bigOperation?.Name;
            }
        }

        private void bigOperationSelectControl_Clear()
        {
            _bigOperation = null;
            bigOperationSelectControl.Text = "";
        }
    }
}
