using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public static class ThemeManager
    {
        public static class AccentColors
        {
            public static readonly Color GreenNeon = Color.FromArgb(0, 255, 127);
            public static readonly Color ElectricBlue = Color.FromArgb(0, 191, 255);
            public static readonly Color Purple = Color.FromArgb(138, 43, 226);
            public static readonly Color Orange = Color.FromArgb(255, 140, 0);
            public static readonly Color Red = Color.FromArgb(220, 20, 60);
            public static readonly Color Cyan = Color.FromArgb(0, 255, 255);
        }

        public static class DarkTheme
        {
            public static readonly Color Background = Color.FromArgb(15, 15, 15);
            public static readonly Color Surface = Color.FromArgb(25, 25, 25);
            public static readonly Color SurfaceVariant = Color.FromArgb(30, 30, 30);
            public static readonly Color TextPrimary = Color.White;
            public static readonly Color TextSecondary = Color.FromArgb(180, 180, 180);
            public static readonly Color Border = Color.FromArgb(50, 50, 50);
        }

        public static class LightTheme
        {
            public static readonly Color Background = Color.FromArgb(245, 245, 245);
            public static readonly Color Surface = Color.White;
            public static readonly Color SurfaceVariant = Color.FromArgb(250, 250, 250);
            public static readonly Color TextPrimary = Color.FromArgb(33, 33, 33);
            public static readonly Color TextSecondary = Color.FromArgb(100, 100, 100);
            public static readonly Color Border = Color.FromArgb(224, 224, 224);
        }

        private static Color _currentAccentColor = AccentColors.GreenNeon;
        private static bool _isDarkTheme = true;

        public static Color CurrentAccentColor
        {
            get => _currentAccentColor;
            set
            {
                _currentAccentColor = value;
                OnThemeChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                _isDarkTheme = value;
                OnThemeChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler? OnThemeChanged;

        public static void LoadThemeFromDatabase(DatabaseHelper db)
        {
            try
            {
                var (theme, accentColorHex) = db.GetThemeSettings();
                _currentAccentColor = ColorFromHex(accentColorHex) ?? AccentColors.GreenNeon;
                _isDarkTheme = theme.ToLower() == "dark";
            }
            catch
            {
                _currentAccentColor = AccentColors.GreenNeon;
                _isDarkTheme = true;
            }
        }

        public static void SaveThemeToDatabase(DatabaseHelper db, Color accentColor)
        {
            try
            {
                string hex = ColorToHex(accentColor);
                string themeName = _isDarkTheme ? "Dark" : "Light";
                db.UpdateThemeSettings(themeName, hex);
                _currentAccentColor = accentColor;
                OnThemeChanged?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar tema: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ToggleTheme(DatabaseHelper db)
        {
            _isDarkTheme = !_isDarkTheme;
            SaveThemeToDatabase(db, _currentAccentColor);
        }

        public static void ApplyTheme(Form form)
        {
            if (form == null) return;

            form.BackColor = _isDarkTheme ? DarkTheme.Background : LightTheme.Background;
            ApplyThemeToControls(form.Controls);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            var textColor = _isDarkTheme ? DarkTheme.TextPrimary : LightTheme.TextPrimary;
            var surfaceColor = _isDarkTheme ? DarkTheme.Surface : LightTheme.Surface;
            var surfaceVariant = _isDarkTheme ? DarkTheme.SurfaceVariant : LightTheme.SurfaceVariant;

            foreach (Control control in controls)
            {
                if (control is Panel panel)
                {
                    if (panel.BackColor == DarkTheme.Background || panel.BackColor == LightTheme.Background)
                        panel.BackColor = _isDarkTheme ? DarkTheme.Background : LightTheme.Background;
                    else if (panel.BackColor == DarkTheme.Surface || panel.BackColor == LightTheme.Surface)
                        panel.BackColor = surfaceColor;
                    else if (panel.BackColor == DarkTheme.SurfaceVariant || panel.BackColor == LightTheme.SurfaceVariant)
                        panel.BackColor = surfaceVariant;
                }
                else if (control is IconButton button)
                {
                    ApplyThemeToIconButton(button);
                }
                else if (control is Label label)
                {
                    if (label.ForeColor == DarkTheme.TextPrimary || label.ForeColor == LightTheme.TextPrimary ||
                        label.ForeColor == Color.White || label.ForeColor == Color.Black)
                        label.ForeColor = textColor;
                }
                else if (control is DataGridView dgv)
                {
                    ApplyThemeToDataGridView(dgv);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = surfaceVariant;
                    textBox.ForeColor = textColor;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        private static void ApplyThemeToIconButton(IconButton button)
        {
            if (button.BackColor == Color.FromArgb(0, 255, 127) || 
                button.BackColor == Color.FromArgb(0, 191, 255) || 
                button.BackColor == Color.FromArgb(138, 43, 226) || 
                button.BackColor == Color.FromArgb(255, 140, 0) || 
                button.BackColor == Color.FromArgb(0, 200, 83) ||
                button.BackColor == Color.FromArgb(0, 120, 212))
            {
                button.BackColor = CurrentAccentColor;
                button.FlatAppearance.MouseOverBackColor = LightenColor(CurrentAccentColor, 20);
            }
            
            button.IconColor = Color.White;
            button.ForeColor = Color.White;
        }

        private static void ApplyThemeToDataGridView(DataGridView dgv)
        {
            var surfaceColor = _isDarkTheme ? DarkTheme.Surface : LightTheme.Surface;
            var textColor = _isDarkTheme ? DarkTheme.TextPrimary : LightTheme.TextPrimary;

            dgv.BackgroundColor = surfaceColor;
            dgv.ForeColor = textColor;
            dgv.GridColor = _isDarkTheme ? DarkTheme.Border : LightTheme.Border;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = CurrentAccentColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = CurrentAccentColor;

            dgv.DefaultCellStyle.BackColor = surfaceColor;
            dgv.DefaultCellStyle.ForeColor = textColor;
            dgv.DefaultCellStyle.SelectionBackColor = LightenColor(CurrentAccentColor, 30);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        public static Color LightenColor(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(color.A, r, g, b);
        }

        public static Color DarkenColor(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(color.A, r, g, b);
        }

        public static string ColorToHex(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color? ColorFromHex(string hex)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hex)) return null;
                hex = hex.TrimStart('#');
                
                if (hex.Length == 6)
                {
                    int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                    return Color.FromArgb(r, g, b);
                }
            }
            catch { }
            
            return null;
        }

        public static string GetColorName(Color color)
        {
            if (color == AccentColors.GreenNeon) return "Verde Neón";
            if (color == AccentColors.ElectricBlue) return "Azul Eléctrico";
            if (color == AccentColors.Purple) return "Púrpura";
            if (color == AccentColors.Orange) return "Naranja";
            if (color == AccentColors.Red) return "Rojo";
            if (color == AccentColors.Cyan) return "Cyan";
            return "Personalizado";
        }
    }
}
