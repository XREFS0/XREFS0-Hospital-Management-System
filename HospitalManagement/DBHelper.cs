// DBHelper.cs
// Hospital Management System - SQLite database helper
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace HospitalManagement
{
    public static class DBHelper
    {
        // SQLite database file lives next to the executable - zero configuration.
        private static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hospital.db");

        private static readonly string connectionString =
            new SqliteConnectionStringBuilder
            {
                DataSource = DbPath,
                // Foreign Keys enforcement handled via PRAGMA per connection
            }.ToString();

        public static string DatabasePath => DbPath;

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }

        private static void ApplyPragmas(SqliteConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }
        }

        private static void AddParameters(SqliteCommand cmd, SqliteParameter[] parameters)
        {
            if (parameters == null) return;
            foreach (var p in parameters)
            {
                // Normalize DateTime -> ISO strings so TEXT comparisons work reliably
                if (p.Value is DateTime dt)
                {
                    // Pure date (midnight) -> yyyy-MM-dd, otherwise full timestamp
                    if (dt.TimeOfDay == TimeSpan.Zero)
                        p.Value = dt.ToString("yyyy-MM-dd");
                    else
                        p.Value = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                cmd.Parameters.Add(p);
            }
        }

        /// <summary>
        /// Creates hospital.db + schema + seed data on first run.
        /// Safe to call on every startup (IF NOT EXISTS).
        /// </summary>
        public static void EnsureDatabase()
        {
            bool fresh = !File.Exists(DbPath);
            if (fresh)
            {
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
            }

            using (SqliteConnection conn = GetConnection())
            {
                conn.Open();
                ApplyPragmas(conn);
                using (SqliteTransaction tx = conn.BeginTransaction())
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;

                    cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    UserID      INTEGER PRIMARY KEY AUTOINCREMENT,
    Username    TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Role        TEXT NOT NULL CHECK (Role IN ('Admin','Doctor','Patient')),
    ReferenceID INTEGER NULL
);

CREATE TABLE IF NOT EXISTS Patients (
    PatientID INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName  TEXT NOT NULL,
    CNIC      TEXT NOT NULL UNIQUE,
    Contact   TEXT NOT NULL,
    Address   TEXT NOT NULL,
    CreatedAt TEXT DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS Doctors (
    DoctorID       INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName       TEXT NOT NULL,
    Specialization TEXT NOT NULL,
    Contact        TEXT NOT NULL,
    CreatedAt      TEXT DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS Appointments (
    AppointmentID   INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientID       INTEGER NOT NULL REFERENCES Patients(PatientID),
    DoctorID        INTEGER NOT NULL REFERENCES Doctors(DoctorID),
    AppointmentDate TEXT NOT NULL,
    AppointmentTime TEXT NOT NULL,
    Status          TEXT NOT NULL DEFAULT 'Scheduled'
                    CHECK (Status IN ('Scheduled','Completed','Cancelled','Closed')),
    Remarks         TEXT NULL,
    CreatedAt       TEXT DEFAULT (datetime('now','localtime')),
    CONSTRAINT UQ_Doctor_DateTime UNIQUE (DoctorID, AppointmentDate, AppointmentTime)
);

CREATE TABLE IF NOT EXISTS MedicalRecords (
    RecordID     INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientID    INTEGER NOT NULL REFERENCES Patients(PatientID),
    Diagnosis    TEXT NOT NULL,
    Treatment    TEXT NOT NULL,
    Prescription TEXT NOT NULL,
    RecordDate   TEXT NOT NULL,
    CreatedAt    TEXT DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS Billing (
    BillID        INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientID     INTEGER NOT NULL REFERENCES Patients(PatientID),
    AppointmentID INTEGER NOT NULL REFERENCES Appointments(AppointmentID),
    Amount        REAL NOT NULL,
    BillDate      TEXT NOT NULL,
    Status        TEXT NOT NULL DEFAULT 'Unpaid' CHECK (Status IN ('Paid','Unpaid')),
    CreatedAt     TEXT DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS Payments (
    PaymentID   INTEGER PRIMARY KEY AUTOINCREMENT,
    BillID      INTEGER NOT NULL REFERENCES Billing(BillID),
    AmountPaid  REAL NOT NULL,
    PaymentDate TEXT NOT NULL,
    CreatedAt   TEXT DEFAULT (datetime('now','localtime'))
);";
                    cmd.ExecuteNonQuery();

                    // Rich idempotent seed (runs on every startup):
                    // - Users uses OR IGNORE (Username is UNIQUE, PK not referenced elsewhere)
                    // - every other table uses WHERE NOT EXISTS guards, which - unlike
                    //   OR IGNORE - do not burn AUTOINCREMENT IDs, so Patient/Doctor/
                    //   Appointment/Bill IDs stay aligned for upgrades and fresh installs.
                    // Fresh installs get the full dataset; old small DBs get topped up.
                    cmd.CommandText = @"
INSERT OR IGNORE INTO Users (Username, PasswordHash, Role, ReferenceID) VALUES
('admin',     'admin123', 'Admin',   NULL),
('dr.ahmed',  'doc123',   'Doctor',  1),
('dr.sara',   'doc456',   'Doctor',  2),
('dr.bilal',  'doc789',   'Doctor',  3),
('dr.ayesha', 'doc321',   'Doctor',  4),
('dr.imran',  'doc654',   'Doctor',  5),
('dr.nadia',  'doc987',   'Doctor',  6),
('patient1',  'pat123',   'Patient', 1),
('patient2',  'pat456',   'Patient', 2),
('patient3',  'pat789',   'Patient', 3),
('patient4',  'pat321',   'Patient', 4),
('patient5',  'pat654',   'Patient', 5);

INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Ahmed Khan', 'Cardiology', '0300-1234567'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Ahmed Khan');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Sara Ali', 'Neurology', '0301-9876543'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Sara Ali');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Bilal Raza', 'Orthopedics', '0302-1122334'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Bilal Raza');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Ayesha Siddiqui', 'Pediatrics', '0321-4455667'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Ayesha Siddiqui');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Imran Sheikh', 'Dermatology', '0333-5566778'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Imran Sheikh');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Nadia Hussain', 'Gynecology', '0305-6677889'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Nadia Hussain');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Kamran Malik', 'General Surgery', '0345-7788990'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Kamran Malik');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Farah Javed', 'Ophthalmology', '0322-8899001'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Farah Javed');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Tariq Mehmood', 'ENT', '0308-9900112'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Tariq Mehmood');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Hina Shahid', 'Psychiatry', '0315-0011223'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Hina Shahid');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Faisal Qureshi', 'Urology', '0334-1122334'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Faisal Qureshi');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Mahnoor Asif', 'Dentistry', '0311-2233445'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Mahnoor Asif');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Adnan Farooq', 'Pulmonology', '0302-3344556'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Adnan Farooq');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Rabia Anwar', 'Endocrinology', '0346-4455667'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Rabia Anwar');
INSERT INTO Doctors (FullName, Specialization, Contact)
SELECT 'Dr. Shahid Iqbal', 'Gastroenterology', '0323-5566778'
WHERE NOT EXISTS (SELECT 1 FROM Doctors WHERE FullName = 'Dr. Shahid Iqbal');

INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Ali Hassan', '35202-1234567-1', '0311-1234567', 'House 5, Street 3, Rawalpindi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-1234567-1');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Fatima Malik', '35202-7654321-2', '0322-9876543', 'Block B, Satellite Town, Rawalpindi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-7654321-2');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Usman Tariq', '35202-1122334-3', '0333-1122334', 'F-8/2, Islamabad' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-1122334-3');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Hassan Raza', '35202-2233445-4', '0345-2233445', 'House 12, Gulberg III, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-2233445-4');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Amina Khan', '35202-3344556-5', '0321-3344556', 'DHA Phase 2, Karachi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-3344556-5');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Muhammad Asif', '35202-4455667-6', '0334-4455667', 'Model Town Link Road, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-4455667-6');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Zainab Ahmed', '35202-5566778-7', '0345-5566778', 'F-10/3, Islamabad' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-5566778-7');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Bilal Hussain', '35202-6677889-8', '0300-6677889', 'Saddar Bazaar, Rawalpindi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-6677889-8');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Maryam Nawaz', '35202-7788990-9', '0322-7788990', 'Bahria Town Phase 4, Rawalpindi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-7788990-9');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Ahmed Sheikh', '35202-8899001-0', '0333-8899001', 'North Nazimabad Block H, Karachi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-8899001-0');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Sanaullah Khan', '35202-9900112-1', '0346-9900112', 'University Road, Peshawar' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-9900112-1');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Iqra Fatima', '35202-0011223-2', '0301-0011223', 'Johar Town Block F, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-0011223-2');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Danish Ali', '35202-1122445-3', '0323-1122445', 'Clifton Block 5, Karachi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-1122445-3');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Hira Shah', '35202-2233556-4', '0335-2233556', 'E-11/2, Islamabad' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-2233556-4');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Kamran Akmal', '35202-3344667-5', '0347-3344667', 'Faisal Town Block C, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-3344667-5');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Saad Rehman', '35202-4455778-6', '0302-4455778', 'Scheme 33, Karachi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-4455778-6');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Noreen Akhtar', '35202-5566889-7', '0324-5566889', 'Chaklala Scheme III, Rawalpindi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-5566889-7');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Fahad Mustafa', '35202-6677990-8', '0336-6677990', 'Gulshan-e-Iqbal Block 7, Karachi' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-6677990-8');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Sadia Khanum', '35202-7788001-9', '0348-7788001', 'Wapda Town Block D, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-7788001-9');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Arsalan Ahmed', '35202-8899112-0', '0305-8899112', 'G-9/1, Islamabad' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-8899112-0');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Mehwish Hayat', '35202-9900223-1', '0325-9900223', 'DHA Phase 5, Lahore' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-9900223-1');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Younas Khan', '35202-1011334-2', '0337-1011334', 'Hayatabad Phase 2, Peshawar' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-1011334-2');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Bushra Bibi', '35202-2122445-3', '0349-2122445', 'Satellite Town Block 3, Quetta' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-2122445-3');
INSERT INTO Patients (FullName, CNIC, Contact, Address)
SELECT 'Khalid Mahmood', '35202-3233556-4', '0306-3233556', 'Peoples Colony No. 2, Faisalabad' WHERE NOT EXISTS (SELECT 1 FROM Patients WHERE CNIC = '35202-3233556-4');

INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 1, 1, '2025-05-10', '09:00', 'Scheduled', 'First consultation' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 1 AND AppointmentDate = '2025-05-10' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 2, 2, '2025-05-10', '10:00', 'Completed', 'Follow-up in 2 weeks' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 2 AND AppointmentDate = '2025-05-10' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 3, 3, '2025-05-11', '11:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 3 AND AppointmentDate = '2025-05-11' AND AppointmentTime = '11:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 4, 4, '2025-05-12', '09:30', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 4 AND AppointmentDate = '2025-05-12' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 5, 5, '2025-05-12', '10:30', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 5 AND AppointmentDate = '2025-05-12' AND AppointmentTime = '10:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 6, 1, '2025-05-12', '11:00', 'Scheduled', 'Chest pain evaluation' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 1 AND AppointmentDate = '2025-05-12' AND AppointmentTime = '11:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 7, 6, '2025-05-13', '09:00', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 6 AND AppointmentDate = '2025-05-13' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 8, 7, '2025-05-13', '10:00', 'Cancelled', 'Patient did not show up' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 7 AND AppointmentDate = '2025-05-13' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 9, 2, '2025-05-13', '11:30', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 2 AND AppointmentDate = '2025-05-13' AND AppointmentTime = '11:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 10, 8, '2025-05-14', '09:00', 'Completed', 'Eye examination' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 8 AND AppointmentDate = '2025-05-14' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 11, 9, '2025-05-14', '10:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 9 AND AppointmentDate = '2025-05-14' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 12, 3, '2025-05-14', '11:00', 'Completed', 'Knee injury' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 3 AND AppointmentDate = '2025-05-14' AND AppointmentTime = '11:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 13, 10, '2025-05-15', '09:30', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 10 AND AppointmentDate = '2025-05-15' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 14, 4, '2025-05-15', '10:30', 'Completed', 'Vaccination visit' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 4 AND AppointmentDate = '2025-05-15' AND AppointmentTime = '10:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 15, 11, '2025-05-16', '09:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 11 AND AppointmentDate = '2025-05-16' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 16, 12, '2025-05-16', '10:00', 'Completed', 'Root canal session 1' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 12 AND AppointmentDate = '2025-05-16' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 1, 5, '2025-05-17', '09:00', 'Completed', 'Skin allergy follow-up' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 5 AND AppointmentDate = '2025-05-17' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 2, 6, '2025-05-17', '10:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 6 AND AppointmentDate = '2025-05-17' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 3, 7, '2025-05-18', '09:30', 'Cancelled', 'Rescheduled by patient' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 7 AND AppointmentDate = '2025-05-18' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 4, 8, '2025-05-19', '11:00', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 8 AND AppointmentDate = '2025-05-19' AND AppointmentTime = '11:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 5, 9, '2025-05-19', '12:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 9 AND AppointmentDate = '2025-05-19' AND AppointmentTime = '12:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 6, 10, '2025-05-20', '09:00', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 10 AND AppointmentDate = '2025-05-20' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 7, 11, '2025-05-20', '10:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 11 AND AppointmentDate = '2025-05-20' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 8, 12, '2025-05-21', '09:30', 'Completed', 'Cleaning and polishing' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 12 AND AppointmentDate = '2025-05-21' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 9, 13, '2025-05-21', '10:30', 'Scheduled', 'Breathing difficulty' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 13 AND AppointmentDate = '2025-05-21' AND AppointmentTime = '10:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 10, 14, '2025-05-22', '09:00', 'Completed', 'Thyroid checkup' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 14 AND AppointmentDate = '2025-05-22' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 11, 15, '2025-05-22', '10:00', 'Scheduled', 'Stomach pain' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 15 AND AppointmentDate = '2025-05-22' AND AppointmentTime = '10:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 12, 1, '2025-05-23', '09:00', 'Completed', 'ECG and lipid profile' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 1 AND AppointmentDate = '2025-05-23' AND AppointmentTime = '09:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 13, 2, '2025-05-24', '11:00', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 2 AND AppointmentDate = '2025-05-24' AND AppointmentTime = '11:00');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 14, 3, '2025-06-02', '09:30', 'Scheduled', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 3 AND AppointmentDate = '2025-06-02' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 2, 1, '2025-06-07', '09:30', 'Closed', 'File closed after recovery' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 1 AND AppointmentDate = '2025-06-07' AND AppointmentTime = '09:30');
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Remarks)
SELECT 5, 2, '2025-06-08', '10:30', 'Completed', '' WHERE NOT EXISTS (SELECT 1 FROM Appointments WHERE DoctorID = 2 AND AppointmentDate = '2025-06-08' AND AppointmentTime = '10:30');

INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 1, 'Hypertension', 'Lifestyle changes, medication', 'Amlodipine 5mg', '2025-05-10'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 1 AND Diagnosis = 'Hypertension' AND RecordDate = '2025-05-10');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 2, 'Migraine', 'Pain relief, rest', 'Sumatriptan 50mg', '2025-05-10'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 2 AND Diagnosis = 'Migraine' AND RecordDate = '2025-05-10');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 3, 'Fractured wrist', 'Cast for 6 weeks', 'Ibuprofen 400mg', '2025-05-11'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 3 AND Diagnosis = 'Fractured wrist' AND RecordDate = '2025-05-11');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 4, 'Asthma', 'Inhaler therapy', 'Salbutamol inhaler', '2025-05-12'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 4 AND Diagnosis = 'Asthma' AND RecordDate = '2025-05-12');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 5, 'Eczema', 'Topical steroids', 'Hydrocortisone cream', '2025-05-12'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 5 AND Diagnosis = 'Eczema' AND RecordDate = '2025-05-12');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 6, 'Type 2 Diabetes', 'Diet control, medication', 'Metformin 500mg', '2025-05-13'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 6 AND Diagnosis = 'Type 2 Diabetes' AND RecordDate = '2025-05-13');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 7, 'Acute bronchitis', 'Antibiotics, rest', 'Azithromycin 250mg', '2025-05-13'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 7 AND Diagnosis = 'Acute bronchitis' AND RecordDate = '2025-05-13');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 8, 'Otitis media', 'Antibiotic ear drops', 'Ciprofloxacin drops', '2025-05-14'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 8 AND Diagnosis = 'Otitis media' AND RecordDate = '2025-05-14');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 9, 'Allergic rhinitis', 'Antihistamines', 'Cetirizine 10mg', '2025-05-14'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 9 AND Diagnosis = 'Allergic rhinitis' AND RecordDate = '2025-05-14');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 10, 'Cataract', 'Surgical evaluation', 'Eye drops pre-op', '2025-05-15'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 10 AND Diagnosis = 'Cataract' AND RecordDate = '2025-05-15');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 11, 'Anxiety disorder', 'Therapy sessions', 'Sertraline 25mg', '2025-05-15'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 11 AND Diagnosis = 'Anxiety disorder' AND RecordDate = '2025-05-15');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 12, 'Dental caries', 'Filling required', 'Amoxicillin 250mg', '2025-05-16'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 12 AND Diagnosis = 'Dental caries' AND RecordDate = '2025-05-16');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 13, 'Pneumonia', 'Hospital observation', 'Ceftriaxone injection', '2025-05-16'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 13 AND Diagnosis = 'Pneumonia' AND RecordDate = '2025-05-16');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 14, 'Hypothyroidism', 'Hormone replacement', 'Levothyroxine 50mcg', '2025-05-17'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 14 AND Diagnosis = 'Hypothyroidism' AND RecordDate = '2025-05-17');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 15, 'Gastritis', 'Antacids, diet plan', 'Omeprazole 20mg', '2025-05-18'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 15 AND Diagnosis = 'Gastritis' AND RecordDate = '2025-05-18');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 16, 'Lower back pain', 'Physiotherapy', 'Diclofenac gel', '2025-05-19'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 16 AND Diagnosis = 'Lower back pain' AND RecordDate = '2025-05-19');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 1, 'High cholesterol', 'Statin therapy', 'Atorvastatin 10mg', '2025-05-17'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 1 AND Diagnosis = 'High cholesterol' AND RecordDate = '2025-05-17');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 2, 'Tension headache', 'Stress management', 'Paracetamol 500mg', '2025-05-18'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 2 AND Diagnosis = 'Tension headache' AND RecordDate = '2025-05-18');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 4, 'Seasonal flu', 'Rest, fluids', 'Oseltamivir 75mg', '2025-05-20'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 4 AND Diagnosis = 'Seasonal flu' AND RecordDate = '2025-05-20');
INSERT INTO MedicalRecords (PatientID, Diagnosis, Treatment, Prescription, RecordDate)
SELECT 10, 'Post-op review', 'Healing normal', 'Vitamin C supplements', '2025-05-22'
WHERE NOT EXISTS (SELECT 1 FROM MedicalRecords WHERE PatientID = 10 AND Diagnosis = 'Post-op review' AND RecordDate = '2025-05-22');

INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 1, 1, 2500.00, '2025-05-10', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 1);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 2, 2, 3000.00, '2025-05-10', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 2);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 3, 3, 1500.00, '2025-05-11', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 3);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 4, 4, 2200.00, '2025-05-12', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 4);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 5, 5, 1800.00, '2025-05-12', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 5);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 6, 6, 4500.00, '2025-05-12', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 6);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 7, 7, 3200.00, '2025-05-13', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 7);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 9, 9, 1900.00, '2025-05-13', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 9);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 10, 10, 5200.00, '2025-05-14', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 10);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 11, 11, 2400.00, '2025-05-14', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 11);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 12, 12, 6100.00, '2025-05-14', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 12);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 13, 13, 1700.00, '2025-05-15', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 13);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 14, 14, 3800.00, '2025-05-15', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 14);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 15, 15, 2900.00, '2025-05-16', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 15);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 16, 16, 1500.00, '2025-05-16', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 16);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 1, 17, 4200.00, '2025-05-17', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 17);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 2, 18, 2600.00, '2025-05-17', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 18);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 4, 20, 4800.00, '2025-05-19', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 20);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 5, 21, 2100.00, '2025-05-19', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 21);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 6, 22, 3400.00, '2025-05-20', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 22);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 7, 23, 1950.00, '2025-05-20', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 23);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 8, 24, 5600.00, '2025-05-21', 'Paid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 24);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 9, 25, 2300.00, '2025-05-21', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 25);
INSERT INTO Billing (PatientID, AppointmentID, Amount, BillDate, Status)
SELECT 10, 26, 7100.00, '2025-05-22', 'Unpaid' WHERE NOT EXISTS (SELECT 1 FROM Billing WHERE AppointmentID = 26);

INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 2, 3000.00, '2025-05-10' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 2 AND AmountPaid = 3000.00 AND PaymentDate = '2025-05-10');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 4, 2200.00, '2025-05-12' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 4 AND AmountPaid = 2200.00 AND PaymentDate = '2025-05-12');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 5, 1800.00, '2025-05-12' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 5 AND AmountPaid = 1800.00 AND PaymentDate = '2025-05-12');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 7, 3200.00, '2025-05-13' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 7 AND AmountPaid = 3200.00 AND PaymentDate = '2025-05-13');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 9, 5200.00, '2025-05-14' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 9 AND AmountPaid = 5200.00 AND PaymentDate = '2025-05-14');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 11, 6100.00, '2025-05-14' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 11 AND AmountPaid = 6100.00 AND PaymentDate = '2025-05-14');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 13, 3800.00, '2025-05-15' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 13 AND AmountPaid = 3800.00 AND PaymentDate = '2025-05-15');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 15, 1500.00, '2025-05-16' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 15 AND AmountPaid = 1500.00 AND PaymentDate = '2025-05-16');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 16, 4200.00, '2025-05-17' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 16 AND AmountPaid = 4200.00 AND PaymentDate = '2025-05-17');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 18, 4800.00, '2025-05-19' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 18 AND AmountPaid = 4800.00 AND PaymentDate = '2025-05-19');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 20, 3400.00, '2025-05-20' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 20 AND AmountPaid = 3400.00 AND PaymentDate = '2025-05-20');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 22, 5600.00, '2025-05-21' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 22 AND AmountPaid = 5600.00 AND PaymentDate = '2025-05-21');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 1, 1000.00, '2025-05-11' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 1 AND AmountPaid = 1000.00 AND PaymentDate = '2025-05-11');
INSERT INTO Payments (BillID, AmountPaid, PaymentDate)
SELECT 6, 2000.00, '2025-05-13' WHERE NOT EXISTS (SELECT 1 FROM Payments WHERE BillID = 6 AND AmountPaid = 2000.00 AND PaymentDate = '2025-05-13');";
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }

        public static bool TestConnection()
        {
            try
            {
                EnsureDatabase();
                using (SqliteConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqliteParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqliteConnection conn = GetConnection())
                {
                    conn.Open();
                    ApplyPragmas(conn);
                    using (SqliteCommand cmd = new SqliteCommand(query, conn))
                    {
                        AddParameters(cmd, parameters);
                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, SqliteParameter[] parameters = null)
        {
            int result = 0;
            try
            {
                using (SqliteConnection conn = GetConnection())
                {
                    conn.Open();
                    ApplyPragmas(conn);
                    using (SqliteCommand cmd = new SqliteCommand(query, conn))
                    {
                        AddParameters(cmd, parameters);
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        public static object ExecuteScalar(string query, SqliteParameter[] parameters = null)
        {
            object result = null;
            try
            {
                using (SqliteConnection conn = GetConnection())
                {
                    conn.Open();
                    ApplyPragmas(conn);
                    using (SqliteCommand cmd = new SqliteCommand(query, conn))
                    {
                        AddParameters(cmd, parameters);
                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }

    public static class Session
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static int ReferenceID { get; set; }
    }
}
