// PatientForm.cs
// Hospital Management System - Patient Management
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class PatientForm : Form
    {
        private int selectedPatientID = 0;

        public PatientForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadPatients();
        }

        private void LoadPatients(string searchTerm = "")
        {
            string query = @"SELECT PatientID, FullName, CNIC, Contact, Address, CreatedAt 
                             FROM Patients";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE FullName LIKE @Search OR CNIC LIKE @Search OR Contact LIKE @Search";
                SqliteParameter[] p = { new SqliteParameter("@Search", "%" + searchTerm + "%") };
                dgvPatients.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                dgvPatients.DataSource = DBHelper.ExecuteQuery(query);
            }

            dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            // Check duplicate CNIC
            string checkQuery = "SELECT COUNT(*) FROM Patients WHERE CNIC = @CNIC";
            SqliteParameter[] checkP = { new SqliteParameter("@CNIC", txtCNIC.Text.Trim()) };
            int count = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (count > 0)
            {
                MessageBox.Show("A patient with this CNIC already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"INSERT INTO Patients (FullName, CNIC, Contact, Address) 
                             VALUES (@Name, @CNIC, @Contact, @Address)";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Name", txtName.Text.Trim()),
                new SqliteParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqliteParameter("@Contact", txtContact.Text.Trim()),
                new SqliteParameter("@Address", txtAddress.Text.Trim())
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Patient added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadPatients();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == 0)
            {
                MessageBox.Show("Please select a patient to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            // Check duplicate CNIC for other patients
            string checkQuery = "SELECT COUNT(*) FROM Patients WHERE CNIC = @CNIC AND PatientID != @ID";
            SqliteParameter[] checkP = {
                new SqliteParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqliteParameter("@ID", selectedPatientID)
            };
            int count = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (count > 0)
            {
                MessageBox.Show("Another patient with this CNIC already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"UPDATE Patients SET FullName=@Name, CNIC=@CNIC, Contact=@Contact, 
                             Address=@Address WHERE PatientID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Name", txtName.Text.Trim()),
                new SqliteParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqliteParameter("@Contact", txtContact.Text.Trim()),
                new SqliteParameter("@Address", txtAddress.Text.Trim()),
                new SqliteParameter("@ID", selectedPatientID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Patient updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadPatients();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == 0)
            {
                MessageBox.Show("Please select a patient to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this patient?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM Patients WHERE PatientID = @ID";
                SqliteParameter[] parameters = { new SqliteParameter("@ID", selectedPatientID) };

                if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
                {
                    MessageBox.Show("Patient deleted successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadPatients();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPatients(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadPatients();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
                selectedPatientID = Convert.ToInt32(row.Cells["PatientID"].Value);
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtCNIC.Text = row.Cells["CNIC"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtCNIC.Text))
            { MessageBox.Show("CNIC is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            { MessageBox.Show("Contact is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            { MessageBox.Show("Address is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearForm()
        {
            selectedPatientID = 0;
            txtName.Clear();
            txtCNIC.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            dgvPatients.ClearSelection();
        }
    }
}
