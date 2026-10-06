// MedicalRecordForm.Designer.cs
namespace HospitalManagement
{
    partial class MedicalRecordForm
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
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.lblTreatment = new System.Windows.Forms.Label();
            this.lblPrescription = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.txtTreatment = new System.Windows.Forms.TextBox();
            this.txtPrescription = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnViewByPatient = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(76, 0, 153);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(980, 45);
            this.lblTitle.Text = "  📋 Medical Records";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblPatient.AutoSize = true; this.lblPatient.Location = new System.Drawing.Point(20, 58); this.lblPatient.Text = "Patient:";
            this.cmbPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPatient.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPatient.Location = new System.Drawing.Point(20, 75); this.cmbPatient.Size = new System.Drawing.Size(280, 26);

            this.lblDate.AutoSize = true; this.lblDate.Location = new System.Drawing.Point(320, 58); this.lblDate.Text = "Record Date:";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(320, 75); this.dtpDate.Size = new System.Drawing.Size(160, 26);

            string[] lTexts = { "Diagnosis:", "Treatment:", "Prescription:" };
            System.Windows.Forms.Label[] lArr = { lblDiagnosis, lblTreatment, lblPrescription };
            System.Windows.Forms.TextBox[] tArr = { txtDiagnosis, txtTreatment, txtPrescription };
            for (int i = 0; i < lArr.Length; i++)
            {
                lArr[i].AutoSize = true; lArr[i].Location = new System.Drawing.Point(20, 115 + i * 50); lArr[i].Text = lTexts[i];
                tArr[i].Font = new System.Drawing.Font("Segoe UI", 10F);
                tArr[i].Location = new System.Drawing.Point(20, 132 + i * 50); tArr[i].Size = new System.Drawing.Size(460, 26);
            }

            System.Windows.Forms.Button[] btns = { btnAdd, btnUpdate, btnViewByPatient, btnRefresh, btnClear };
            string[] bTexts = { "Add Record", "Update", "Filter by Patient", "↻ Refresh", "Clear" };
            string[] bColors = { "0,153,76", "0,102,204", "76,0,153", "80,80,120", "80,80,80" };
            for (int i = 0; i < btns.Length; i++)
            {
                var p = bColors[i].Split(',');
                btns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                btns[i].ForeColor = System.Drawing.Color.White;
                btns[i].Location = new System.Drawing.Point(20 + i * 130, 285);
                btns[i].Size = new System.Drawing.Size(120, 32);
                btns[i].Text = bTexts[i];
            }
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            btnViewByPatient.Click += new System.EventHandler(this.btnViewByPatient_Click);
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecords.Location = new System.Drawing.Point(20, 335);
            this.dgvRecords.Size = new System.Drawing.Size(940, 270);
            this.dgvRecords.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecords_CellClick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(980, 625);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPatient); this.Controls.Add(this.cmbPatient);
            this.Controls.Add(this.lblDate); this.Controls.Add(this.dtpDate);
            foreach (var l in lArr) this.Controls.Add(l);
            foreach (var t in tArr) this.Controls.Add(t);
            foreach (var b in btns) this.Controls.Add(b);
            this.Controls.Add(this.dgvRecords);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MedicalRecordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Medical Records";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblPatient, lblDiagnosis, lblTreatment, lblPrescription, lblDate;
        private System.Windows.Forms.ComboBox cmbPatient;
        private System.Windows.Forms.TextBox txtDiagnosis, txtTreatment, txtPrescription;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnViewByPatient, btnRefresh, btnClear;
        private System.Windows.Forms.DataGridView dgvRecords;
    }
}
