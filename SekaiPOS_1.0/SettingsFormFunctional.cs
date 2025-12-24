using FontAwesome.Sharp;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SekaiPOS_1._0
{
    public class SettingsFormFunctional : Form
    {
        private DatabaseHelper db;
        private TabControl tabControl;
        private TextBox txtStoreName, txtAddress, txtPhone, txtReceiptHeader, txtReceiptFooter;
        private NumericUpDown numTax;
        private DataGridView dgvUsers;

        public SettingsFormFunctional(DatabaseHelper database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            db = database;
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.Padding = new Padding(20);

            var lblTitle = new Label()
            {
                Text = "Configuraci�n del Sistema",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            tabControl = new TabControl()
            {
                Location = new Point(20, 70),
                Size = new Size(1100, 550),
                Font = new Font("Segoe UI", 10F),
                DrawMode = TabDrawMode.OwnerDrawFixed,
                ItemSize = new Size(200, 50)
            };
            tabControl.DrawItem += TabControl_DrawItem;

            var tabGeneral = new TabPage("General");
            tabGeneral.BackColor = Color.FromArgb(20, 20, 20);
            CreateGeneralTab(tabGeneral);

            var tabUsers = new TabPage("Usuarios");
            tabUsers.BackColor = Color.FromArgb(20, 20, 20);
            CreateUsersTab(tabUsers);

            var tabAppearance = new TabPage("Apariencia");
            tabAppearance.BackColor = Color.FromArgb(20, 20, 20);
            CreateAppearanceTab(tabAppearance);

            var tabAbout = new TabPage("Acerca de");
            tabAbout.BackColor = Color.FromArgb(20, 20, 20);
            CreateAboutTab(tabAbout);

            tabControl.TabPages.Add(tabGeneral);
            tabControl.TabPages.Add(tabUsers);
            tabControl.TabPages.Add(tabAppearance);
            tabControl.TabPages.Add(tabAbout);

            this.Controls.Add(lblTitle);
            this.Controls.Add(tabControl);
        }

        private void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            var tabPage = tabControl.TabPages[e.Index];
            var tabBounds = tabControl.GetTabRect(e.Index);

            var backColor = e.State == DrawItemState.Selected
                ? Color.FromArgb(0, 120, 212)
                : Color.FromArgb(30, 30, 30);

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabBounds);
            }

            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(tabPage.Text, e.Font!, textBrush, tabBounds, sf);
            }
        }

        private void LoadSettings()
        {
            try
            {
                var settings = db.GetSettings();
                if (txtStoreName != null) txtStoreName.Text = settings.StoreName;
                if (txtAddress != null) txtAddress.Text = settings.Address;
                if (txtPhone != null) txtPhone.Text = settings.Phone;
                if (numTax != null) numTax.Value = settings.TaxRate * 100;
                if (txtReceiptHeader != null) txtReceiptHeader.Text = settings.ReceiptHeader;
                if (txtReceiptFooter != null) txtReceiptFooter.Text = settings.ReceiptFooter;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar configuraci�n: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateGeneralTab(TabPage tab)
        {
            var panel = new Panel()
            {
                Size = new Size(1050, 450),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20),
                AutoScroll = true
            };

            var lblStoreName = new Label()
            {
                Text = "Nombre de la Tienda:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtStoreName = new TextBox()
            {
                Location = new Point(20, 50),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblAddress = new Label()
            {
                Text = "Direcci�n:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 90),
                AutoSize = true
            };

            txtAddress = new TextBox()
            {
                Location = new Point(20, 120),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblPhone = new Label()
            {
                Text = "Tel�fono:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 160),
                AutoSize = true
            };

            txtPhone = new TextBox()
            {
                Location = new Point(20, 190),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTax = new Label()
            {
                Text = "IVA (%):",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 230),
                AutoSize = true
            };

            numTax = new NumericUpDown()
            {
                Location = new Point(20, 260),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = 0,
                Maximum = 100,
                DecimalPlaces = 2,
                Value = 16
            };

            var lblHeader = new Label()
            {
                Text = "Encabezado de Boleta:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(500, 20),
                AutoSize = true
            };

            txtReceiptHeader = new TextBox()
            {
                Location = new Point(500, 50),
                Size = new Size(400, 60),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true
            };

            var lblFooter = new Label()
            {
                Text = "Pie de Boleta:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(500, 120),
                AutoSize = true
            };

            txtReceiptFooter = new TextBox()
            {
                Location = new Point(500, 150),
                Size = new Size(400, 60),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true
            };

            var btnSaveGeneral = new IconButton()
            {
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Guardar Cambios",
                Size = new Size(200, 45),
                Location = new Point(20, 320),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnSaveGeneral.FlatAppearance.BorderSize = 0;
            btnSaveGeneral.Click += SaveGeneralSettings;

            panel.Controls.Add(lblStoreName);
            panel.Controls.Add(txtStoreName);
            panel.Controls.Add(lblAddress);
            panel.Controls.Add(txtAddress);
            panel.Controls.Add(lblPhone);
            panel.Controls.Add(txtPhone);
            panel.Controls.Add(lblTax);
            panel.Controls.Add(numTax);
            panel.Controls.Add(lblHeader);
            panel.Controls.Add(txtReceiptHeader);
            panel.Controls.Add(lblFooter);
            panel.Controls.Add(txtReceiptFooter);
            panel.Controls.Add(btnSaveGeneral);

            tab.Controls.Add(panel);
        }

        private void SaveGeneralSettings(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStoreName.Text))
                {
                    MessageBox.Show("El nombre de la tienda es requerido", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.UpdateSettings(
                    txtStoreName.Text,
                    txtAddress.Text,
                    txtPhone.Text,
                    numTax.Value / 100,
                    txtReceiptHeader.Text,
                    txtReceiptFooter.Text
                );

                MessageBox.Show("Configuraci�n guardada exitosamente", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateUsersTab(TabPage tab)
        {
            var panel = new Panel()
            {
                Size = new Size(1050, 450),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20)
            };

            var lblInfo = new Label()
            {
                Text = "Gesti�n de Usuarios del Sistema",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 255, 127),
                Location = new Point(20, 20),
                AutoSize = true
            };

            var btnAddUser = new IconButton()
            {
                IconChar = IconChar.UserPlus,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Agregar Usuario",
                Size = new Size(180, 45),
                Location = new Point(20, 60),
                BackColor = Color.FromArgb(0, 200, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.Click += BtnAddUser_Click;

            var btnDeleteUser = new IconButton()
            {
                IconChar = IconChar.UserMinus,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Eliminar Usuario",
                Size = new Size(180, 45),
                Location = new Point(220, 60),
                BackColor = Color.FromArgb(192, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnDeleteUser.FlatAppearance.BorderSize = 0;
            btnDeleteUser.Click += BtnDeleteUser_Click;

            var btnChangePassword = new IconButton()
            {
                IconChar = IconChar.Key,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Cambiar Contrase�a",
                Size = new Size(200, 45),
                Location = new Point(420, 60),
                BackColor = Color.FromArgb(255, 159, 10),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Click += BtnChangePassword_Click;

            dgvUsers = new DataGridView()
            {
                Location = new Point(20, 120),
                Size = new Size(1000, 300),
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(50, 50, 50),
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(0, 120, 212),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    SelectionBackColor = Color.FromArgb(0, 120, 212)
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(30, 30, 30),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(0, 150, 255),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F)
                },
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            LoadUsers();

            panel.Controls.Add(lblInfo);
            panel.Controls.Add(btnAddUser);
            panel.Controls.Add(btnDeleteUser);
            panel.Controls.Add(btnChangePassword);
            panel.Controls.Add(dgvUsers);

            tab.Controls.Add(panel);
        }

        private void LoadUsers()
        {
            try
            {
                var dt = db.GetAllUsers();
                dgvUsers.DataSource = dt;

                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["Id"].Visible = false;
                    dgvUsers.Columns["Username"].HeaderText = "Usuario";
                    dgvUsers.Columns["IsAdmin"].HeaderText = "Administrador";
                    dgvUsers.Columns["IsAdmin"].Width = 150;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddUser_Click(object? sender, EventArgs e)
        {
            using (var dialog = new AddUserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        db.AddUser(dialog.Username, dialog.Password, dialog.IsAdmin);
                        LoadUsers();
                        MessageBox.Show("Usuario agregado exitosamente", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object? sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario para eliminar", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvUsers.SelectedRows[0];
            string username = row.Cells["Username"].Value.ToString()!;

            if (username == "admin")
            {
                MessageBox.Show("No se puede eliminar el usuario admin", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"�Est�s seguro de eliminar al usuario '{username}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int userId = Convert.ToInt32(row.Cells["Id"].Value);
                    db.DeleteUser(userId);
                    LoadUsers();
                    MessageBox.Show("Usuario eliminado exitosamente", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnChangePassword_Click(object? sender, EventArgs e)
        {
            using (var dialog = new ChangePasswordDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        db.ChangePassword(dialog.Username, dialog.NewPassword);
                        MessageBox.Show("Contrase�a cambiada exitosamente", "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateAppearanceTab(TabPage tab)
        {
            var panel = new Panel()
            {
                Size = new Size(1050, 450),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20)
            };

            var lblInfo = new Label()
            {
                Text = "Personalización Visual del Sistema",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ThemeManager.CurrentAccentColor,
                Location = new Point(20, 20),
                Size = new Size(1000, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblTheme = new Label()
            {
                Text = "Tema: Oscuro (Tech Style)",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                Location = new Point(20, 70),
                AutoSize = true
            };

            var lblAccent = new Label()
            {
                Text = $"Color de Acento Actual: {ThemeManager.GetColorName(ThemeManager.CurrentAccentColor)}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                Location = new Point(20, 100),
                AutoSize = true
            };

            var lblInstruction = new Label()
            {
                Text = "Click en un color para aplicarlo:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 180, 180),
                Location = new Point(20, 140),
                AutoSize = true
            };

            var panel1 = CreateClickableColorPreview("Verde Neón", ThemeManager.AccentColors.GreenNeon, 20, 180, lblAccent);
            var panel2 = CreateClickableColorPreview("Azul Eléctrico", ThemeManager.AccentColors.ElectricBlue, 180, 180, lblAccent);
            var panel3 = CreateClickableColorPreview("Púrpura", ThemeManager.AccentColors.Purple, 340, 180, lblAccent);
            var panel4 = CreateClickableColorPreview("Naranja", ThemeManager.AccentColors.Orange, 20, 300, lblAccent);
            var panel5 = CreateClickableColorPreview("Rojo", ThemeManager.AccentColors.Red, 180, 300, lblAccent);
            var panel6 = CreateClickableColorPreview("Cyan", ThemeManager.AccentColors.Cyan, 340, 300, lblAccent);

            var lblNote = new Label()
            {
                Text = "? Los cambios se aplican inmediatamente\n?? Se guarda automáticamente",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(150, 150, 150),
                Location = new Point(520, 180),
                Size = new Size(400, 80),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(10)
            };

            var btnReset = new IconButton()
            {
                IconChar = IconChar.RotateBackward,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Restaurar Default",
                Size = new Size(180, 45),
                Location = new Point(520, 280),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += (s, e) =>
            {
                ThemeManager.SaveThemeToDatabase(db, ThemeManager.AccentColors.GreenNeon);
                lblAccent.Text = $"Color de Acento Actual: {ThemeManager.GetColorName(ThemeManager.CurrentAccentColor)}";
                lblInfo.ForeColor = ThemeManager.CurrentAccentColor;
                MessageBox.Show("Tema restaurado a Verde Neón", "Tema Restaurado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            panel.Controls.Add(lblInfo);
            panel.Controls.Add(lblTheme);
            panel.Controls.Add(lblAccent);
            panel.Controls.Add(lblInstruction);
            panel.Controls.Add(panel1);
            panel.Controls.Add(panel2);
            panel.Controls.Add(panel3);
            panel.Controls.Add(panel4);
            panel.Controls.Add(panel5);
            panel.Controls.Add(panel6);
            panel.Controls.Add(lblNote);
            panel.Controls.Add(btnReset);

            tab.Controls.Add(panel);
        }

        private Panel CreateClickableColorPreview(string name, Color color, int x, int y, Label lblStatus)
        {
            var container = new Panel()
            {
                Size = new Size(150, 100),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(35, 35, 35),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.FixedSingle
            };

            var colorBox = new Panel()
            {
                Size = new Size(110, 50),
                Location = new Point(20, 10),
                BackColor = color,
                Cursor = Cursors.Hand
            };

            var label = new Label()
            {
                Text = name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(5, 70),
                Size = new Size(140, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };

            var checkIcon = new IconPictureBox()
            {
                IconChar = IconChar.CheckCircle,
                IconColor = Color.FromArgb(0, 255, 127),
                IconSize = 24,
                Size = new Size(24, 24),
                Location = new Point(120, 5),
                BackColor = Color.Transparent,
                Visible = color == ThemeManager.CurrentAccentColor
            };

            EventHandler clickHandler = (s, e) =>
            {
                try
                {
                    ThemeManager.SaveThemeToDatabase(db, color);
                    lblStatus.Text = $"Color de Acento Actual: {name}";
                    var parentPanel = container.Parent;
                    if (parentPanel != null)
                    {
                        foreach (Control ctrl in parentPanel.Controls)
                        {
                            if (ctrl is Panel p && p != container)
                            {
                                foreach (Control child in p.Controls)
                                {
                                    if (child is IconPictureBox icon && icon.IconChar == IconChar.CheckCircle)
                                        icon.Visible = false;
                                }
                            }
                        }
                    }
                    checkIcon.Visible = true;
                    checkIcon.IconColor = color;
                    MessageBox.Show($"? Tema aplicado: {name}", "Tema Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            container.Click += clickHandler;
            colorBox.Click += clickHandler;
            label.Click += clickHandler;

            container.MouseEnter += (s, e) => container.BackColor = Color.FromArgb(45, 45, 45);
            container.MouseLeave += (s, e) => container.BackColor = Color.FromArgb(35, 35, 35);

            container.Controls.Add(colorBox);
            container.Controls.Add(label);
            container.Controls.Add(checkIcon);
            return container;
        }

        

        private void CreateAboutTab(TabPage tab)
        {
            var panel = new Panel()
            {
                Size = new Size(1050, 450),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20)
            };

            var picLogo = new IconPictureBox()
            {
                IconChar = IconChar.Microchip,
                IconColor = Color.FromArgb(0, 255, 127),
                IconSize = 100,
                Size = new Size(100, 100),
                Location = new Point(475, 30),
                BackColor = Color.Transparent
            };

            var lblAppName = new Label()
            {
                Text = "SEKAI POS",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(350, 150),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblVersion = new Label()
            {
                Text = "Versi�n 1.0.0 - Build 2025.01",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(180, 180, 180),
                Location = new Point(350, 200),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDescription = new Label()
            {
                Text = "Sistema de Punto de Venta para Tiendas de Tecnolog�a\n\n" +
                       "Desarrollado con .NET 10 y Windows Forms\n" +
                       "Base de datos: SQLite\n" +
                       "UI Framework: FontAwesome.Sharp\n\n" +
                       "� 2025 - Todos los derechos reservados",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(250, 250),
                Size = new Size(550, 130),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnGitHub = new IconButton()
            {
                IconChar = IconChar.Github,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Ver en GitHub",
                Size = new Size(180, 45),
                Location = new Point(460, 390),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnGitHub.FlatAppearance.BorderSize = 0;
            btnGitHub.Click += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://github.com/Johann3421/SekaiPOS_1.0",
                        UseShellExecute = true
                    });
                }
                catch { }
            };

            panel.Controls.Add(picLogo);
            panel.Controls.Add(lblAppName);
            panel.Controls.Add(lblVersion);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(btnGitHub);

            tab.Controls.Add(panel);
        }
    }

    // Dialog for adding users
    public class AddUserDialog : Form
    {
        public string Username { get; private set; } = "";
        public string Password { get; private set; } = "";
        public bool IsAdmin { get; private set; }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirm;
        private CheckBox chkAdmin;

        public AddUserDialog()
        {
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            this.Text = "Agregar Usuario";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblUser = new Label() { Text = "Usuario:", ForeColor = Color.White, Location = new Point(20, 20), AutoSize = true };
            txtUsername = new TextBox() { Location = new Point(20, 45), Size = new Size(340, 25), BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            var lblPass = new Label() { Text = "Contrase�a:", ForeColor = Color.White, Location = new Point(20, 80), AutoSize = true };
            txtPassword = new TextBox() { Location = new Point(20, 105), Size = new Size(340, 25), PasswordChar = '*', BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            var lblConfirm = new Label() { Text = "Confirmar:", ForeColor = Color.White, Location = new Point(20, 140), AutoSize = true };
            txtConfirm = new TextBox() { Location = new Point(20, 165), Size = new Size(340, 25), PasswordChar = '*', BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            chkAdmin = new CheckBox() { Text = "Administrador", ForeColor = Color.White, Location = new Point(20, 200), AutoSize = true };

            var btnOk = new Button() { Text = "Crear", DialogResult = DialogResult.OK, Location = new Point(180, 230), Size = new Size(90, 30), BackColor = Color.FromArgb(0, 120, 212), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnCancel = new Button() { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(280, 230), Size = new Size(90, 30), BackColor = Color.FromArgb(100, 100, 100), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Ingresa un nombre de usuario", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (txtPassword.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Las contrase�as no coinciden", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (txtPassword.Text.Length < 4)
                {
                    MessageBox.Show("La contrase�a debe tener al menos 4 caracteres", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                Username = txtUsername.Text;
                Password = txtPassword.Text;
                IsAdmin = chkAdmin.Checked;
            };

            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(txtConfirm);
            this.Controls.Add(chkAdmin);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }
    }

    // Dialog for changing password
    public class ChangePasswordDialog : Form
    {
        public string Username { get; private set; } = "";
        public string NewPassword { get; private set; } = "";

        private TextBox txtUsername;
        private TextBox txtNewPassword;
        private TextBox txtConfirm;

        public ChangePasswordDialog()
        {
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            this.Text = "Cambiar Contrase�a";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblUser = new Label() { Text = "Usuario:", ForeColor = Color.White, Location = new Point(20, 20), AutoSize = true };
            txtUsername = new TextBox() { Location = new Point(20, 45), Size = new Size(340, 25), BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            var lblPass = new Label() { Text = "Nueva Contrase�a:", ForeColor = Color.White, Location = new Point(20, 80), AutoSize = true };
            txtNewPassword = new TextBox() { Location = new Point(20, 105), Size = new Size(340, 25), PasswordChar = '*', BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            var lblConfirm = new Label() { Text = "Confirmar:", ForeColor = Color.White, Location = new Point(20, 140), AutoSize = true };
            txtConfirm = new TextBox() { Location = new Point(20, 165), Size = new Size(340, 25), PasswordChar = '*', BackColor = Color.FromArgb(35, 35, 35), ForeColor = Color.White };

            var btnOk = new Button() { Text = "Cambiar", DialogResult = DialogResult.OK, Location = new Point(180, 210), Size = new Size(90, 30), BackColor = Color.FromArgb(0, 120, 212), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            var btnCancel = new Button() { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(280, 210), Size = new Size(90, 30), BackColor = Color.FromArgb(100, 100, 100), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Ingresa el nombre de usuario", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (txtNewPassword.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Las contrase�as no coinciden", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (txtNewPassword.Text.Length < 4)
                {
                    MessageBox.Show("La contrase�a debe tener al menos 4 caracteres", "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                Username = txtUsername.Text;
                NewPassword = txtNewPassword.Text;
            };

            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtNewPassword);
            this.Controls.Add(lblConfirm);
            this.Controls.Add(txtConfirm);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }
    }
}
