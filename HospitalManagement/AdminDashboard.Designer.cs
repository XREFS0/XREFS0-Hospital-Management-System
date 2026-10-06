// AdminDashboard.Designer.cs
namespace HospitalManagement
{
    partial class AdminDashboard
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelStats = new System.Windows.Forms.Panel();
            this.lblTotalPatients = new System.Windows.Forms.Label();
            this.lblTotalDoctors = new System.Windows.Forms.Label();
            this.lblTotalAppointments = new System.Windows.Forms.Label();
            this.lblPendingAppointments = new System.Windows.Forms.Label();
            this.lblUnpaidBills = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnManagePatients = new System.Windows.Forms.Button();
            this.btnManageDoctors = new System.Windows.Forms.Button();
            this.btnManageAppointments = new System.Windows.Forms.Button();
            this.btnManageBilling = new System.Windows.Forms.Button();
            this.btnManageMedicalRecords = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            this.panelTop.Controls.Add(this.lblWelcome);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Size = new System.Drawing.Size(900, 60);

            this.lblWelcome.AutoSize = false;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(10, 12);
            this.lblWelcome.Size = new System.Drawing.Size(600, 35);
            this.lblWelcome.Text = "Admin Dashboard";

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(0, 180, 100);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(680, 14);
            this.btnRefresh.Size = new System.Drawing.Size(90, 32);
            this.btnRefresh.Text = "↻ Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(785, 14);
            this.btnLogout.Size = new System.Drawing.Size(90, 32);
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // panelStats
            this.panelStats.BackColor = System.Drawing.Color.FromArgb(230, 245, 255);
            this.panelStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStats.Controls.Add(this.lblTotalPatients);
            this.panelStats.Controls.Add(this.lblTotalDoctors);
            this.panelStats.Controls.Add(this.lblTotalAppointments);
            this.panelStats.Controls.Add(this.lblPendingAppointments);
            this.panelStats.Controls.Add(this.lblUnpaidBills);
            this.panelStats.Location = new System.Drawing.Point(15, 75);
            this.panelStats.Size = new System.Drawing.Size(860, 80);

            System.Windows.Forms.Label[] statLabels = {
                this.lblTotalPatients, this.lblTotalDoctors,
                this.lblTotalAppointments, this.lblPendingAppointments, this.lblUnpaidBills
            };

            string[] statColors = { "0,153,76", "0,102,204", "153,76,0", "204,102,0", "180,0,0" };
            for (int i = 0; i < statLabels.Length; i++)
            {
                statLabels[i].AutoSize = false;
                statLabels[i].BackColor = System.Drawing.Color.White;
                statLabels[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                statLabels[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                var parts = statColors[i].Split(',');
                statLabels[i].ForeColor = System.Drawing.Color.FromArgb(
                    int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                statLabels[i].Location = new System.Drawing.Point(10 + i * 170, 10);
                statLabels[i].Size = new System.Drawing.Size(160, 55);
                statLabels[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }

            // panelButtons
            this.panelButtons.Location = new System.Drawing.Point(15, 170);
            this.panelButtons.Size = new System.Drawing.Size(860, 350);
            this.panelButtons.Controls.Add(this.btnManagePatients);
            this.panelButtons.Controls.Add(this.btnManageDoctors);
            this.panelButtons.Controls.Add(this.btnManageAppointments);
            this.panelButtons.Controls.Add(this.btnManageBilling);
            this.panelButtons.Controls.Add(this.btnManageMedicalRecords);
            this.panelButtons.Controls.Add(this.btnPayments);
            this.panelButtons.Controls.Add(this.btnReports);

            string[] btnTexts = { "👥 Manage Patients", "🩺 Manage Doctors", "📅 Manage Appointments",
                                   "💰 Manage Billing", "📋 Medical Records", "💳 Payments", "📊 Reports" };
            System.Windows.Forms.Button[] btns = {
                btnManagePatients, btnManageDoctors, btnManageAppointments,
                btnManageBilling, btnManageMedicalRecords, btnPayments, btnReports
            };
            string[] btnColorArr = {
                "0,153,76","0,102,204","153,76,0","153,0,76","76,0,153","0,153,153","80,80,80"
            };

            for (int i = 0; i < btns.Length; i++)
            {
                var p = btnColorArr[i].Split(',');
                btns[i].BackColor = System.Drawing.Color.FromArgb(
                    int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btns[i].Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
                btns[i].ForeColor = System.Drawing.Color.White;
                btns[i].Size = new System.Drawing.Size(190, 80);
                btns[i].Text = btnTexts[i];
                btns[i].Location = new System.Drawing.Point(10 + (i % 4) * 210, 10 + (i / 4) * 100);
            }

            btns[0].Click += new System.EventHandler(this.btnManagePatients_Click);
            btns[1].Click += new System.EventHandler(this.btnManageDoctors_Click);
            btns[2].Click += new System.EventHandler(this.btnManageAppointments_Click);
            btns[3].Click += new System.EventHandler(this.btnManageBilling_Click);
            btns[4].Click += new System.EventHandler(this.btnManageMedicalRecords_Click);
            btns[5].Click += new System.EventHandler(this.btnPayments_Click);
            btns[6].Click += new System.EventHandler(this.btnReports_Click);

            // AdminDashboard
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard - Hospital Management System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdminDashboard_FormClosed);
            this.panelTop.ResumeLayout(false);
            this.panelStats.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label lblTotalPatients;
        private System.Windows.Forms.Label lblTotalDoctors;
        private System.Windows.Forms.Label lblTotalAppointments;
        private System.Windows.Forms.Label lblPendingAppointments;
        private System.Windows.Forms.Label lblUnpaidBills;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnManagePatients;
        private System.Windows.Forms.Button btnManageDoctors;
        private System.Windows.Forms.Button btnManageAppointments;
        private System.Windows.Forms.Button btnManageBilling;
        private System.Windows.Forms.Button btnManageMedicalRecords;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnReports;
    }
}
