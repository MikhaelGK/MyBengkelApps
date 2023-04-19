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
using System.Xml.Linq;

namespace BENGKEL
{
    public partial class CustomerVehicleMasterForm : Form
    {
        List<ViewModelCustomerVehicle> lData = new List<ViewModelCustomerVehicle>();
        List<ViewModelCustomerVehicle> cData = new List<ViewModelCustomerVehicle>();

        int editId;
        string title = "Customer Vechicle Master";
        Operation operation = Operation.None;

        public CustomerVehicleMasterForm()
        {
            InitializeComponent();
        }

        private void KeyDownF3(int searchCode, KeyEventArgs e)
        {
            var context = new db();
            if (e.KeyCode == Keys.F3)
            {
                var form = new ChooseForm(searchCode, null);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var id = form.GetID();
                    if (searchCode == 1)
                    {
                        var customer = context.Customers
                            .FirstOrDefault(x => x.CustomerId == id);
                        tbCId.Text = customer.CustomerId;
                        tbCName.Text = customer.Name;
                        tbCAds.Text = customer.Address;
                        tbCPho.Text = customer.Phone;
                    }
                    if (searchCode == 2)
                    {
                        var vehicle = context.Vehicles
                            .FirstOrDefault(x => x.VehicleId == id);
                        tbVId.Text = vehicle.VehicleId;
                        tbVName.Text = vehicle.Name;
                    }
                }
            }
        }

        private bool Validate()
        {
            var strgs = new List<string>()
            {
                tbCId.Text,
                tbVId.Text,
                tbVNumber.Text
            };
            var valid = Validation.StringIsEmpty(strgs);
            if (!valid)
            {
                Mbox.Warning(title, "All input must be filled");
                return false;
            }
            return true;
        }

        private void ClearUI()
        {
            tbCId.Clear();
            tbCName.Clear();
            tbCAds.Clear();
            tbCPho.Clear();
            tbVId.Clear();
            tbVName.Clear();
            tbVNumber.Clear();
        }

        private void LoadDatabase()
        {
            lData.Clear();
            var context = new db();
            var customerVehicle = context.CustomerVehicles.ToList();
            foreach (var cv in customerVehicle)
            {
                var cvData = new ViewModelCustomerVehicle()
                {
                    ID = cv.CustomerVehicleId,
                    CustomerID= cv.CustomerId,
                    CustomerName = cv.Customer.Name,
                    VehicleID= cv.VehicleId,
                    VehicleName = cv.Vehicle.Name,
                    VehicleNumber = cv.Number,
                };
                lData.Add(cvData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colCVId = x.ID,
                colCName = x.CustomerName,
                colVName = x.VehicleName,
                colVNumber = x.VehicleNumber,
            }).ToList();
        }

        private void LoadHoldData()
        {
            if (operation == Operation.Delete)
            {
                dgv.DataSource = lData.Select(x => new
                {
                    colCVId = x.ID,
                    colCName = x.CustomerName,
                    colVName = x.VehicleName,
                    colVNumber = x.VehicleNumber,
                }).ToList();
                return;
            }

            var items = cData.ToList();
            cData.Clear();
            foreach (var i in items)
            {
                var vcData = new ViewModelCustomerVehicle()
                {
                    ID = cData.Count + 1,
                    CustomerID = i.CustomerID,
                    CustomerName  = i .CustomerName,
                    VehicleID = i.VehicleID,
                    VehicleName = i.VehicleName,
                    VehicleNumber = i.VehicleNumber
                };
                cData.Add(vcData);
            }

            dgv.DataSource = cData.Select(x => new
            {
                colCVId = x.ID,
                colCName = x.CustomerName,
                colVName = x.VehicleName,
                colVNumber = x.VehicleNumber,
            }).ToList();
        }

        private void CustomerVehicleMasterForm_Load(object sender, EventArgs e)
        {
            LoadDatabase();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var valid = Validate();
            if (!valid) return;

            if (operation == Operation.Delete || operation == Operation.Update)
            {
                Mbox.Error(title, "Operation cannot be executed");
                return;
            }

            operation = Operation.Add;

            var customerVehicle = new ViewModelCustomerVehicle()
            {
                CustomerID = tbCId.Text,
                CustomerName = tbCName.Text,
                VehicleID = tbVId.Text,
                VehicleName = tbVName.Text,
                VehicleNumber = tbVNumber.Text
            };
            cData.Add(customerVehicle);
            LoadHoldData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selectedRow = dgv.CurrentRow;
            if (selectedRow is null)
            {
                Mbox.Error(title, "There is no selected row");
                return;
            }

            if (operation == Operation.Add || operation == Operation.Delete)
            {
                Mbox.Error(title, "Operation cannot be executed");
                return;
            }

            operation = Operation.Update;
            var context = new db();
            var id = Convert.ToInt32(selectedRow.Cells[0].Value);
            var customerVehicle = context.CustomerVehicles
                .FirstOrDefault(x => x.CustomerVehicleId == id);
            tbCId.Text = customerVehicle.CustomerId;
            tbCName.Text = customerVehicle.Customer.Name;
            tbCAds.Text = customerVehicle.Customer.Address;
            tbCPho.Text = customerVehicle.Customer.Phone;
            tbVId.Text = customerVehicle.VehicleId;
            tbVName.Text = customerVehicle.Vehicle.Name;
            tbVNumber.Text = customerVehicle.Number;
            editId = id;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedRow = dgv.CurrentRow;
            if (selectedRow is null)
            {
                Mbox.Error(title, "There is no selected row");
                return;
            }

            if (operation == Operation.Update)
            {
                Mbox.Error(title, "Operation cannot be executed");
                return;
            }

            var id = (int)selectedRow.Cells[0].Value;
            if (operation == Operation.Add)
            {
                var customerVehicle = cData.FirstOrDefault(x => x.ID == id);
                cData.Remove(customerVehicle);
                if (cData.Count == 0) LoadDatabase();
                else LoadHoldData();
                return;
            }

            var eData = lData
                .FirstOrDefault(x => x.ID == id);
            cData.Add(eData);
            lData.Remove(eData);
            operation = Operation.Delete;
            LoadHoldData();
            ClearUI();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var context = new db();
            switch (operation)
            {
                case Operation.None:
                    break;
                case Operation.Add:
                    foreach (var cv in cData)
                    {
                        var cvData = new CustomerVehicle()
                        {
                            CustomerId = cv.CustomerID,
                            VehicleId = cv.VehicleID,
                            Number = cv.VehicleNumber
                        };
                        context.CustomerVehicles.Add(cvData);
                        context.SaveChanges();
                    }
                    cData.Clear();
                    break;
                case Operation.Update:
                    var customerVehicle = context.CustomerVehicles
                        .FirstOrDefault(x => x.CustomerVehicleId == editId);
                    customerVehicle.CustomerId = tbCId.Text;
                    customerVehicle.VehicleId = tbVId.Text;
                    customerVehicle.Number = tbVNumber.Text;
                    context.SaveChanges();
                    LoadDatabase();
                    break;
                case Operation.Delete:
                    foreach (var i in cData)
                    {
                        var rData = context.CustomerVehicles
                            .FirstOrDefault(x => x.CustomerVehicleId == i.ID);
                        context.CustomerVehicles.Remove(rData);
                        context.SaveChanges();
                    }
                    break;
                default:
                    break;
            }

            operation = Operation.None;
            cData.Clear();
            LoadDatabase();
            ClearUI();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (operation)
            {
                case Operation.None:
                    break;
                case Operation.Add:
                    cData.Clear();
                    LoadDatabase();
                    break;
                case Operation.Update:
                    ClearUI();
                    LoadDatabase();
                    break;
                case Operation.Delete:
                    LoadDatabase();
                    break;
                default:
                    break;
            }

            operation = Operation.None;
            cData.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lData.Clear();
            var context = new db();
            var customerVehicle = context.CustomerVehicles.AsEnumerable()
                .Where(x => x.CustomerVehicleId.ToString().Contains(tbSearch.Text) || 
                x.Customer.Name.Contains(tbSearch.Text) || 
                x.Vehicle.Name.Contains(tbSearch.Text))
                .ToList();
            foreach (var cv in customerVehicle)
            {
                var cvData = new ViewModelCustomerVehicle()
                {
                    ID = cv.CustomerVehicleId,
                    CustomerID = cv.CustomerId,
                    CustomerName = cv.Customer.Name,
                    VehicleID = cv.VehicleId,
                    VehicleName = cv.Vehicle.Name,
                    VehicleNumber = cv.Number,
                };
                lData.Add(cvData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colCVId = x.ID,
                colCName = x.CustomerName,
                colVName = x.VehicleName,
                colVNumber = x.VehicleNumber,
            }).ToList();
        }

        private void tbCId_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownF3(1, e);
        }

        private void tbVId_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownF3(2, e);
        }
    }
}
