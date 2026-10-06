// BillingForm.cs
// Hospital Management System - Billing
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class BillingForm : Form
    {
        private int selectedBillID = 0;

        public BillingForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadComboBoxes();
            LoadBills();
        }

        private void LoadComboBoxes()
        {
            DataTable dtPatients = DBHelper.ExecuteQuery("SELECT PatientID, FullName FROM Patients ORDER BY FullName");
            cmbPatient.DataSource = dtPatients;
            cmbPatient.DisplayMember = "FullName";
            cmbPatient.ValueMember = "PatientID";
            cmbPatient.SelectedIndexChanged += new System.EventHandler(cmbPatient_SelectedIndexChanged);

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "Unpaid", "Paid" });
            cmbStatus.SelectedIndex = 0;
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedValue == null) return;
            string query = @"SELECT AppointmentID, ('Appt #' || AppointmentID || ' - ' || AppointmentDate) AS AppointmentInfo
                             FROM Appointments WHERE PatientID = @PID AND Status != 'Cancelled'";
            SqliteParameter[] p = { new SqliteParameter("@PID", cmbPatient.SelectedValue) };
            DataTable dt = DBHelper.ExecuteQuery(query, p);
            cmbAppointment.DataSource = dt;
            cmbAppointment.DisplayMember = "AppointmentInfo";
            cmbAppointment.ValueMember = "AppointmentID";
        }

        private void LoadBills(string filterStatus = "")
        {
            string query = @"SELECT b.BillID, p.FullName AS PatientName, b.AppointmentID,
                             b.Amount, b.BillDate, b.Status, b.CreatedAt
                             FROM Billing b
                             JOIN Patients p ON b.PatientID = p.PatientID";
            if (!string.IsNullOrEmpty(filterStatus))
            {
                query += " WHERE b.Status = @Status";
                SqliteParameter[] p = { new SqliteParameter("@Status", filterStatus) };
                dgvBills.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                dgvBills.DataSource = DBHelper.ExecuteQuery(query);
            }
            dgvBills.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBills.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBills.ReadOnly = true;

            // Highlight unpaid bills in red
            foreach (DataGridViewRow row in dgvBills.Rows)
            {
                if (row.Cells["Status"].Value != null &&
                    row.Cells["Status"].Value.ToString() == "Unpaid")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedValue == null || cmbAppointment.SelectedValue == null)
            { MessageBox.Show("Select patient and appointment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            decimal amount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            { MessageBox.Show("Enter a valid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string query = @"INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
                             VALUES (@PID, @AID, @Amount, @Date, @Status)";
            SqliteParameter[] parameters = {
                new SqliteParameter("@PID", cmbPatient.SelectedValue),
                new SqliteParameter("@AID", cmbAppointment.SelectedValue),
                new SqliteParameter("@Amount", amount),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@Status", cmbStatus.SelectedItem.ToString())
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Bill generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBills();
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedBillID == 0)
            { MessageBox.Show("Select a bill.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string query = "UPDATE Billing SET Status=@Status WHERE BillID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Status", cmbStatus.SelectedItem.ToString()),
                new SqliteParameter("@ID", selectedBillID)
            };
            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Bill status updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadBills();
            }
        }

        private void btnFilterPaid_Click(object sender, EventArgs e) => LoadBills("Paid");
        private void btnFilterUnpaid_Click(object sender, EventArgs e) => LoadBills("Unpaid");
        private void btnRefresh_Click(object sender, EventArgs e) => LoadBills();
        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBills.Rows[e.RowIndex];
                selectedBillID = Convert.ToInt32(row.Cells["BillID"].Value);
                string status = row.Cells["Status"].Value.ToString();
                cmbStatus.SelectedItem = status;
                txtAmount.Text = row.Cells["Amount"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            selectedBillID = 0;
            txtAmount.Clear();
            dtpDate.Value = DateTime.Today;
            cmbStatus.SelectedIndex = 0;
            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            dgvBills.ClearSelection();
        }
    }
}
