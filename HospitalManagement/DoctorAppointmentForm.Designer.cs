// DoctorAppointmentForm.Designer.cs
namespace HospitalManagement
{
    partial class DoctorAppointmentForm
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
            this.lblDoctorName = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.lblNewStatus = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            this.btnFilterScheduled = new System.Windows.Forms.Button();
            this.btnFilterCompleted = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(0, 102, 153);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblDoctorName);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Size = new System.Drawing.Size(960, 65);

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 8);
            this.lblTitle.Size = new System.Drawing.Size(500, 30);
            this.lblTitle.Text = "  🩺 My Appointments (Doctor View)";

            // lblDoctorName
            this.lblDoctorName.AutoSize = true;
            this.lblDoctorName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDoctorName.ForeColor = System.Drawing.Color.LightCyan;
            this.lblDoctorName.Location = new System.Drawing.Point(13, 42);
            this.lblDoctorName.Text = "Doctor: ";

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(845, 16);
            this.btnLogout.Size = new System.Drawing.Size(90, 32);
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // Filter buttons
            System.Windows.Forms.Button[] filterBtns = { btnFilterScheduled, btnFilterCompleted, btnRefresh };
            string[] filterTexts = { "📋 Scheduled", "✓ Completed", "↻ All" };
            string[] filterColors = { "0,120,180", "0,140,70", "80,80,120" };
            for (int i = 0; i < filterBtns.Length; i++)
            {
                var p = filterColors[i].Split(',');
                filterBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                filterBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                filterBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                filterBtns[i].ForeColor = System.Drawing.Color.White;
                filterBtns[i].Location = new System.Drawing.Point(20 + i * 130, 78);
                filterBtns[i].Size = new System.Drawing.Size(120, 28);
                filterBtns[i].Text = filterTexts[i];
            }
            btnFilterScheduled.Click += new System.EventHandler(this.btnFilterScheduled_Click);
            btnFilterCompleted.Click += new System.EventHandler(this.btnFilterCompleted_Click);
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // dgvAppointments
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.BackgroundColor = System.Drawing.Color.White;
            this.dgvAppointments.Location = new System.Drawing.Point(20, 118);
            this.dgvAppointments.Size = new System.Drawing.Size(920, 260);
            this.dgvAppointments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellClick);

            // lblCurrentStatus
            this.lblCurrentStatus.AutoSize = true;
            this.lblCurrentStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblCurrentStatus.Location = new System.Drawing.Point(20, 393);
            this.lblCurrentStatus.Text = "Current Status: -";

            // lblNewStatus
            this.lblNewStatus.AutoSize = true;
            this.lblNewStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNewStatus.Location = new System.Drawing.Point(20, 423);
            this.lblNewStatus.Text = "Update Status To:";

            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.Location = new System.Drawing.Point(20, 441);
            this.cmbStatus.Size = new System.Drawing.Size(200, 26);

            // lblRemarks
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRemarks.Location = new System.Drawing.Point(240, 423);
            this.lblRemarks.Text = "Remarks / Notes:";

            // txtRemarks
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRemarks.Location = new System.Drawing.Point(240, 441);
            this.txtRemarks.Size = new System.Drawing.Size(400, 26);

            // btnUpdateStatus
            this.btnUpdateStatus.BackColor = System.Drawing.Color.FromArgb(0, 153, 76);
            this.btnUpdateStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdateStatus.ForeColor = System.Drawing.Color.White;
            this.btnUpdateStatus.Location = new System.Drawing.Point(660, 436);
            this.btnUpdateStatus.Size = new System.Drawing.Size(160, 36);
            this.btnUpdateStatus.Text = "💾 Update Status";
            this.btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(960, 500);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.btnFilterScheduled);
            this.Controls.Add(this.btnFilterCompleted);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.lblCurrentStatus);
            this.Controls.Add(this.lblNewStatus);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.btnUpdateStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DoctorAppointmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doctor - My Appointments";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DoctorAppointmentForm_FormClosed);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle, lblDoctorName, lblCurrentStatus, lblNewStatus, lblRemarks;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnUpdateStatus, btnFilterScheduled, btnFilterCompleted, btnRefresh;
        private System.Windows.Forms.DataGridView dgvAppointments;
    }
}