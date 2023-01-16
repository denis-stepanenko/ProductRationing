using ProductRationing.DAL.Models;
using System.Windows;

namespace ProductRationing
{
    public partial class OldOperationWindow : Window
    {
        private Operation _operation;
        private Unit _unit;
        private Group _group;
        private BigOperation _bigOperation;

        public OldOperationWindow(Operation operation)
        {
            InitializeComponent();
            _operation = operation;

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
                _bigOperation = operation.BigOperation;
            }
        }

        private void UnitSelectControl_Click()
        {
            var selectUnitWindow = new SelectUnitWindow();
            if(selectUnitWindow.ShowDialog() == true)
            {
                _unit = selectUnitWindow.Unit;
                unitSelectControl.Text = _unit?.Name;
            }
        }

        private void GroupSelectControl_Click()
        {
            var selectGroupWindow = new SelectGroupWindow();
            if(selectGroupWindow.ShowDialog() == true)
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
