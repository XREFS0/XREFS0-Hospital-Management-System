// AppointmentForm.Designer.cs
namespace HospitalManagement
{
    partial class AppointmentForm
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
            this.lblDoctor = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFilterScheduled = new System.Windows.Forms.Button();
            this.btnFilterCompleted = new System.Windows.Forms.Button();
            this.btnFilterCancelled = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(153, 76, 0);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(1000, 45);
            this.lblTitle.Text = "  📅 Appointment Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Row 1: Patient, Doctor
            this.lblPatient.AutoSize = true; this.lblPatient.Location = new System.Drawing.Point(20, 58); this.lblPatient.Text = "Patient:";
            this.cmbPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPatient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPatient.Location = new System.Drawing.Point(20, 75); this.cmbPatient.Size = new System.Drawing.Size(220, 26);

            this.lblDoctor.AutoSize = true; this.lblDoctor.Location = new System.Drawing.Point(260, 58); this.lblDoctor.Text = "Doctor:";
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDoctor.Location = new System.Drawing.Point(260, 75); this.cmbDoctor.Size = new System.Drawing.Size(220, 26);

            // Row 2: Date, Time, Status
            this.lblDate.AutoSize = true; this.lblDate.Location = new System.Drawing.Point(20, 115); this.lblDate.Text = "Date:";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(20, 132); this.dtpDate.Size = new System.Drawing.Size(160, 26);

            this.lblTime.AutoSize = true; this.lblTime.Location = new System.Drawing.Point(200, 115); this.lblTime.Text = "Time (HH:MM):";
            this.txtTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTime.Location = new System.Drawing.Point(200, 132); this.txtTime.Size = new System.Drawing.Size(100, 26);
            this.txtTime.Text = "09:00";

            this.lblStatus.AutoSize = true; this.lblStatus.Location = new System.Drawing.Point(320, 115); this.lblStatus.Text = "Status:";
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.Location = new System.Drawing.Point(320, 132); this.cmbStatus.Size = new System.Drawing.Size(160, 26);

            // Row 3: Remarks
            this.lblRemarks.AutoSize = true; this.lblRemarks.Location = new System.Drawing.Point(20, 172); this.lblRemarks.Text = "Remarks:";
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRemarks.Location = new System.Drawing.Point(20, 189); this.txtRemarks.Size = new System.Drawing.Size(460, 26);

            // Buttons
            System.Windows.Forms.Button[] actBtns = { btnAdd, btnUpdate, btnCancel, btnClear };
            string[] actTexts = { "Create", "Update", "Cancel Appt", "Clear" };
            string[] actColors = { "0,153,76", "0,102,204", "180,60,0", "80,80,80" };
            for (int i = 0; i < actBtns.Length; i++)
            {
                var p = actColors[i].Split(',');
                actBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                actBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                actBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                actBtns[i].ForeColor = System.Drawing.Color.White;
                actBtns[i].Location = new System.Drawing.Point(20 + i * 120, 230);
                actBtns[i].Size = new System.Drawing.Size(110, 32);
                actBtns[i].Text = actTexts[i];
            }
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // Filter buttons
            System.Windows.Forms.Button[] filterBtns = { btnFilterScheduled, btnFilterCompleted, btnFilterCancelled, btnRefresh };
            string[] filterTexts = { "Scheduled", "Completed", "Cancelled", "↻ All" };
            string[] filterColors = { "0,120,180", "0,140,70", "180,50,0", "80,80,120" };
            for (int i = 0; i < filterBtns.Length; i++)
            {
                var p = filterColors[i].Split(',');
                filterBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                filterBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                filterBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                filterBtns[i].ForeColor = System.Drawing.Color.White;
                filterBtns[i].Location = new System.Drawing.Point(20 + i * 120, 278);
                filterBtns[i].Size = new System.Drawing.Size(110, 28);
                filterBtns[i].Text = filterTexts[i];
            }
            btnFilterScheduled.Click += new System.EventHandler(this.btnFilterScheduled_Click);
            btnFilterCompleted.Click += new System.EventHandler(this.btnFilterCompleted_Click);
            btnFilterCancelled.Click += new System.EventHandler(this.btnFilterCancelled_Click);
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.BackgroundColor = System.Drawing.Color.White;
            this.dgvAppointments.Location = new System.Drawing.Point(20, 320);
            this.dgvAppointments.Size = new System.Drawing.Size(960, 280);
            this.dgvAppointments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellClick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 625);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPatient); this.Controls.Add(this.cmbPatient);
            this.Controls.Add(this.lblDoctor); this.Controls.Add(this.cmbDoctor);
            this.Controls.Add(this.lblDate); this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblTime); this.Controls.Add(this.txtTime);
            this.Controls.Add(this.lblStatus); this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblRemarks); this.Controls.Add(this.txtRemarks);
            foreach (var b in actBtns) this.Controls.Add(b);
            foreach (var b in filterBtns) this.Controls.Add(b);
            this.Controls.Add(this.dgvAppointments);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AppointmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblPatient, lblDoctor, lblDate, lblTime, lblStatus, lblRemarks;
        private System.Windows.Forms.ComboBox cmbPatient, cmbDoctor, cmbStatus;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtTime, txtRemarks;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnCancel, btnClear;
        private System.Windows.Forms.Button btnFilterScheduled, btnFilterCompleted, btnFilterCancelled, btnRefresh;
        private System.Windows.Forms.DataGridView dgvAppointments;
    }
}
