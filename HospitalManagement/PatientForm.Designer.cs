// PatientForm.Designer.cs
namespace HospitalManagement
{
    partial class PatientForm
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
            this.lblCNIC = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCNIC = new System.Windows.Forms.TextBox();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dgvPatients = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatients)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Size = new System.Drawing.Size(950, 45);
            this.lblTitle.Text = "  👥 Patient Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            int leftCol = 20, rightCol = 500;
            System.Windows.Forms.Label[] labels = { lblName, lblCNIC, lblContact, lblAddress };
            string[] labelTexts = { "Full Name:", "CNIC / ID:", "Contact:", "Address:" };
            System.Windows.Forms.TextBox[] fields = { txtName, txtCNIC, txtContact, txtAddress };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].AutoSize = true;
                labels[i].Font = new System.Drawing.Font("Segoe UI", 9F);
                labels[i].Location = new System.Drawing.Point(leftCol, 60 + i * 50);
                labels[i].Text = labelTexts[i];

                fields[i].Font = new System.Drawing.Font("Segoe UI", 10F);
                fields[i].Location = new System.Drawing.Point(leftCol, 78 + i * 50);
                fields[i].Size = new System.Drawing.Size(250, 26);
            }
            txtAddress.Size = new System.Drawing.Size(400, 26);

            System.Windows.Forms.Button[] actionBtns = { btnAdd, btnUpdate, btnDelete, btnClear };
            string[] btnTexts = { "Add Patient", "Update", "Delete", "Clear" };
            string[] btnColors = { "0,153,76", "0,102,204", "180,0,0", "80,80,80" };
            for (int i = 0; i < actionBtns.Length; i++)
            {
                var p = btnColors[i].Split(',');
                actionBtns[i].BackColor = System.Drawing.Color.FromArgb(
                    int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]));
                actionBtns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                actionBtns[i].Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                actionBtns[i].ForeColor = System.Drawing.Color.White;
                actionBtns[i].Location = new System.Drawing.Point(20 + i * 120, 270);
                actionBtns[i].Size = new System.Drawing.Size(110, 35);
                actionBtns[i].Text = btnTexts[i];
            }
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSearch.Location = new System.Drawing.Point(20, 325);
            this.lblSearch.Text = "Search:";

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(70, 322);
            this.txtSearch.Size = new System.Drawing.Size(250, 26);

            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(0, 153, 153);
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(330, 320);
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(100, 100, 150);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(420, 320);
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.Text = "↻ Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.dgvPatients.AllowUserToAddRows = false;
            this.dgvPatients.AllowUserToDeleteRows = false;
            this.dgvPatients.BackgroundColor = System.Drawing.Color.White;
            this.dgvPatients.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPatients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPatients.Location = new System.Drawing.Point(20, 365);
            this.dgvPatients.Size = new System.Drawing.Size(900, 280);
            this.dgvPatients.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPatients_CellClick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(950, 670);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCNIC);
            this.Controls.Add(this.txtCNIC);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvPatients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PatientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblName, lblCNIC, lblContact, lblAddress, lblSearch;
        private System.Windows.Forms.TextBox txtName, txtCNIC, txtContact, txtAddress, txtSearch;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnDelete, btnClear, btnSearch, btnRefresh;
        private System.Windows.Forms.DataGridView dgvPatients;
    }
}
