// Program.cs
// Hospital Management System
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

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
