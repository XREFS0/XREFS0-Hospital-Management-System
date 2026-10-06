// DoctorForm.Designer.cs
namespace HospitalManagement
{
    partial class DoctorForm
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblSpecialization = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSpecialization = new System.Windows.Forms.TextBox();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dgvDoctors = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoctors)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(900, 45);
            this.lblTitle.Text = "  🩺 Doctor Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            string[] lblTexts = { "Full Name:", "Specialization:", "Contact:" };
            System.Windows.Forms.Label[] lblArr = { lblName, lblSpecialization, lblContact };
            System.Windows.Forms.TextBox[] txtArr = { txtName, txtSpecialization, txtContact };

            for (int i = 0; i < lblArr.Length; i++)
            {
                lblArr[i].AutoSize = true;
                lblArr[i].Font = new System.Drawing.Font("Segoe UI", 9F);
                lblArr[i].Location = new System.Drawing.Point(20, 60 + i * 50);
                lblArr[i].Text = lblTexts[i];

                txtArr[i].Font = new System.Drawing.Font("Segoe UI", 10F);
                txtArr[i].Location = new System.Drawing.Point(20, 78 + i * 50);
                txtArr[i].Size = new System.Drawing.Size(280, 26);
            }

            System.Windows.Forms.Button[] actionBtns = { btnAdd, btnUpdate, btnDelete, btnClear };
            string[] btnTexts = { "Add Doctor", "Update", "Delete", "Clear" };
            string[] btnColors = { "0,153,76", "0,102,204", "180,0,0", "80,80,80" };
            for (int i = 0; i < actionBtns.Length; i++)
            {
                var p = btnColors[i].Split(',');
                actionBtns[i].BackColor = System.Drawing.Color.FromArgb(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                actionBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                actionBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                actionBtns[i].ForeColor = System.Drawing.Color.White;
                actionBtns[i].Location = new System.Drawing.Point(20 + i * 120, 220);
                actionBtns[i].Size = new System.Drawing.Size(110, 35);
                actionBtns[i].Text = btnTexts[i];
            }
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 275);
            this.lblSearch.Text = "Search:";

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(70, 272);
            this.txtSearch.Size = new System.Drawing.Size(250, 26);

            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(0, 153, 153);
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(330, 270);
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(100, 100, 150);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(420, 270);
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.Text = "↻ Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.dgvDoctors.AllowUserToAddRows = false;
            this.dgvDoctors.AllowUserToDeleteRows = false;
            this.dgvDoctors.BackgroundColor = System.Drawing.Color.White;
            this.dgvDoctors.Location = new System.Drawing.Point(20, 315);
            this.dgvDoctors.Size = new System.Drawing.Size(860, 250);
            this.dgvDoctors.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDoctors_CellClick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 590);
            this.Controls.Add(this.lblTitle);
            foreach (var l in lblArr) this.Controls.Add(l);
            foreach (var t in txtArr) this.Controls.Add(t);
            foreach (var b in actionBtns) this.Controls.Add(b);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvDoctors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DoctorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Doctor Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDoctors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblName, lblSpecialization, lblContact, lblSearch;
        private System.Windows.Forms.TextBox txtName, txtSpecialization, txtContact, txtSearch;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnDelete, btnClear, btnSearch, btnRefresh;
        private System.Windows.Forms.DataGridView dgvDoctors;
    }
}
