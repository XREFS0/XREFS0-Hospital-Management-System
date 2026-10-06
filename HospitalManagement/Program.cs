// Program.cs
// Hospital Management System
// Author: XREFS0

using System;
using System.Windows.Forms;

namespace HospitalManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // SQLite database (hospital.db) is created automatically on first run.
            DBHelper.EnsureDatabase();

            Application.Run(new LoginForm());
        }
    }
}
