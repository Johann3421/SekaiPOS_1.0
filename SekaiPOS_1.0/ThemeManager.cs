using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public static class ThemeManager
    {
        // Colores predefinidos
        public static class AccentColors
        {
            public static readonly Color GreenNeon = Color.FromArgb(0, 255, 127);      // Verde Neón (Default)
            public static readonly Color ElectricBlue = Color.FromArgb(0, 191, 255);   // Azul Eléctrico
            public static readonly Color Purple = Color.FromArgb(138, 43, 226);        // Púrpura
            public static readonly Color Orange = Color.FromArgb(255, 140, 0);         // Naranja
            public static readonly Color Red = Color.FromArgb(220, 20, 60);            // Rojo
            public static readonly Color Cyan = Color.FromArgb(0, 255, 255);           // Cyan
        }

        // Colores del tema oscuro
        public static class DarkTheme
        {
            public static readonly Color Background = Color.FromArgb(15, 15, 15);
            public static readonly Color Surface = Color.FromArgb(25, 25, 25);
            public static readonly Color SurfaceVariant = Color.FromArgb(30, 30, 30);
            public static readonly Color TextPrimary = Color.White;
            public static readonly Color TextSecondary = Color.FromArgb(180, 180, 180);
            public static readonly Color Border = Color.FromArgb(50, 50, 50);
        }

        private static Color _currentAccentColor = AccentColors.GreenNeon;
        public static Color CurrentAccentColor
        {
            get => _currentAccentColor;
            set
            {
                _currentAccentColor = value;
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
            }
            catch
            {
                _currentAccentColor = AccentColors.GreenNeon;
            }
        }

        public static void SaveThemeToDatabase(DatabaseHelper db, Color accentColor)
        {
            try
            {
                string hex = ColorToHex(accentColor);
                db.UpdateThemeSettings("Dark", hex);
                _currentAccentColor = accentColor;
                OnThemeChanged?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar tema: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ApplyTheme(Form form)
        {
            if (form == null) return;

            form.BackColor = DarkTheme.Background;
            ApplyThemeToControls(form.Controls);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Aplicar tema según tipo de control
                if (control is Panel panel)
                {
                    ApplyThemeToPanel(panel);
                }
                else if (control is IconButton button)
                {
                    ApplyThemeToIconButton(button);
                }
                else if (control is Button btn)
                {
                    ApplyThemeToButton(btn);
                }
                else if (control is Label label)
                {
                    ApplyThemeToLabel(label);
                }
                else if (control is DataGridView dgv)
                {
                    ApplyThemeToDataGridView(dgv);
                }
                else if (control is TextBox textBox)
                {
                    ApplyThemeToTextBox(textBox);
                }
                else if (control is IconPictureBox iconPic)
                {
                    ApplyThemeToIconPictureBox(iconPic);
                }
                else if (control is TabControl tabControl)
                {
                    ApplyThemeToTabControl(tabControl);
                }

                // Recursivamente aplicar a controles hijos
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        private static void ApplyThemeToPanel(Panel panel)
        {
            // Mantener color de fondo específico si no es el default
            if (panel.BackColor == Color.Transparent ||
                panel.BackColor == Color.FromArgb(20, 20, 20) ||
                panel.BackColor == Color.FromArgb(25, 25, 25) ||
                panel.BackColor == Color.FromArgb(30, 30, 30))
            {
                // No cambiar, mantener el color de superficie
            }
        }

        private static void ApplyThemeToIconButton(IconButton button)
        {
            // Botones con color de acento
            if (button.BackColor == Color.FromArgb(0, 255, 127) || // Verde anterior
                button.BackColor == Color.FromArgb(0, 191, 255) || // Azul anterior
                button.BackColor == Color.FromArgb(138, 43, 226) || // Púrpura anterior
                button.BackColor == Color.FromArgb(255, 140, 0) || // Naranja anterior
                button.BackColor == Color.FromArgb(0, 200, 83))    // Verde botón agregar
            {
                button.BackColor = CurrentAccentColor;
                button.FlatAppearance.MouseOverBackColor = LightenColor(CurrentAccentColor, 20);
            }
            // Botones primarios
            else if (button.BackColor == Color.FromArgb(0, 120, 212))
            {
                button.BackColor = CurrentAccentColor;
                button.FlatAppearance.MouseOverBackColor = LightenColor(CurrentAccentColor, 20);
            }

            button.IconColor = Color.White;
            button.ForeColor = Color.White;
        }

        private static void ApplyThemeToButton(Button button)
        {
            if (button.BackColor == Color.FromArgb(0, 120, 212) ||
                button.BackColor == Color.FromArgb(0, 255, 127))
            {
                button.BackColor = CurrentAccentColor;
            }
        }

        private static void ApplyThemeToLabel(Label label)
        {
            // Labels con color de acento
            if (label.ForeColor == Color.FromArgb(0, 255, 127) ||
                label.ForeColor == Color.FromArgb(0, 120, 212))
            {
                label.ForeColor = CurrentAccentColor;
            }
        }

        private static void ApplyThemeToDataGridView(DataGridView dgv)
        {
            if (dgv.ColumnHeadersDefaultCellStyle.BackColor == Color.FromArgb(0, 120, 212) ||
                dgv.ColumnHeadersDefaultCellStyle.BackColor == Color.FromArgb(0, 255, 127))
            {
                dgv.ColumnHeadersDefaultCellStyle.BackColor = CurrentAccentColor;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = CurrentAccentColor;
            }

            if (dgv.DefaultCellStyle.SelectionBackColor == Color.FromArgb(0, 150, 255) ||
                dgv.DefaultCellStyle.SelectionBackColor == Color.FromArgb(0, 255, 127))
            {
                dgv.DefaultCellStyle.SelectionBackColor = LightenColor(CurrentAccentColor, 30);
            }
        }

        private static void ApplyThemeToTextBox(TextBox textBox)
        {
            textBox.BackColor = DarkTheme.SurfaceVariant;
            textBox.ForeColor = DarkTheme.TextPrimary;
        }

        private static void ApplyThemeToIconPictureBox(IconPictureBox iconPic)
        {
            // Iconos con color de acento
            if (iconPic.IconColor == Color.FromArgb(0, 255, 127) ||
                iconPic.IconColor == Color.FromArgb(0, 120, 212))
            {
                iconPic.IconColor = CurrentAccentColor;
            }
        }

        private static void ApplyThemeToTabControl(TabControl tabControl)
        {
            // Los tabs se manejan con DrawItem event
            tabControl.Invalidate();
        }

        // Utilidades de color
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
