// PaymentForm.cs
// Hospital Management System - Payment Recording
// Author: XREFS0

using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class PaymentForm : Form
    {
        public PaymentForm()
        {
            InitializeComponent();
            Theme.ApplyForm(this);
            LoadUnpaidBills();
            LoadPayments();
        }

        private void LoadUnpaidBills()
        {
            string query = @"SELECT b.BillID, 
                             ('Bill #' || b.BillID || ' - ' || p.FullName || ' - PKR ' || b.Amount) AS BillInfo,
                             b.Amount
                             FROM Billing b
                             JOIN Patients p ON b.PatientID = p.PatientID
                             WHERE b.Status = 'Unpaid'";
            DataTable dt = DBHelper.ExecuteQuery(query);
            cmbBill.DataSource = dt;
            cmbBill.DisplayMember = "BillInfo";
            cmbBill.ValueMember = "BillID";
        }

        private void LoadPayments()
        {
            string query = @"SELECT py.PaymentID, b.BillID, p.FullName AS PatientName,
                             b.Amount AS BillAmount, py.AmountPaid, py.PaymentDate,
                             (b.Amount - IFNULL(
                                (SELECT SUM(AmountPaid) FROM Payments WHERE BillID = b.BillID), 0
                             )) AS RemainingBalance
                             FROM Payments py
                             JOIN Billing b ON py.BillID = b.BillID
                             JOIN Patients p ON b.PatientID = p.PatientID
                             ORDER BY py.PaymentDate DESC";
            dgvPayments.DataSource = DBHelper.ExecuteQuery(query);
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.ReadOnly = true;
        }

        private void cmbBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBill.SelectedValue == null || cmbBill.Items.Count == 0) return;

            int billID = Convert.ToInt32(cmbBill.SelectedValue);

            // Get bill amount
            string queryBill = "SELECT Amount FROM Billing WHERE BillID = @ID";
            SqliteParameter[] p1 = { new SqliteParameter("@ID", billID) };
            object billAmount = DBHelper.ExecuteScalar(queryBill, p1);

            // Get total paid so far
            string queryPaid = "SELECT IFNULL(SUM(AmountPaid),0) FROM Payments WHERE BillID = @ID";
            SqliteParameter[] p2 = { new SqliteParameter("@ID", billID) };
            object totalPaid = DBHelper.ExecuteScalar(queryPaid, p2);

            if (billAmount != null && totalPaid != null)
            {
                decimal total = Convert.ToDecimal(billAmount);
                decimal paid = Convert.ToDecimal(totalPaid);
                decimal remaining = total - paid;
                lblBillAmount.Text = "Bill Amount: PKR " + total.ToString("0.00");
                lblTotalPaid.Text = "Total Paid: PKR " + paid.ToString("0.00");
                lblRemaining.Text = "Remaining: PKR " + remaining.ToString("0.00");
            }
        }

        private void btnRecordPayment_Click(object sender, EventArgs e)
        {
            if (cmbBill.SelectedValue == null || cmbBill.Items.Count == 0)
            { MessageBox.Show("Select a bill.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            decimal amountPaid;
            if (!decimal.TryParse(txtAmountPaid.Text.Trim(), out amountPaid) || amountPaid <= 0)
            { MessageBox.Show("Enter valid payment amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int billID = Convert.ToInt32(cmbBill.SelectedValue);

            // Check remaining balance
            string queryBill = "SELECT Amount FROM Billing WHERE BillID = @ID";
            string queryPaid = "SELECT IFNULL(SUM(AmountPaid),0) FROM Payments WHERE BillID = @ID";
            SqliteParameter[] p = { new SqliteParameter("@ID", billID) };

            decimal total = Convert.ToDecimal(DBHelper.ExecuteScalar(queryBill, p));
            decimal alreadyPaid = Convert.ToDecimal(DBHelper.ExecuteScalar(queryPaid, p));
            decimal remaining = total - alreadyPaid;

            if (amountPaid > remaining)
            {
                MessageBox.Show($"Payment exceeds remaining balance of PKR {remaining:0.00}. Overpayment not allowed.",
                    "Overpayment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insert payment
            string insertQuery = @"INSERT INTO Payments (BillID, AmountPaid, PaymentDate) 
                                   VALUES (@BillID, @Amount, @Date)";
            SqliteParameter[] insertParams = {
                new SqliteParameter("@BillID", billID),
                new SqliteParameter("@Amount", amountPaid),
                new SqliteParameter("@Date", dtpPaymentDate.Value.Date)
            };
            DBHelper.ExecuteNonQuery(insertQuery, insertParams);

            // If fully paid, update bill status
            decimal newTotal = alreadyPaid + amountPaid;
            if (newTotal >= total)
            {
                string updateQuery = "UPDATE Billing SET Status='Paid' WHERE BillID=@ID";
                SqliteParameter[] updateParams = { new SqliteParameter("@ID", billID) };
                DBHelper.ExecuteNonQuery(updateQuery, updateParams);
                MessageBox.Show("Payment recorded. Bill is now fully PAID.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Payment recorded. Remaining balance: PKR {(total - newTotal):0.00}", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txtAmountPaid.Clear();
            LoadUnpaidBills();
            LoadPayments();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUnpaidBills();
            LoadPayments();
        }
    }
}
