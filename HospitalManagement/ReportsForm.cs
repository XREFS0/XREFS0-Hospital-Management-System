// ReportsForm.cs
// Hospital Management System - Reports
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Data;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
        }

        private void btnPatientVisitReport_Click(object sender, EventArgs e)
        {
            ShowReportInGrid(GetPatientVisitData(), "Patient Visit Report");
            lblReportTitle.Text = "📊 Patient Visit Report";
        }

        private void btnBillingReport_Click(object sender, EventArgs e)
        {
            ShowReportInGrid(GetBillingData(), "Billing Report");
            lblReportTitle.Text = "📊 Billing Report";
        }

        private void btnAppointmentReport_Click(object sender, EventArgs e)
        {
            ShowReportInGrid(GetAppointmentData(), "Appointment Status Report");
            lblReportTitle.Text = "📊 Appointment Status Report";
        }

        private DataTable GetPatientVisitData()
        {
            string query = @"SELECT p.FullName AS PatientName, p.CNIC, p.Contact,
                             COUNT(a.AppointmentID) AS TotalVisits,
                             IFNULL(MAX(a.AppointmentDate),'No Visits') AS LastVisit
                             FROM Patients p
                             LEFT JOIN Appointments a ON p.PatientID = a.PatientID
                             GROUP BY p.FullName, p.CNIC, p.Contact
                             ORDER BY p.FullName";
            return DBHelper.ExecuteQuery(query);
        }

        private DataTable GetBillingData()
        {
            string query = @"SELECT b.BillID, p.FullName AS PatientName,
                             b.Amount, b.BillDate, b.Status,
                             IFNULL((SELECT SUM(AmountPaid) FROM Payments WHERE BillID=b.BillID),0) AS AmountPaid,
                             b.Amount - IFNULL((SELECT SUM(AmountPaid) FROM Payments WHERE BillID=b.BillID),0) AS Balance
                             FROM Billing b
                             JOIN Patients p ON b.PatientID = p.PatientID
                             ORDER BY b.BillDate DESC";
            return DBHelper.ExecuteQuery(query);
        }

        private DataTable GetAppointmentData()
        {
            string query = @"SELECT a.AppointmentID, p.FullName AS PatientName,
                             d.FullName AS DoctorName, d.Specialization,
                             a.AppointmentDate, a.AppointmentTime, a.Status, a.Remarks
                             FROM Appointments a
                             JOIN Patients p ON a.PatientID = p.PatientID
                             JOIN Doctors d ON a.DoctorID = d.DoctorID
                             ORDER BY a.AppointmentDate DESC";
            return DBHelper.ExecuteQuery(query);
        }

        private void ShowReportInGrid(DataTable dt, string title)
        {
            dgvReport.DataSource = dt;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.ReadOnly = true;
            dgvReport.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}