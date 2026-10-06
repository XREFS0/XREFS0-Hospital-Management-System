// PatientDashboard.cs
// Hospital Management System - Patient Dashboard
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class PatientDashboard : Form
    {
        public PatientDashboard()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            lblPatientName.Text = "Welcome, " + Session.Username;
            LoadMyAppointments();
            LoadMyBills();
        }

        private void LoadMyAppointments(string filterStatus = "")
        {
            string query = @"SELECT a.AppointmentID,
                             d.FullName AS DoctorName,
                             d.Specialization,
                             a.AppointmentDate,
                             a.AppointmentTime,
                             a.Status,
                             a.Remarks
                             FROM Appointments a
                             JOIN Doctors d ON a.DoctorID = d.DoctorID
                             WHERE a.PatientID = @PatientID";

            var paramList = new System.Collections.Generic.List<SqliteParameter>();
            paramList.Add(new SqliteParameter("@PatientID", Session.ReferenceID));

            if (!string.IsNullOrEmpty(filterStatus))
            {
                query += " AND a.Status = @Status";
                paramList.Add(new SqliteParameter("@Status", filterStatus));
            }

            query += " ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC";

            dgvAppointments.DataSource = DBHelper.ExecuteQuery(query, paramList.ToArray());
            dgvAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.ReadOnly = true;
            dgvAppointments.AllowUserToAddRows = false;
            dgvAppointments.AllowUserToDeleteRows = false;
        }

        private void LoadMyBills()
        {
            string query = @"SELECT b.BillID,
                             b.Amount,
                             b.BillDate,
                             b.Status,
                             IFNULL((SELECT SUM(AmountPaid) FROM Payments WHERE BillID = b.BillID), 0) AS AmountPaid,
                             b.Amount - IFNULL((SELECT SUM(AmountPaid) FROM Payments WHERE BillID = b.BillID), 0) AS Balance
                             FROM Billing b
                             WHERE b.PatientID = @PatientID
                             ORDER BY b.BillDate DESC";

            SqliteParameter[] p = { new SqliteParameter("@PatientID", Session.ReferenceID) };
            dgvBills.DataSource = DBHelper.ExecuteQuery(query, p);
            dgvBills.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBills.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBills.ReadOnly = true;
            dgvBills.AllowUserToAddRows = false;
            dgvBills.AllowUserToDeleteRows = false;

            // Color rows by payment status
            foreach (DataGridViewRow row in dgvBills.Rows)
            {
                if (row.Cells["Status"].Value != null &&
                    row.Cells["Status"].Value.ToString() == "Unpaid")
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(220, 255, 220);
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                }
            }
        }

        private void btnFilterScheduled_Click(object sender, EventArgs e) => LoadMyAppointments("Scheduled");
        private void btnFilterCompleted_Click(object sender, EventArgs e) => LoadMyAppointments("Completed");
        private void btnFilterCancelled_Click(object sender, EventArgs e) => LoadMyAppointments("Cancelled");
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMyAppointments();
            LoadMyBills();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }

        private void PatientDashboard_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
