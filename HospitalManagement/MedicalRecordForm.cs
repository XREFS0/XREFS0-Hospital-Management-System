// MedicalRecordForm.cs
// Hospital Management System - Medical Records
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class MedicalRecordForm : Form
    {
        private int selectedRecordID = 0;

        public MedicalRecordForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadPatientCombo();
            LoadRecords();
        }

        private void LoadPatientCombo()
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT PatientID, FullName FROM Patients ORDER BY FullName");
            cmbPatient.DataSource = dt;
            cmbPatient.DisplayMember = "FullName";
            cmbPatient.ValueMember = "PatientID";
        }

        private void LoadRecords(int patientFilter = 0)
        {
            string query = @"SELECT r.RecordID, p.FullName AS PatientName, r.Diagnosis, 
                             r.Treatment, r.Prescription, r.RecordDate, r.CreatedAt
                             FROM MedicalRecords r
                             JOIN Patients p ON r.PatientID = p.PatientID";
            if (patientFilter > 0)
            {
                query += " WHERE r.PatientID = @PID ORDER BY r.RecordDate DESC";
                SqliteParameter[] p = { new SqliteParameter("@PID", patientFilter) };
                dgvRecords.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                query += " ORDER BY r.RecordDate DESC";
                dgvRecords.DataSource = DBHelper.ExecuteQuery(query);
            }
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecords.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            // Always INSERT - never overwrite history
            string query = @"INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
                             VALUES (@PatientID, @Diagnosis, @Treatment, @Prescription, @Date)";
            SqliteParameter[] parameters = {
                new SqliteParameter("@PatientID", cmbPatient.SelectedValue),
                new SqliteParameter("@Diagnosis", txtDiagnosis.Text.Trim()),
                new SqliteParameter("@Treatment", txtTreatment.Text.Trim()),
                new SqliteParameter("@Prescription", txtPrescription.Text.Trim()),
                new SqliteParameter("@Date", dtpDate.Value.Date)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Medical record added.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadRecords();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRecordID == 0)
            {
                MessageBox.Show("Please select a record to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            string query = @"UPDATE MedicalRecords SET PatientID=@PatientID, Diagnosis=@Diagnosis, 
                             Treatment=@Treatment, Prescription=@Prescription, RecordDate=@Date
                             WHERE RecordID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@PatientID", cmbPatient.SelectedValue),
                new SqliteParameter("@Diagnosis", txtDiagnosis.Text.Trim()),
                new SqliteParameter("@Treatment", txtTreatment.Text.Trim()),
                new SqliteParameter("@Prescription", txtPrescription.Text.Trim()),
                new SqliteParameter("@Date", dtpDate.Value.Date),
                new SqliteParameter("@ID", selectedRecordID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Record updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadRecords();
            }
        }

        private void btnViewByPatient_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedValue != null)
                LoadRecords(Convert.ToInt32(cmbPatient.SelectedValue));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadRecords();
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void dgvRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRecords.Rows[e.RowIndex];
                selectedRecordID = Convert.ToInt32(row.Cells["RecordID"].Value);

                string query = "SELECT * FROM MedicalRecords WHERE RecordID = @ID";
                SqliteParameter[] p = { new SqliteParameter("@ID", selectedRecordID) };
                DataTable dt = DBHelper.ExecuteQuery(query, p);
                if (dt.Rows.Count > 0)
                {
                    cmbPatient.SelectedValue = dt.Rows[0]["PatientID"];
                    txtDiagnosis.Text = dt.Rows[0]["Diagnosis"].ToString();
                    txtTreatment.Text = dt.Rows[0]["Treatment"].ToString();
                    txtPrescription.Text = dt.Rows[0]["Prescription"].ToString();
                    dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["RecordDate"]);
                }
            }
        }

        private bool ValidateInputs()
        {
            if (cmbPatient.SelectedValue == null)
            { MessageBox.Show("Select a patient.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text))
            { MessageBox.Show("Diagnosis is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtTreatment.Text))
            { MessageBox.Show("Treatment is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtPrescription.Text))
            { MessageBox.Show("Prescription is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearForm()
        {
            selectedRecordID = 0;
            if (cmbPatient.Items.Count > 0) cmbPatient.SelectedIndex = 0;
            txtDiagnosis.Clear();
            txtTreatment.Clear();
            txtPrescription.Clear();
            dtpDate.Value = DateTime.Today;
            dgvRecords.ClearSelection();
        }
    }
}
