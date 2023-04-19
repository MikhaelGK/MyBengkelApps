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
    public partial class CustomerMasterForm : Form
    {
        List<ViewModelCustomer> lData = new List<ViewModelCustomer>();
        List<ViewModelCustomer> cData = new List<ViewModelCustomer>();

        string title = "Customer Master";
        string editId = "";
        Operation operation = Operation.None;

        public CustomerMasterForm()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            var strgs = new List<string>()
            {
                tbName.Text,
                tbEmi.Text,
                tbPwd.Text,
                tbPho.Text,
                tbAds.Text
            };
            var valid = Validation.StringIsEmpty(strgs);
            if (!valid)
            {
                Mbox.Warning(title, "All input must be filled");
                return false;
            }

            var emailValid = Validation.StringIsEmail(tbEmi.Text);
            if (!emailValid)
            {
                Mbox.Warning(title, "Incorrect Email Format");
                return false;
            }

            var pwdValid = Validation.StringIsPassword(tbPwd.Text);
            if (!pwdValid)
            {
                Mbox.Warning(title, "Password must be between 8-12 and contains uppercase, lowercase, numeric");
                return false;
            }

            var phoValid = Validation.StringIsNumber(tbPho.Text);
            if (!phoValid)
            {
                Mbox.Warning(title, "Phone number must be between 10-12 and cannot contains any letter");
                return false;
            }

            return true;
        }

        private void ClearUI()
        {
            tbName.Clear();
            tbEmi.Clear();
            tbPwd.Clear();
            tbAds.Clear();
            tbPho.Clear();
        }

        private void LoadDatabase()
        {
            lData.Clear();
            var context = new db();
            var customers = context.Customers.ToList();
            foreach (var c in customers)
            {
                var cData = new ViewModelCustomer()
                {
                    ID = c.CustomerId,
                    Name = c.Name,
                    Email = c.Email,
                    Password = c.Password,
                    Address = c.Address,
                    Phone = c.Phone,
                };
                lData.Add(cData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colCId = x.ID,
                colCName = x.Name,
                colEmi = x.Email,
                colAds = x.Address,
                colPho = x.Phone
            }).ToList();
        }

        private void LoadHoldData()
        {
            if (operation == Operation.Delete)
            {
                dgv.DataSource = lData.Select(x => new
                {
                    colCId = x.ID,
                    colCName = x.Name,
                    colEmi = x.Email,
                    colAds = x.Address,
                    colPho = x.Phone
                }).ToList();
                return;
            }

            dgv.DataSource = cData.Select(x => new
            {
                colCId = x.ID,
                colCName = x.Name,
                colEmi = x.Email,
                colAds = x.Address,
                colPho = x.Phone
            }).ToList();
        }

        private void CustomerMasterForm_Load(object sender, EventArgs e)
        {
            LoadDatabase();
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.Red;
            
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

            var id = "";
            var context = new db();
            while (true)
            {
                id = Assistance.GenerateID(5, Master.Customer.ToString());
                var validId = context.Customers.FirstOrDefault(x => x.CustomerId == id) == null ? true : false;
                if (validId) break;
            }

            var customer = new ViewModelCustomer()
            {
                ID = id,
                Name = tbName.Text,
                Email = tbEmi.Text,
                Password = tbPwd.Text,
                Address = tbAds.Text,
                Phone = tbPho.Text
            };
            cData.Add(customer);
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
            var id = selectedRow.Cells[0].Value.ToString();
            var customer = context.Customers
                .FirstOrDefault(x => x.CustomerId == id);
            tbName.Text = customer.Name;
            tbEmi.Text = customer.Email;
            tbPwd.Text = customer.Password;
            tbAds.Text = customer.Address;
            tbPho.Text = customer.Phone;
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

            var id = selectedRow.Cells[0].Value.ToString();
            if (operation == Operation.Add)
            {
                var customer = cData.FirstOrDefault(x => x.ID == id);
                cData.Remove(customer);
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
                    foreach (var c in cData)
                    {
                        var cData = new Customer()
                        {
                            CustomerId = c.ID,
                            Name = c.Name,
                            Email = c.Email,
                            Password = c.Password,
                            Address = c.Address,
                            Phone = c.Phone
                        };
                        context.Customers.Add(cData);
                        context.SaveChanges();
                    }
                    cData.Clear();
                    break;
                case Operation.Update:
                    var customer = context.Customers
                        .FirstOrDefault(x => x.CustomerId == editId);
                    customer.Name = tbName.Text;
                    customer.Email = tbEmi.Text;
                    customer.Password = tbPwd.Text;
                    customer.Address = tbAds.Text;
                    customer.Phone = tbPho.Text;
                    context.SaveChanges();
                    LoadDatabase();
                    break;
                case Operation.Delete:
                    foreach (var i in cData)
                    {
                        var rData = context.Customers
                            .FirstOrDefault(x => x.CustomerId == i.ID);
                        context.Customers.Remove(rData);
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
            var customers = context.Customers
                .Where(x => x.CustomerId.Contains(tbSearch.Text) || x.Name.Contains(tbSearch.Text))
                .ToList();
            foreach (var customer in customers)
            {
                var eData = new ViewModelCustomer()
                {
                    ID = customer.CustomerId,
                    Name = customer.Name,
                    Email = customer.Email,
                    Password = customer.Password,
                    Address = customer.Address,
                    Phone = customer.Phone,
                };
                lData.Add(eData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colCId = x.ID,
                colCName = x.Name,
                colEmi = x.Email,
                colAds = x.Address,
                colPho = x.Phone
            }).ToList();
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgv.Rows[0].DefaultCellStyle.BackColor = Color.AliceBlue;
        }
    }
}
