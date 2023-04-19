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
    public partial class TransactionForm : Form
    {
        List<ViewModelTrx> lData = new List<ViewModelTrx>();

        string editId = "";
        string title = "Transaction Form";
        Operation operation = Operation.None;
        public TransactionForm()
        {
            InitializeComponent();
        }

        private void ClearUI()
        {
            tbCVId.Clear();
            tbVNumber.Clear();
            tbCost.Clear();
        }
        private void AddStruk(string title, string body)
        {
            flowLayoutPanel1.Controls.Add(new Label()
            {
                Text = title,
                Anchor = AnchorStyles.Left,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size()
                {
                    Width = 472,
                    Height = 20
                },
            });
            flowLayoutPanel1.Controls.Add(new Label()
            {
                Text = body.ToString(),
                Anchor = AnchorStyles.Right,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size()
                {
                    Width = 472,
                    Height = 20
                },
            });
        }
        private void LoadDatabase()
        {
            dgv.DataSource = lData.Select(x => new
            {
                colCName = x.CustomerName,
                colVName = x.VehicleName,
                colVNumber = x.VehicleNumber,
                colCost = x.Cost
            }).ToList();
        }
        private bool Validate()
        {
            var strgs = new List<string>()
            {
                tbCost.Text,
                tbCId.Text,
                tbCVId.Text,
            };
            var valid = Validation.StringIsEmpty(strgs);
            if (!valid)
            {
                Mbox.Warning(title, "All input must be filled");
                return false;
            }
            return true;
        }
        private void KeyDownF3(int searchCode, KeyEventArgs e, string CId)
        {
            var context = new db();
            if (e.KeyCode == Keys.F3)
            {
                var form = new ChooseForm(searchCode, CId);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var id = form.GetID();
                    switch (searchCode)
                    {
                        case 1:
                            var customer = context.Customers
                            .FirstOrDefault(x => x.CustomerId == id);
                            tbCId.Text = customer.CustomerId;
                            tbCName.Text = customer.Name;
                            break;
                        case 4:
                            var cvId = Convert.ToInt32(id);
                            var customerVehicle = context.CustomerVehicles
                                .FirstOrDefault(x => x.CustomerVehicleId == cvId);
                            tbCVId.Text = customerVehicle.CustomerVehicleId.ToString();
                            tbVNumber.Text = customerVehicle.Number;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void TransactionForm_Load(object sender, EventArgs e)
        {
            lTrxId.Text = Assistance.GenerateID(6, Master.Transaction.ToString());
            lDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #region Key Down

        private void tbCId_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownF3(1, e, null);
        }

        private void tbEId_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownF3(3, e, null);
        }

        private void tbCVId_KeyDown(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(tbCId.Text))
            {
                Mbox.Error(title, "Customer must choosen first");
                return;
            }

            KeyDownF3(4, e, tbCId.Text);
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var valid = Validate();
            if (!valid) return;

            var found = lData.FirstOrDefault(x => x.VehicleNumber == tbVNumber.Text);
            if (operation == Operation.None)
            {
                if (found != null) operation = Operation.Add;
                else operation = Operation.None;
            }

            switch (operation)
            {
                case Operation.None:
                    var context = new db();
                    var id = "";
                    while (true)
                    {
                        id = Assistance.GenerateID(5, Master.Transaction.ToString());
                        var validId = context.Customers.FirstOrDefault(x => x.CustomerId == id) == null ? true : false;
                        if (validId) break;
                    }

                    var cvId = Convert.ToInt32(tbCVId.Text);
                    var customerVehicle = context.CustomerVehicles
                        .FirstOrDefault(x => x.CustomerVehicleId == cvId);

                    var trx = new ViewModelTrx()
                    {
                        TrxId = id,
                        CustomerId = customerVehicle.CustomerId,
                        CustomerName = customerVehicle.Customer.Name,
                        CustomerVehicleId = customerVehicle.CustomerVehicleId,
                        VehicleId = customerVehicle.VehicleId,
                        VehicleName = customerVehicle.Vehicle.Name,
                        VehicleNumber = customerVehicle.Number,
                        Cost = Convert.ToInt32(tbCost.Text)
                    };
                    lData.Add(trx);
                    tbCId.Enabled = false;
                    ClearUI();
                    LoadDatabase();
                    break;
                case Operation.Add:
                    found.Cost += Convert.ToInt32(tbCost.Text);
                    tbCId.Enabled = false;
                    ClearUI();
                    LoadDatabase();
                    break;
                case Operation.Update:
                    Mbox.Error(title, "Operation cannot be executed");
                    break;
                default:
                    break;
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selected = dgv.CurrentRow;
            if (selected is null)
            {
                Mbox.Error(title, "There is no selected row");
                return;
            }
            var context = new db();
            var cName = selected.Cells[0].Value.ToString();
            var vName = selected.Cells[1].Value.ToString();

            var eData = lData
                .Where(x => x.CustomerName == cName &&
                    x.VehicleName == vName)
                .FirstOrDefault();
            switch (operation)
            {
                case Operation.None:
                    operation = Operation.Update;
                    tbCVId.Text = eData.CustomerVehicleId.ToString();
                    tbVNumber.Text = eData.VehicleNumber;
                    tbCId.Text = eData.CustomerId;
                    tbCName.Text = eData.CustomerName;
                    tbCost.Text = eData.Cost.ToString();

                    tbCVId.Enabled = false;
                    break;
                case Operation.Update:
                    eData.Cost = Convert.ToInt32(tbCost.Text);
                    ClearUI();
                    operation = Operation.None;

                    tbCVId.Enabled = true;
                    LoadDatabase();
                    break;
                case Operation.Add:
                    Mbox.Error(title, "Operation cannot be executed");
                    break;
                case Operation.Delete:
                    Mbox.Error(title, "Operation cannot be executed");
                    break;
                default:
                    break;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var selected = dgv.CurrentRow;
            if (selected is null)
            {
                Mbox.Error(title, "There is no selected row");
                return;
            }

            switch (operation)
            {
                case Operation.None:
                    var cName = selected.Cells[0].Value.ToString();
                    var vName = selected.Cells[1].Value.ToString();

                    var dData = lData
                        .FirstOrDefault(x =>
                            x.CustomerName == cName &&
                            x.VehicleName == vName);
                    lData.Remove(dData);
                    LoadDatabase();
                    break;
                case Operation.Add:
                    break;
                case Operation.Update:
                    Mbox.Error(title, "Operation cannot be executed");
                    break;
                case Operation.Delete:
                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearUI();
            lData.Clear();
            LoadDatabase();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            #region Header Struk

            var strgs = new List<string>()
            {
                "=====================================================================================================================================================",
                "SIDOREJO BENGKEL",
                "=====================================================================================================================================================",
                "",
                ""
            };

            foreach (var s in strgs)
            {
                flowLayoutPanel1.Controls.Add(new Label()
                {
                    Text = s,
                    Anchor = AnchorStyles.Left,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size()
                    {
                        Width = 950,
                        Height = 20
                    },
                });
            }

            #endregion

            #region Body Struk

            var total = 0;
            AddStruk("Customer Name", lData[0].CustomerName);

            foreach (var data in lData)
            {
                total += data.Cost;
                AddStruk(data.VehicleName, data.Cost.ToString());
            }

            #endregion

            flowLayoutPanel1.Controls.Add(new Label()
            {
                Text = "=====================================================================================================================================================",
                Anchor = AnchorStyles.Left,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size()
                {
                    Width = 950,
                    Height = 20
                },
            });

            #region Total

            AddStruk("Total", total.ToString());

            #endregion

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            var context = new db();

            var hData = new HeaderTrx()
            {
                TrxId = lData[0].TrxId,
                Date = DateTime.Today,
                EmployeeId = UserDto.UserId,
                CustomerId = lData[0].CustomerId
            };
            context.HeaderTrxes.Add(hData);
            context.SaveChanges();

            foreach (var i in lData)
            {
                var dData = new DetailTrx()
                {
                    TrxId = hData.TrxId,
                    CustomerVehicleId = i.CustomerVehicleId,
                    Cost = i.Cost
                };
                context.DetailTrxes.Add(dData);
                context.SaveChanges();
            }

            this.Close();
        }
    }
}
