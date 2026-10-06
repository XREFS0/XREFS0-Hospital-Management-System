// PatientDashboard.Designer.cs
namespace HospitalManagement
{
    partial class PatientDashboard
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblAppointmentsHeader = new System.Windows.Forms.Label();
            this.btnFilterScheduled = new System.Windows.Forms.Button();
            this.btnFilterCompleted = new System.Windows.Forms.Button();
            this.btnFilterCancelled = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.lblBillsHeader = new System.Windows.Forms.Label();
            this.dgvBills = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBills)).BeginInit();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(0, 102, 153);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblPatientName);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Size = new System.Drawing.Size(980, 65);

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 8);
            this.lblTitle.Size = new System.Drawing.Size(550, 30);
            this.lblTitle.Text = "  🏥 Patient Portal";

            // lblPatientName
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPatientName.ForeColor = System.Drawing.Color.LightCyan;
            this.lblPatientName.Location = new System.Drawing.Point(13, 42);
            this.lblPatientName.Text = "Welcome";

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(865, 16);
            this.btnLogout.Size = new System.Drawing.Size(90, 32);
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // lblAppointmentsHeader
            this.lblAppointmentsHeader.AutoSize = false;
            this.lblAppointmentsHeader.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.lblAppointmentsHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAppointmentsHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAppointmentsHeader.ForeColor = System.Drawing.Color.FromArgb(0, 102, 153);
            this.lblAppointmentsHeader.Location = new System.Drawing.Point(15, 78);
            this.lblAppointmentsHeader.Size = new System.Drawing.Size(940, 30);
            this.lblAppointmentsHeader.Text = "  📅 My Appointments  (read-only — contact reception to make changes)";
            this.lblAppointmentsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Filter buttons
            System.Windows.Forms.Button[] filterBtns = {
                btnFilterScheduled, btnFilterCompleted, btnFilterCancelled, btnRefresh
            };
            string[] filterTexts = { "📋 Scheduled", "✓ Completed", "✗ Cancelled", "↻ All" };
            string[] filterColors = { "0,120,180", "0,140,70", "160,60,0", "80,80,120" };
            for (int i = 0; i < filterBtns.Length; i++)
            {
                var p = filterColors[i].Split(',');
                filterBtns[i].BackColor = System.Drawing.Color.FromArgb(
                    int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                filterBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                filterBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                filterBtns[i].ForeColor = System.Drawing.Color.White;
                filterBtns[i].Location = new System.Drawing.Point(15 + i * 125, 118);
                filterBtns[i].Size = new System.Drawing.Size(115, 28);
                filterBtns[i].Text = filterTexts[i];
            }
            btnFilterScheduled.Click += new System.EventHandler(this.btnFilterScheduled_Click);
            btnFilterCompleted.Click += new System.EventHandler(this.btnFilterCompleted_Click);
            btnFilterCancelled.Click += new System.EventHandler(this.btnFilterCancelled_Click);
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // dgvAppointments
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.BackgroundColor = System.Drawing.Color.White;
            this.dgvAppointments.Location = new System.Drawing.Point(15, 155);
            this.dgvAppointments.Size = new System.Drawing.Size(940, 220);
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // lblBillsHeader
            this.lblBillsHeader.AutoSize = false;
            this.lblBillsHeader.BackColor = System.Drawing.Color.FromArgb(230, 255, 230);
            this.lblBillsHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBillsHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBillsHeader.ForeColor = System.Drawing.Color.FromArgb(0, 102, 0);
            this.lblBillsHeader.Location = new System.Drawing.Point(15, 390);
            this.lblBillsHeader.Size = new System.Drawing.Size(940, 30);
            this.lblBillsHeader.Text = "  💰 My Bills";
            this.lblBillsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // dgvBills
            this.dgvBills.AllowUserToAddRows = false;
            this.dgvBills.AllowUserToDeleteRows = false;
            this.dgvBills.BackgroundColor = System.Drawing.Color.White;
            this.dgvBills.Location = new System.Drawing.Point(15, 428);
            this.dgvBills.Size = new System.Drawing.Size(940, 190);
            this.dgvBills.ReadOnly = true;
            this.dgvBills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 640);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblAppointmentsHeader);
            foreach (var b in filterBtns) this.Controls.Add(b);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.lblBillsHeader);
            this.Controls.Add(this.dgvBills);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PatientDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patient Portal - Hospital Management System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PatientDashboard_FormClosed);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBills)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle, lblPatientName, lblAppointmentsHeader, lblBillsHeader;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnFilterScheduled, btnFilterCompleted, btnFilterCancelled, btnRefresh;
        private System.Windows.Forms.DataGridView dgvAppointments, dgvBills;
    }
}
