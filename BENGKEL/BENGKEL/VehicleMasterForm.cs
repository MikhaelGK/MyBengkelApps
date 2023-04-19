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
    public partial class VehicleMasterForm : Form
    {
        List<ViewModelVehicle> lData = new List<ViewModelVehicle>();
        List<ViewModelVehicle> cData = new List<ViewModelVehicle>();

        string title = "Vehicle Master";
        string editId = "";
        Operation operation = Operation.None;

        public VehicleMasterForm()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            var strgs = new List<string>()
            {
                tbName.Text,
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
            tbName.Clear();
        }

        private void LoadDatabase()
        {
            lData.Clear();
            var context = new db();
            var vehicle = context.Vehicles.ToList();
            foreach (var e in vehicle)
            {
                var vData = new ViewModelVehicle()
                {
                    ID = e.VehicleId,
                    Name = e.Name
                };
                lData.Add(vData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colVId = x.ID,
                colVName = x.Name
            }).ToList();
        }

        private void LoadHoldData()
        {
            if (operation == Operation.Delete)
            {
                dgv.DataSource = lData.Select(x => new
                {
                    colVId = x.ID,
                    colVName = x.Name
                }).ToList();
                return;
            }

            dgv.DataSource = cData.Select(x => new
            {
                colVId = x.ID,
                colVName = x.Name
            }).ToList();
        }

        private void VehicleMasterForm_Load(object sender, EventArgs e)
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
                id = Assistance.GenerateID(5, Master.Vehicle.ToString());
                var validId = context.Vehicles.FirstOrDefault(x => x.VehicleId == id) == null ? true : false;
                if (validId) break;
            }

            var vehicle = new ViewModelVehicle()
            {
                ID = id,
                Name = tbName.Text
            };
            cData.Add(vehicle);
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
            var vehicle = context.Vehicles
                .FirstOrDefault(x => x.VehicleId == id);
            tbName.Text = vehicle.Name;
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
                var vehicle = cData.FirstOrDefault(x => x.ID == id);
                cData.Remove(vehicle);
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
                        var eData = new Vehicle()
                        {
                            VehicleId = c.ID,
                            Name = c.Name
                        };
                        context.Vehicles.Add(eData);
                        context.SaveChanges();
                    }
                    cData.Clear();
                    break;
                case Operation.Update:
                    var vehicle = context.Vehicles
                        .FirstOrDefault(x => x.VehicleId == editId);
                    vehicle.Name = tbName.Text;
                    context.SaveChanges();
                    LoadDatabase();
                    break;
                case Operation.Delete:
                    foreach (var i in cData)
                    {
                        var rData = context.Vehicles
                            .FirstOrDefault(x => x.VehicleId == i.ID);
                        context.Vehicles.Remove(rData);
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
            var vehicle = context.Vehicles
                .Where(x => x.VehicleId.Contains(tbSearch.Text) || x.Name.Contains(tbSearch.Text))
                .ToList();
            foreach (var employee in vehicle)
            {
                var eData = new ViewModelVehicle()
                {
                    ID = employee.VehicleId,
                    Name = employee.Name
                };
                lData.Add(eData);
            }

            dgv.DataSource = lData.Select(x => new
            {
                colVId = x.ID,
                colVName = x.Name
            }).ToList();
        }
    }
}
