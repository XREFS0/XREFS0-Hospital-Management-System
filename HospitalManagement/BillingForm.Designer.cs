// BillingForm.Designer.cs
namespace HospitalManagement
{
    partial class BillingForm
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
            this.lblPatient = new System.Windows.Forms.Label();
            this.lblAppointment = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.cmbAppointment = new System.Windows.Forms.ComboBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            this.btnFilterPaid = new System.Windows.Forms.Button();
            this.btnFilterUnpaid = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvBills = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBills)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(0, 102, 0);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(980, 45);
            this.lblTitle.Text = "  💰 Billing Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPatient.AutoSize = true; this.lblPatient.Location = new System.Drawing.Point(20, 58); this.lblPatient.Text = "Patient:";
            this.cmbPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPatient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPatient.Location = new System.Drawing.Point(20, 75); this.cmbPatient.Size = new System.Drawing.Size(260, 26);

            this.lblAppointment.AutoSize = true; this.lblAppointment.Location = new System.Drawing.Point(300, 58); this.lblAppointment.Text = "Appointment:";
            this.cmbAppointment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppointment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAppointment.Location = new System.Drawing.Point(300, 75); this.cmbAppointment.Size = new System.Drawing.Size(250, 26);

            this.lblAmount.AutoSize = true; this.lblAmount.Location = new System.Drawing.Point(20, 115); this.lblAmount.Text = "Amount (PKR):";
            this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAmount.Location = new System.Drawing.Point(20, 132); this.txtAmount.Size = new System.Drawing.Size(160, 26);

            this.lblDate.AutoSize = true; this.lblDate.Location = new System.Drawing.Point(200, 115); this.lblDate.Text = "Bill Date:";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(200, 132); this.dtpDate.Size = new System.Drawing.Size(150, 26);

            this.lblStatus.AutoSize = true; this.lblStatus.Location = new System.Drawing.Point(370, 115); this.lblStatus.Text = "Status:";
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.Location = new System.Drawing.Point(370, 132); this.cmbStatus.Size = new System.Drawing.Size(130, 26);

            // Action buttons
            System.Windows.Forms.Button[] actBtns = { btnGenerate, btnUpdateStatus, btnClear };
            string[] actTexts = { "Generate Bill", "Update Status", "Clear" };
            string[] actColors = { "0,153,76", "0,102,204", "80,80,80" };
            for (int i = 0; i < actBtns.Length; i++)
            {
                var p = actColors[i].Split(',');
                actBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                actBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                actBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                actBtns[i].ForeColor = System.Drawing.Color.White;
                actBtns[i].Location = new System.Drawing.Point(20 + i * 140, 180);
                actBtns[i].Size = new System.Drawing.Size(130, 32);
                actBtns[i].Text = actTexts[i];
            }
            btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // Filter buttons
            System.Windows.Forms.Button[] fBtns = { btnFilterPaid, btnFilterUnpaid, btnRefresh };
            string[] fTexts = { "✓ Show Paid", "✗ Show Unpaid", "↻ All" };
            string[] fColors = { "0,140,70", "180,0,0", "80,80,120" };
            for (int i = 0; i < fBtns.Length; i++)
            {
                var p = fColors[i].Split(',');
                fBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                fBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                fBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                fBtns[i].ForeColor = System.Drawing.Color.White;
                fBtns[i].Location = new System.Drawing.Point(20 + i * 140, 228);
                fBtns[i].Size = new System.Drawing.Size(130, 28);
                fBtns[i].Text = fTexts[i];
            }
            btnFilterPaid.Click += new System.EventHandler(this.btnFilterPaid_Click);
            btnFilterUnpaid.Click += new System.EventHandler(this.btnFilterUnpaid_Click);
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.dgvBills.AllowUserToAddRows = false;
            this.dgvBills.AllowUserToDeleteRows = false;
            this.dgvBills.BackgroundColor = System.Drawing.Color.White;
            this.dgvBills.Location = new System.Drawing.Point(20, 272);
            this.dgvBills.Size = new System.Drawing.Size(940, 280);
            this.dgvBills.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBills_CellClick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 575);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPatient); this.Controls.Add(this.cmbPatient);
            this.Controls.Add(this.lblAppointment); this.Controls.Add(this.cmbAppointment);
            this.Controls.Add(this.lblAmount); this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblDate); this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblStatus); this.Controls.Add(this.cmbStatus);
            foreach (var b in actBtns) this.Controls.Add(b);
            foreach (var b in fBtns) this.Controls.Add(b);
            this.Controls.Add(this.dgvBills);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "BillingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Billing Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBills)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblPatient, lblAppointment, lblAmount, lblDate, lblStatus;
        private System.Windows.Forms.ComboBox cmbPatient, cmbAppointment, cmbStatus;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnGenerate, btnUpdateStatus, btnFilterPaid, btnFilterUnpaid, btnRefresh, btnClear;
        private System.Windows.Forms.DataGridView dgvBills;
    }
}
