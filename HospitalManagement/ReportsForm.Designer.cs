// ReportsForm.Designer.cs
namespace HospitalManagement
{
    partial class ReportsForm
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
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.btnPatientVisitReport = new System.Windows.Forms.Button();
            this.btnBillingReport = new System.Windows.Forms.Button();
            this.btnAppointmentReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(50, 50, 80);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(1100, 45);
            this.lblTitle.Text = "  📊 Reports";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Buttons
            System.Windows.Forms.Button[] btns = {
                btnPatientVisitReport, btnBillingReport,
                btnAppointmentReport, btnClose
            };
            string[] texts = {
                "👥 Patient Visit Report", "💰 Billing Report",
                "📅 Appointment Status Report", "✕ Close"
            };
            string[] colors = { "0,153,76", "153,76,0", "0,102,204", "150,0,0" };
            for (int i = 0; i < btns.Length; i++)
            {
                var p = colors[i].Split(',');
                btns[i].BackColor = System.Drawing.Color.FromArgb(
                    int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btns[i].Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                btns[i].ForeColor = System.Drawing.Color.White;
                btns[i].Location = new System.Drawing.Point(15 + i * 265, 58);
                btns[i].Size = new System.Drawing.Size(250, 40);
                btns[i].Text = texts[i];
            }
            btnPatientVisitReport.Click += new System.EventHandler(this.btnPatientVisitReport_Click);
            btnBillingReport.Click += new System.EventHandler(this.btnBillingReport_Click);
            btnAppointmentReport.Click += new System.EventHandler(this.btnAppointmentReport_Click);
            btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // lblReportTitle
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 80);
            this.lblReportTitle.Location = new System.Drawing.Point(15, 112);
            this.lblReportTitle.Text = "Select a report to generate...";

            // dgvReport
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.Location = new System.Drawing.Point(15, 145);
            this.dgvReport.Size = new System.Drawing.Size(1060, 490);
            this.dgvReport.ReadOnly = true;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Visible = true;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 660);
            this.Controls.Add(this.lblTitle);
            foreach (var b in btns) this.Controls.Add(b);
            this.Controls.Add(this.lblReportTitle);
            this.Controls.Add(this.dgvReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reports";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblReportTitle;
        private System.Windows.Forms.Button btnPatientVisitReport, btnBillingReport,
                                            btnAppointmentReport, btnClose;
        private System.Windows.Forms.DataGridView dgvReport;
    }
}