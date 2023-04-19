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
    enum Operation { None, Add, Update, Delete }
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void vehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new VehicleMasterForm()
            {
                MdiParent = ActiveForm
            };
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new EmployeeMasterForm()
            {
                MdiParent = ActiveForm
            };
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CustomerMasterForm()
            {
                MdiParent = ActiveForm
            };
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TransactionForm()
            {
                MdiParent = ActiveForm
            };
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void customerVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CustomerVehicleMasterForm()
            {
                MdiParent = ActiveForm
            };
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }
    }
}
