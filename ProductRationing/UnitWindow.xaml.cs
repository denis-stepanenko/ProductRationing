using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Validators;
using System.Linq;
using System.Windows;

namespace ProductRationing
{
    public partial class UnitWindow : Window
    {
        private Unit _unit;

        private readonly UnitRepo _repo = new UnitRepo();

        public UnitWindow(Unit unit)
        {
            InitializeComponent();
            _unit = unit;

            if (unit != null)
            {
                nameTextBox.Text = unit.Name;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_unit == null)
            {
                var newUnit = new Unit
                {
                    Name = nameTextBox.Text
                };

                var results = new UnitValidator().Validate(newUnit);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _repo.Add(newUnit);
            }
            else
            {
                _unit.Name = nameTextBox.Text;

                var results = new UnitValidator().Validate(_unit);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _repo.Update(_unit);
            }

            Close();
        }
    }
}
