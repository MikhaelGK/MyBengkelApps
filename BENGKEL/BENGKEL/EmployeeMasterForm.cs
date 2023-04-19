using BENGKEL.Helper;
using BENGKEL.Viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BENGKEL
{
    public partial class EmployeeMasterForm : Form
    {
        List<ViewModelEmployee> lData = new List<ViewModelEmployee>();
        List<ViewModelEmployee> cData = new List<ViewModelEmployee>(); 

        string title = "Employee Master";
        string editId = "";
        Operation operation = Operation.None;

        public EmployeeMasterForm()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            var strgs = new List<string>()
            {
                tbName.Text,
                tbPos.Text,
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
            tbPos.Clear();
        }

        private void LoadDatabase()
        {
            lData.Clear();
            var context = new db();
            var employees = context.Employees.ToList();
            foreach (var e in employees)
            {
                var eData = new ViewModelEmployee()
                {
                    ID = e.EmployeeId,
                    Name = e.Name,
                    Position = e.Position,
                    Email = e.Email,
                    Password = e.Password,
                    Address = e.Address,
                    Phone = e.Phone,
                };
                lData.Add(eData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colEId = x.ID,
                colEName = x.Name,
                colEPos = x.Position,
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
                    colEId = x.ID,
                    colEName = x.Name,
                    colEPos = x.Position,
                    colEmi = x.Email,
                    colAds = x.Address,
                    colPho = x.Phone
                }).ToList();
                return;
            }

            dgv.DataSource = cData.Select(x => new
            {
                colEId = x.ID,
                colEName = x.Name,
                colPos = x.Position,
                colEmi = x.Email,
                colAds = x.Address,
                colPho = x.Phone
            }).ToList();
        }

        private void EmployeeMasterForm_Load(object sender, EventArgs e)
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

            var id = "";
            var context = new db();
            while (true)
            {
                id = Assistance.GenerateID(5, Master.Employee.ToString());
                var validId = context.Employees.FirstOrDefault(x => x.EmployeeId == id) == null ? true : false;
                if (validId) break;
            }

            var employee = new ViewModelEmployee()
            {
                ID = id,
                Name = tbName.Text,
                Position = tbPos.Text,
                Email = tbEmi.Text,
                Password = tbPwd.Text,
                Address = tbAds.Text,
                Phone = tbPho.Text
            };
            cData.Add(employee);
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
            var employee = context.Employees
                .FirstOrDefault(x => x.EmployeeId == id);
            tbName.Text = employee.Name;
            tbPos.Text = employee.Position;
            tbEmi.Text = employee.Email;
            tbPwd.Text = employee.Password;
            tbAds.Text = employee.Address;
            tbPho.Text = employee.Phone;
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
                var employee = cData.FirstOrDefault(x => x.ID == id);
                cData.Remove(employee);
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
                        var eData = new Employee()
                        {
                            EmployeeId = c.ID,
                            Name = c.Name,
                            Position = c.Position,
                            Email = c.Email,
                            Password = c.Password,
                            Address = c.Address,
                            Phone = c.Phone
                        };
                        context.Employees.Add(eData);
                        context.SaveChanges();
                    }
                    cData.Clear();
                    break;
                case Operation.Update:
                    var employee = context.Employees
                        .FirstOrDefault(x => x.EmployeeId == editId);
                    employee.Name = tbName.Text;
                    employee.Position = tbPos.Text;
                    employee.Email = tbEmi.Text;
                    employee.Password = tbPwd.Text;
                    employee.Address = tbAds.Text;
                    employee.Phone = tbPho.Text;
                    context.SaveChanges();
                    LoadDatabase();
                    break;
                case Operation.Delete:
                    foreach (var i in cData)
                    {
                        var rData = context.Employees
                            .FirstOrDefault(x => x.EmployeeId == i.ID);
                        context.Employees.Remove(rData);
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
            var employees = context.Employees
                .Where(x => x.EmployeeId.Contains(tbSearch.Text) || x.Name.Contains(tbSearch.Text))
                .ToList();
            foreach (var employee in employees)
            {
                var eData = new ViewModelEmployee()
                {
                    ID = employee.EmployeeId,
                    Name = employee.Name,
                    Position = employee.Position,
                    Email = employee.Email,
                    Password = employee.Password,
                    Address = employee.Address,
                    Phone = employee.Phone,
                };
                lData.Add(eData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colEId = x.ID,
                colEName = x.Name,
                colEPos = x.Position,
                colEmi = x.Email,
                colAds = x.Address,
                colPho = x.Phone
            }).ToList();
        }
    }
}
