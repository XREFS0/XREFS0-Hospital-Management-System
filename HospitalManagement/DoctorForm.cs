// DoctorForm.cs
// Hospital Management System - Doctor Management
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class DoctorForm : Form
    {
        private int selectedDoctorID = 0;

        public DoctorForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadDoctors();
        }

        private void LoadDoctors(string searchTerm = "")
        {
            string query = "SELECT DoctorID, FullName, Specialization, Contact, CreatedAt FROM Doctors";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE FullName LIKE @Search OR Specialization LIKE @Search";
                SqliteParameter[] p = { new SqliteParameter("@Search", "%" + searchTerm + "%") };
                dgvDoctors.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                dgvDoctors.DataSource = DBHelper.ExecuteQuery(query);
            }
            dgvDoctors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDoctors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoctors.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string query = @"INSERT INTO Doctors (FullName, Specialization, Contact) 
                             VALUES (@Name, @Spec, @Contact)";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Name", txtName.Text.Trim()),
                new SqliteParameter("@Spec", txtSpecialization.Text.Trim()),
                new SqliteParameter("@Contact", txtContact.Text.Trim())
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Doctor added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadDoctors();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedDoctorID == 0)
            {
                MessageBox.Show("Please select a doctor to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            string query = @"UPDATE Doctors SET FullName=@Name, Specialization=@Spec, 
                             Contact=@Contact WHERE DoctorID=@ID";
            SqliteParameter[] parameters = {
                new SqliteParameter("@Name", txtName.Text.Trim()),
                new SqliteParameter("@Spec", txtSpecialization.Text.Trim()),
                new SqliteParameter("@Contact", txtContact.Text.Trim()),
                new SqliteParameter("@ID", selectedDoctorID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Doctor updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadDoctors();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedDoctorID == 0)
            {
                MessageBox.Show("Please select a doctor to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this doctor?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM Doctors WHERE DoctorID = @ID";
                SqliteParameter[] parameters = { new SqliteParameter("@ID", selectedDoctorID) };
                if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
                {
                    MessageBox.Show("Doctor deleted.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadDoctors();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();
        private void btnSearch_Click(object sender, EventArgs e) => LoadDoctors(txtSearch.Text.Trim());
        private void btnRefresh_Click(object sender, EventArgs e) { txtSearch.Clear(); LoadDoctors(); }

        private void dgvDoctors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDoctors.Rows[e.RowIndex];
                selectedDoctorID = Convert.ToInt32(row.Cells["DoctorID"].Value);
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtSpecialization.Text = row.Cells["Specialization"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtSpecialization.Text))
            { MessageBox.Show("Specialization is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            { MessageBox.Show("Contact is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearForm()
        {
            selectedDoctorID = 0;
            txtName.Clear();
            txtSpecialization.Clear();
            txtContact.Clear();
            dgvDoctors.ClearSelection();
        }
    }
}
