using BENGKEL.Helper;
using BENGKEL.Viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BENGKEL
{
    public partial class LoginForm : Form
    {
        string title = "Login";
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var strgs = new List<string>()
            {
                tbEmi.Text,
                tbPwd.Text
            };
            var valid = Validation.StringIsEmpty(strgs);
            if (!valid)
            {
                Mbox.Warning(title, "All input must be filled");
                return;
            }

            var context = new db();
            var employee = context.Employees
                .FirstOrDefault(x => x.Email == tbEmi.Text);
            if (employee is null)
            {
                Mbox.Error(title, "User Not Found");
                return;
            }

            if (employee.Password != tbPwd.Text)
            {
                Mbox.Error(title, "Incorrect Password");
                return;
            }

            UserDto.UserId = employee.EmployeeId;
            var form = new MainForm();
            this.Hide();

            form.FormClosed += delegate
            {
                tbEmi.Clear();
                tbPwd.Clear();
                this.Show();
            };

            form.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
