// LoginForm.cs
// Hospital Management System - Login Form
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            this.AcceptButton = btnLogin;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT UserID, Username, Role, IFNULL(ReferenceID, 0) AS ReferenceID 
                             FROM Users 
                             WHERE Username = @Username AND PasswordHash = @Password";

            SqliteParameter[] parameters = {
                new SqliteParameter("@Username", username),
                new SqliteParameter("@Password", password)
            };

            DataTable dt = DBHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 1)
            {
                Session.UserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                Session.Username = dt.Rows[0]["Username"].ToString();
                Session.Role = dt.Rows[0]["Role"].ToString();
                Session.ReferenceID = Convert.ToInt32(dt.Rows[0]["ReferenceID"]);

                this.Hide();

                switch (Session.Role)
                {
                    case "Admin":
                        new AdminDashboard().Show();
                        break;
                    case "Doctor":
                        new DoctorAppointmentForm().Show();
                        break;
                    case "Patient":
                        new PatientDashboard().Show();
                        break;
                    default:
                        MessageBox.Show("Unknown role.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin_Click(sender, e);
        }
    }
}