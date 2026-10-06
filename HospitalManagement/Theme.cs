// Theme.cs
// Hospital Management System - centralized professional theme.
// One call (Theme.ApplyForm(this)) gives every form the same modern look:
// navy headers, soft-gray background, flat semantic buttons, polished grids.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement
{
    public enum ThemeButtonKind
    {
        Primary,
        Accent,
        Success,
        Danger,
        Warning,
        Secondary
    }

    public static class Theme
    {
        // Palette
        public static readonly Color Primary = Color.FromArgb(27, 42, 78);
        public static readonly Color PrimaryDark = Color.FromArgb(17, 28, 54);
        public static readonly Color Accent = Color.FromArgb(13, 148, 136);
        public static readonly Color AccentDark = Color.FromArgb(10, 120, 110);
        public static readonly Color Success = Color.FromArgb(22, 163, 74);
        public static readonly Color SuccessDark = Color.FromArgb(17, 128, 58);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);
        public static readonly Color DangerDark = Color.FromArgb(178, 30, 30);
        public static readonly Color Warning = Color.FromArgb(217, 119, 6);
        public static readonly Color Secondary = Color.FromArgb(100, 116, 139);
        public static readonly Color SecondaryDark = Color.FromArgb(74, 88, 110);
        public static readonly Color Background = Color.FromArgb(241, 245, 249);
        public static readonly Color Card = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color TextDark = Color.FromArgb(15, 23, 42);
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        public static readonly Color GridAltRow = Color.FromArgb(248, 250, 252);

        /// <summary>Apply the full theme to a form (call after InitializeComponent).</summary>
        public static void ApplyForm(Form form)
        {
            if (form == null) return;
            form.BackColor = Background;
            form.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                Button btn = c as Button;
                if (btn != null)
                {
                    StyleButton(btn, DetectKind(btn));
                }
                else
                {
                    DataGridView grid = c as DataGridView;
                    if (grid != null)
                    {
                        StyleGrid(grid);
                    }
                    else
                    {
                        TextBox txt = c as TextBox;
                        if (txt != null)
                        {
                            txt.BackColor = Color.White;
                            txt.ForeColor = TextDark;
                            txt.BorderStyle = BorderStyle.FixedSingle;
                            txt.Font = new Font("Segoe UI", 10f);
                        }
                        else
                        {
                            ComboBox cmb = c as ComboBox;
                            if (cmb != null)
                            {
                                cmb.BackColor = Color.White;
                                cmb.ForeColor = TextDark;
                                cmb.Font = new Font("Segoe UI", 10f);
                                cmb.FlatStyle = FlatStyle.Standard;
                            }
                            else
                            {
                                DateTimePicker dtp = c as DateTimePicker;
                                if (dtp != null)
                                {
                                    dtp.Font = new Font("Segoe UI", 10f);
                                }
                                else
                                {
                                    Panel panel = c as Panel;
                                    if (panel != null)
                                    {
                                        StylePanel(panel);
                                        // Recurse AFTER styling so header labels turn white
                                        ApplyToControls(panel.Controls);
                                        continue;
                                    }
                                    else
                                    {
                                        Label lbl = c as Label;
                                        if (lbl != null)
                                            StyleLabel(lbl);
                                    }
                                }
                            }
                        }
                    }
                }

                if (c.HasChildren)
                    ApplyToControls(c.Controls);
            }
        }

        private static void StylePanel(Panel panel)
        {
            // Top-docked bars become the navy brand header
            if (panel.Dock == DockStyle.Top)
            {
                panel.BackColor = Primary;
                foreach (Control child in panel.Controls)
                {
                    Label lbl = child as Label;
                    if (lbl != null)
                    {
                        lbl.ForeColor = Color.White;
                        if (lbl.Font != null && lbl.Font.Size < 13f)
                            lbl.Font = new Font("Segoe UI", lbl.Font.Size, FontStyle.Bold);
                    }
                }
            }
            else if (panel.BorderStyle == BorderStyle.FixedSingle)
            {
                // Stat/card containers: light card look
                if (panel.BackColor.ToArgb() != Card.ToArgb())
                    panel.BackColor = Card;
            }
        }

        private static void StyleLabel(Label lbl)
        {
            if (lbl == null) return;

            // Full-width top header bars -> navy brand bar (unifies legacy
            // per-form colors: brown/green/purple/teal -> one professional navy)
            if (lbl.Dock == DockStyle.Top)
            {
                lbl.BackColor = Primary;
                lbl.ForeColor = Color.White;
                lbl.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
                return;
            }

            // Dashboard stat cards: keep their accent text color, ensure card look
            if (lbl.BorderStyle == BorderStyle.FixedSingle && lbl.Parent != null)
            {
                lbl.BackColor = Card;
                if (lbl.Font == null || lbl.Font.Size < 9.5f)
                    lbl.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
        }

        /// <summary>Decide button color from its name/text (Add=green, Delete/Cancel=danger...).</summary>
        private static ThemeButtonKind DetectKind(Button btn)
        {
            string n = (btn.Name ?? "").ToLowerInvariant();
            string t = (btn.Text ?? "").ToLowerInvariant();

            // Filters/search are informational, never destructive (check first:
            // "btnFilterCancelled" contains "cancel")
            if (n.Contains("filter") || n.Contains("search") || n.Contains("view") || t.Contains("search"))
                return ThemeButtonKind.Accent;
            if (n.Contains("refresh") || n.Contains("clear"))
                return ThemeButtonKind.Secondary;
            if (n.Contains("delete") || n.Contains("logout") || n.Contains("close") || n.Contains("exit")
                || n == "btncancel" || t == "cancel" || t == "logout" || t == "close" || t == "exit" || t == "delete")
                return ThemeButtonKind.Danger;
            if (n.Contains("add") || n.Contains("generate") || n.Contains("record") || n.Contains("save") || n.Contains("pay"))
                return ThemeButtonKind.Success;
            if (t.Contains("login"))
                return ThemeButtonKind.Primary;
            if (n.Contains("update"))
                return ThemeButtonKind.Primary;
            return ThemeButtonKind.Primary;
        }

        public static void StyleButton(Button btn, ThemeButtonKind kind)
        {
            if (btn == null) return;
            Color back, hover;
            switch (kind)
            {
                case ThemeButtonKind.Success: back = Success; hover = SuccessDark; break;
                case ThemeButtonKind.Danger: back = Danger; hover = DangerDark; break;
                case ThemeButtonKind.Accent: back = Accent; hover = AccentDark; break;
                case ThemeButtonKind.Warning: back = Warning; hover = Warning; break;
                case ThemeButtonKind.Secondary: back = Secondary; hover = SecondaryDark; break;
                default: back = Primary; hover = PrimaryDark; break;
            }
            btn.BackColor = back;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.FlatAppearance.MouseDownBackColor = hover;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            float size = btn.Font != null ? btn.Font.Size : 9.5f;
            if (size < 9.5f) size = 9.5f;
            FontStyle style = (kind == ThemeButtonKind.Secondary) ? FontStyle.Regular : FontStyle.Bold;
            btn.Font = new Font("Segoe UI", size, style);
        }

        public static void StyleGrid(DataGridView grid)
        {
            if (grid == null) return;
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Card;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Border;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowTemplate.Height = 30;
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            var header = new DataGridViewCellStyle
            {
                BackColor = Primary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.False
            };
            grid.ColumnHeadersDefaultCellStyle = header;

            var cells = new DataGridViewCellStyle
            {
                BackColor = Card,
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 9.5f),
                SelectionBackColor = Accent,
                SelectionForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 4, 0)
            };
            grid.DefaultCellStyle = cells;

            var alt = new DataGridViewCellStyle
            {
                BackColor = GridAltRow,
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 9.5f),
                SelectionBackColor = Accent,
                SelectionForeColor = Color.White
            };
            grid.AlternatingRowsDefaultCellStyle = alt;
        }
    }
}
