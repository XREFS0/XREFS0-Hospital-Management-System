// AdminDashboard.cs
// Hospital Management System - Admin Dashboard
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Data;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            // Total Patients
            object totalPatients = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Patients");
            lblTotalPatients.Text = "Total Patients: " + (totalPatients != null ? totalPatients.ToString() : "0");

            // Total Doctors
            object totalDoctors = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Doctors");
            lblTotalDoctors.Text = "Total Doctors: " + (totalDoctors != null ? totalDoctors.ToString() : "0");

            // Total Appointments
            object totalAppointments = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Appointments");
            lblTotalAppointments.Text = "Total Appointments: " + (totalAppointments != null ? totalAppointments.ToString() : "0");

            // Pending Appointments
            object pendingAppointments = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Appointments WHERE Status = 'Scheduled'");
            lblPendingAppointments.Text = "Pending Appointments: " + (pendingAppointments != null ? pendingAppointments.ToString() : "0");

            // Total Unpaid Bills
            object unpaidBills = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Billing WHERE Status = 'Unpaid'");
            lblUnpaidBills.Text = "Total Unpaid Bills: " + (unpaidBills != null ? unpaidBills.ToString() : "0");

            lblWelcome.Text = "Welcome, " + Session.Username + " (Admin)";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardStats();
            MessageBox.Show("Dashboard refreshed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnManagePatients_Click(object sender, EventArgs e)
        {
            new PatientForm().ShowDialog();
            LoadDashboardStats();
        }

        private void btnManageDoctors_Click(object sender, EventArgs e)
        {
            new DoctorForm().ShowDialog();
            LoadDashboardStats();
        }

        private void btnManageAppointments_Click(object sender, EventArgs e)
        {
            new AppointmentForm().ShowDialog();
            LoadDashboardStats();
        }

        private void btnManageBilling_Click(object sender, EventArgs e)
        {
            new BillingForm().ShowDialog();
            LoadDashboardStats();
        }

        private void btnManageMedicalRecords_Click(object sender, EventArgs e)
        {
            new MedicalRecordForm().ShowDialog();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            new PaymentForm().ShowDialog();
            LoadDashboardStats();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            new ReportsForm().ShowDialog();
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

        private void AdminDashboard_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
