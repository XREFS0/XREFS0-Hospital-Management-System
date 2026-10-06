// AppointmentForm.cs
// Hospital Management System - Appointment Management
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class AppointmentForm : Form
    {
        private int selectedAppointmentID = 0;

        public AppointmentForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadComboBoxes();
            LoadAppointments();
        }

        private void LoadComboBoxes()
        {
            // Load Patients
            DataTable dtPatients = DBHelper.ExecuteQuery("SELECT PatientID, FullName FROM Patients ORDER BY FullName");
            cmbPatient.DataSource = dtPatients;
            cmbPatient.DisplayMember = "FullName";
            cmbPatient.ValueMember = "PatientID";

            // Load Doctors
            DataTable dtDoctors = DBHelper.ExecuteQuery("SELECT DoctorID, FullName FROM Doctors ORDER BY FullName");
            cmbDoctor.DataSource = dtDoctors;
            cmbDoctor.DisplayMember = "FullName";
            cmbDoctor.ValueMember = "DoctorID";

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[] { "Scheduled", "Completed", "Cancelled" });
            cmbStatus.SelectedIndex = 0;
        }

        private void LoadAppointments(string filterStatus = "")
        {
            string query = @"SELECT a.AppointmentID, p.FullName AS PatientName, d.FullName AS DoctorName,
                             a.AppointmentDate, a.AppointmentTime, a.Status, a.Remarks
                             FROM Appointments a
                             JOIN Patients p ON a.PatientID = p.PatientID
                             JOIN Doctors d ON a.DoctorID = d.DoctorID";
            if (!string.IsNullOrEmpty(filterStatus))
            {
                query += " WHERE a.Status = @Status";
                SqliteParameter[] p = { new SqliteParameter("@Status", filterStatus) };
                dgvAppointments.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                dgvAppointments.DataSource = DBHelper.ExecuteQuery(query);
            }
            dgvAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            // Check double booking
            string checkQuery = @"SELECT COUNT(*) FROM Appointments 
                                  WHERE DoctorID = @DoctorID 
                                  AND AppointmentDate = @Date 
                                  AND AppointmentTime = @Time
                                  AND Status != 'Cancelled'";
            SqliteParameter[] checkP = {
                new SqliteParameter("@DoctorID", cmbDoctor.SelectedValue),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@Time", txtTime.Text.Trim())
            };
            int conflict = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (conflict > 0)
            {
                MessageBox.Show("This doctor already has an appointment at the selected date and time.",
                    "Double Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks) 
                             VALUES (@PatientID, @DoctorID, @Date, @Time, @Status, @Remarks)";
            SqliteParameter[] parameters = {
                new SqliteParameter("@PatientID", cmbPatient.SelectedValue),
                new SqliteParameter("@DoctorID", cmbDoctor.SelectedValue),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@Time", txtTime.Text.Trim()),
                new SqliteParameter("@Status", cmbStatus.SelectedItem.ToString()),
                new SqliteParameter("@Remarks", txtRemarks.Text.Trim())
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Appointment created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadAppointments();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == 0)
            {
                MessageBox.Show("Please select an appointment.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            // Check double booking excluding this appointment
            string checkQuery = @"SELECT COUNT(*) FROM Appointments 
                                  WHERE DoctorID = @DoctorID AND AppointmentDate = @Date 
                                  AND AppointmentTime = @Time AND AppointmentID != @ID
                                  AND Status != 'Cancelled'";
            SqliteParameter[] checkP = {
                new SqliteParameter("@DoctorID", cmbDoctor.SelectedValue),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@Time", txtTime.Text.Trim()),
                new SqliteParameter("@ID", selectedAppointmentID)
            };
            int conflict = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (conflict > 0)
            {
                MessageBox.Show("Double booking detected for this doctor at selected time.",
                    "Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"UPDATE Appointments SET PatientID=@PatientID, DoctorID=@DoctorID, 
                             AppointmentDate=@Date, AppointmentTime=@Time, Status=@Status, 
                             Remarks=@Remarks WHERE AppointmentID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@PatientID", cmbPatient.SelectedValue),
                new SqliteParameter("@DoctorID", cmbDoctor.SelectedValue),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@Time", txtTime.Text.Trim()),
                new SqliteParameter("@Status", cmbStatus.SelectedItem.ToString()),
                new SqliteParameter("@Remarks", txtRemarks.Text.Trim()),
                new SqliteParameter("@ID", selectedAppointmentID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Appointment updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadAppointments();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == 0)
            {
                MessageBox.Show("Please select an appointment.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Cancel this appointment?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "UPDATE Appointments SET Status='Cancelled' WHERE AppointmentID=@ID";
                SqliteParameter[] parameters = { new SqliteParameter("@ID", selectedAppointmentID) };
                if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
                {
                    MessageBox.Show("Appointment cancelled.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadAppointments();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void btnFilterScheduled_Click(object sender, EventArgs e) => LoadAppointments("Scheduled");
        private void btnFilterCompleted_Click(object sender, EventArgs e) => LoadAppointments("Completed");
        private void btnFilterCancelled_Click(object sender, EventArgs e) => LoadAppointments("Cancelled");
        private void btnRefresh_Click(object sender, EventArgs e) => LoadAppointments();

        private void dgvAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];
                selectedAppointmentID = Convert.ToInt32(row.Cells["AppointmentID"].Value);

                // Load full appointment from DB to populate combo boxes properly
                string query = "SELECT * FROM Appointments WHERE AppointmentID = @ID";
                SqliteParameter[] p = { new SqliteParameter("@ID", selectedAppointmentID) };
                DataTable dt = DBHelper.ExecuteQuery(query, p);
                if (dt.Rows.Count > 0)
                {
                    cmbPatient.SelectedValue = dt.Rows[0]["PatientID"];
                    cmbDoctor.SelectedValue = dt.Rows[0]["DoctorID"];
                    dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["AppointmentDate"]);
                    txtTime.Text = dt.Rows[0]["AppointmentTime"].ToString();
                    cmbStatus.SelectedItem = dt.Rows[0]["Status"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                }
            }
        }

        private bool ValidateInputs()
        {
            if (cmbPatient.SelectedValue == null)
            { MessageBox.Show("Please select a patient.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (cmbDoctor.SelectedValue == null)
            { MessageBox.Show("Please select a doctor.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtTime.Text))
            { MessageBox.Show("Time is required (e.g., 09:00).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearForm()
        {
            selectedAppointmentID = 0;
            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            if (cmbDoctor.Items.Count > 0) cmbDoctor.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            txtTime.Text = "09:00";
            cmbStatus.SelectedIndex = 0;
            txtRemarks.Clear();
            dgvAppointments.ClearSelection();
        }
    }
}
