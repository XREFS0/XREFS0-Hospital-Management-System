// PaymentForm.Designer.cs
namespace HospitalManagement
{
    partial class PaymentForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBill = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblBillAmount = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.cmbBill = new System.Windows.Forms.ComboBox();
            this.txtAmountPaid = new System.Windows.Forms.TextBox();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.btnRecordPayment = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvPayments = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(0, 153, 153);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(980, 45);
            this.lblTitle.Text = "  💳 Payment Recording";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblBill
            this.lblBill.AutoSize = true;
            this.lblBill.Location = new System.Drawing.Point(20, 58);
            this.lblBill.Text = "Select Unpaid Bill:";
            this.lblBill.Font = new System.Drawing.Font("Segoe UI", 9F);

            // cmbBill
            this.cmbBill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBill.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBill.Location = new System.Drawing.Point(20, 75);
            this.cmbBill.Size = new System.Drawing.Size(450, 26);
            this.cmbBill.SelectedIndexChanged += new System.EventHandler(this.cmbBill_SelectedIndexChanged);

            // Info labels
            this.lblBillAmount.AutoSize = true;
            this.lblBillAmount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBillAmount.ForeColor = System.Drawing.Color.FromArgb(0, 102, 0);
            this.lblBillAmount.Location = new System.Drawing.Point(20, 115);
            this.lblBillAmount.Text = "Bill Amount: -";

            this.lblTotalPaid.AutoSize = true;
            this.lblTotalPaid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalPaid.ForeColor = System.Drawing.Color.FromArgb(0, 0, 153);
            this.lblTotalPaid.Location = new System.Drawing.Point(200, 115);
            this.lblTotalPaid.Text = "Total Paid: -";

            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRemaining.ForeColor = System.Drawing.Color.DarkRed;
            this.lblRemaining.Location = new System.Drawing.Point(370, 115);
            this.lblRemaining.Text = "Remaining: -";

            // lblAmount
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(20, 145);
            this.lblAmount.Text = "Amount Paid (PKR):";
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 9F);

            // txtAmountPaid
            this.txtAmountPaid.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAmountPaid.Location = new System.Drawing.Point(20, 163);
            this.txtAmountPaid.Size = new System.Drawing.Size(180, 26);

            // lblDate
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(220, 145);
            this.lblDate.Text = "Payment Date:";
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F);

            // dtpPaymentDate
            this.dtpPaymentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPaymentDate.Location = new System.Drawing.Point(220, 163);
            this.dtpPaymentDate.Size = new System.Drawing.Size(160, 26);

            // btnRecordPayment
            this.btnRecordPayment.BackColor = System.Drawing.Color.FromArgb(0, 153, 76);
            this.btnRecordPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecordPayment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecordPayment.ForeColor = System.Drawing.Color.White;
            this.btnRecordPayment.Location = new System.Drawing.Point(400, 158);
            this.btnRecordPayment.Size = new System.Drawing.Size(160, 36);
            this.btnRecordPayment.Text = "💾 Record Payment";
            this.btnRecordPayment.Click += new System.EventHandler(this.btnRecordPayment_Click);

            // btnRefresh
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(80, 80, 120);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(575, 158);
            this.btnRefresh.Size = new System.Drawing.Size(100, 36);
            this.btnRefresh.Text = "↻ Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // dgvPayments
            this.dgvPayments.AllowUserToAddRows = false;
            this.dgvPayments.AllowUserToDeleteRows = false;
            this.dgvPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvPayments.Location = new System.Drawing.Point(20, 215);
            this.dgvPayments.Size = new System.Drawing.Size(940, 300);
            this.dgvPayments.ReadOnly = true;
            this.dgvPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 540);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblBill);
            this.Controls.Add(this.cmbBill);
            this.Controls.Add(this.lblBillAmount);
            this.Controls.Add(this.lblTotalPaid);
            this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.txtAmountPaid);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpPaymentDate);
            this.Controls.Add(this.btnRecordPayment);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvPayments);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Payment Recording";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblBill, lblAmount, lblDate, lblBillAmount, lblTotalPaid, lblRemaining;
        private System.Windows.Forms.ComboBox cmbBill;
        private System.Windows.Forms.TextBox txtAmountPaid;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.Button btnRecordPayment, btnRefresh;
        private System.Windows.Forms.DataGridView dgvPayments;
    }
}
