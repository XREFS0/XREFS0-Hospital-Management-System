// DoctorAppointmentForm.cs
// Hospital Management System - Doctor Appointment Handling (Doctor Dashboard)
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class DoctorAppointmentForm : Form
    {
        private int selectedAppointmentID = 0;

        public DoctorAppointmentForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            lblDoctorName.Text = "Doctor: " + Session.Username;
            LoadMyAppointments();
        }

        private void LoadMyAppointments(string filterStatus = "")
        {
            string query = @"SELECT a.AppointmentID, p.FullName AS PatientName,
                             a.AppointmentDate, a.AppointmentTime, a.Status, a.Remarks
                             FROM Appointments a
                             JOIN Patients p ON a.PatientID = p.PatientID
                             WHERE a.DoctorID = @DoctorID";

            var paramList = new System.Collections.Generic.List<SqliteParameter>();
            paramList.Add(new SqliteParameter("@DoctorID", Session.ReferenceID));

            if (!string.IsNullOrEmpty(filterStatus))
            {
                query += " AND a.Status = @Status";
                paramList.Add(new SqliteParameter("@Status", filterStatus));
            }

            query += " ORDER BY a.AppointmentDate, a.AppointmentTime";

            dgvAppointments.DataSource = DBHelper.ExecuteQuery(query, paramList.ToArray());
            dgvAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.ReadOnly = true;
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == 0)
            {
                MessageBox.Show("Select an appointment.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string currentStatus = lblCurrentStatus.Text.Replace("Current Status: ", "");
            string newStatus = cmbStatus.SelectedItem.ToString();

            bool validTransition = false;
            if (currentStatus == "Scheduled" && (newStatus == "Completed" || newStatus == "Cancelled"))
                validTransition = true;
            else if (currentStatus == "Completed" && newStatus == "Closed")
                validTransition = true;

            if (!validTransition)
            {
                MessageBox.Show($"Invalid status transition from '{currentStatus}' to '{newStatus}'.\n" +
                    "Valid: Scheduled→Completed/Cancelled, Completed→Closed",
                    "Invalid Transition", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Appointments SET Status=@Status, Remarks=@Remarks WHERE AppointmentID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Status", newStatus),
                new SqliteParameter("@Remarks", txtRemarks.Text.Trim()),
                new SqliteParameter("@ID", selectedAppointmentID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Appointment status updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadMyAppointments();
            }
        }

        private void btnFilterScheduled_Click(object sender, EventArgs e) => LoadMyAppointments("Scheduled");
        private void btnFilterCompleted_Click(object sender, EventArgs e) => LoadMyAppointments("Completed");
        private void btnRefresh_Click(object sender, EventArgs e) { ClearForm(); LoadMyAppointments(); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }

        private void dgvAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];
                selectedAppointmentID = Convert.ToInt32(row.Cells["AppointmentID"].Value);
                string currentStatus = row.Cells["Status"].Value.ToString();
                lblCurrentStatus.Text = "Current Status: " + currentStatus;
                txtRemarks.Text = row.Cells["Remarks"].Value?.ToString() ?? "";

                cmbStatus.Items.Clear();
                if (currentStatus == "Scheduled")
                    cmbStatus.Items.AddRange(new string[] { "Completed", "Cancelled" });
                else if (currentStatus == "Completed")
                    cmbStatus.Items.Add("Closed");

                if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            }
        }

        private void ClearForm()
        {
            selectedAppointmentID = 0;
            lblCurrentStatus.Text = "Current Status: -";
            txtRemarks.Clear();
            cmbStatus.Items.Clear();
            dgvAppointments.ClearSelection();
        }

        private void DoctorAppointmentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}