using BENGKEL.Helper;
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
    enum Choose { Customer, Vehicle, Employee, CustomerVehicle }
    public partial class ChooseForm : Form
    {
        private readonly Choose _choose;
        private string id;
        private string title = "Choose Form";
        private string CId;

        public ChooseForm(int search, string CId)
        {
            InitializeComponent();
            this.CId = CId;
            switch (search)
            {
                case 1:
                    _choose = Choose.Customer;
                    break;
                case 2:
                    _choose = Choose.Vehicle;
                    break;
                case 3:
                    _choose = Choose.Employee;
                    break;
                case 4:
                    _choose = Choose.CustomerVehicle;
                    break;
                default:
                    break;
            }
        }

        public string GetID()
        {
            return id;
        }

        private void ChooseForm_Load(object sender, EventArgs e)
        {
            var context = new db();
            switch (_choose)
            {
                case Choose.Customer:
                    var customer = context.Customers
                    .ToList();
                    dgv.DataSource = customer.Select(x => new
                    {
                        x.CustomerId,
                        x.Name,
                        x.Address,
                        x.Phone
                    }).ToList();
                    break;
                case Choose.Vehicle:
                    var vehicle = context.Vehicles.ToList();
                    dgv.DataSource = vehicle.Select(x => new
                    {
                        x.VehicleId,
                        x.Name
                    }).ToList();
                    break;
                case Choose.Employee:
                    var employee = context.Employees.ToList();
                    dgv.DataSource = employee.Select(x => new
                    {
                        x.EmployeeId,
                        x.Name,
                        x.Position,
                        x.Phone
                    }).ToList();
                    break;
                case Choose.CustomerVehicle:
                    var customerVehicle = context.CustomerVehicles
                        .Where(x => x.CustomerId == CId).ToList();
                    dgv.DataSource = customerVehicle.Select(x => new
                    {
                        x.CustomerVehicleId,
                        x.Vehicle.Name,
                        x.Number
                    }).ToList();
                    break;
                default:
                    break;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var selected = dgv.CurrentRow;
            if (selected is null)
            {
                Mbox.Warning(title, "There is no selected row");
                return;
            }

            id = selected.Cells[0].Value.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var context = new db();
            switch (_choose)
            {
                case Choose.Customer:
                    var customer = context.Customers
                    .Where(x => x.CustomerId.Contains(tbSearch.Text) ||
                        x.Name.Contains(tbSearch.Text))
                    .ToList();
                    dgv.DataSource = customer.Select(x => new
                    {
                        x.CustomerId,
                        x.Name,
                        x.Address,
                        x.Phone
                    }).ToList();
                    break;
                case Choose.Vehicle:
                    var vehicle = context.Vehicles
                    .Where(x => x.VehicleId.Contains(tbSearch.Text) ||
                        x.Name.Contains(tbSearch.Text))
                    .ToList();
                    dgv.DataSource = vehicle.Select(x => new
                    {
                        x.VehicleId,
                        x.Name
                    }).ToList();
                    break;
                case Choose.Employee:
                    var employee = context.Employees
                    .Where(x => x.EmployeeId.Contains(tbSearch.Text) ||
                    x.Name.Contains(tbSearch.Text))
                    .ToList();
                    dgv.DataSource = employee.Select(x => new
                    {
                        x.EmployeeId,
                        x.Name,
                        x.Position,
                        x.Phone
                    }).ToList();
                    break;
                case Choose.CustomerVehicle:
                    var customerVehicle = context.CustomerVehicles
                        .Where(x => x.CustomerId == CId && x.Vehicle.Name.Contains(tbSearch.Text)).ToList();
                    dgv.DataSource = customerVehicle.Select(x => new
                    {
                        x.CustomerVehicleId,
                        x.Vehicle.Name,
                        x.Number
                    }).ToList();
                    break;
                default:
                    break;
            }
        }
    }
}
