using ProductRationing.DAL.Data;
using ProductRationing.DAL.Models;
using ProductRationing.Validators;
using System.Linq;
using System.Windows;

namespace ProductRationing
{
    public partial class GroupWindow : Window
    {
        private Group _group;

        private readonly GroupRepo _repo = new GroupRepo();

        public GroupWindow(Group group)
        {
            InitializeComponent();
            _group = group;

            if (group != null)
            {
                nameTextBox.Text = group.Name;
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_group == null)
            {
                var newGroup = new Group
                {
                    Name = nameTextBox.Text
                };

                var results = new GroupValidator().Validate(newGroup);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _repo.Add(newGroup);
            }
            else
            {
                _group.Name = nameTextBox.Text;

                var results = new GroupValidator().Validate(_group);
                string errors = string.Join("\n", results.Errors.Select(x => x.ToString()));

                if (!results.IsValid)
                {
                    MessageBox.Show(errors);
                    return;
                }

                _repo.Update(_group);
            }

            Close();
        }
    }
}
