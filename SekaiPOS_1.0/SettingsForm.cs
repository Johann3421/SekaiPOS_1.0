using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class SettingsForm : Form
    {
        private DatabaseHelper db;
        private TabControl tabControl;

        public SettingsForm(DatabaseHelper database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
                
            db = database;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.Padding = new Padding(20);

            var lblTitle = new Label()
            {
                Text = "Configuración del Sistema",
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

            // Tab 1: General Settings
            var tabGeneral = new TabPage("General");
            tabGeneral.BackColor = Color.FromArgb(20, 20, 20);
            CreateGeneralTab(tabGeneral);

            // Tab 2: Users Management
            var tabUsers = new TabPage("Usuarios");
            tabUsers.BackColor = Color.FromArgb(20, 20, 20);
            CreateUsersTab(tabUsers);

            // Tab 3: Appearance
            var tabAppearance = new TabPage("Apariencia");
            tabAppearance.BackColor = Color.FromArgb(20, 20, 20);
            CreateAppearanceTab(tabAppearance);

            // Tab 4: About
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

        private void CreateGeneralTab(TabPage tab)
        {
            var panel = new Panel()
            {
                Size = new Size(1050, 450),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(25, 25, 25),
                Padding = new Padding(20)
            };

            var lblStoreName = new Label()
            {
                Text = "Nombre de la Tienda:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var txtStoreName = new TextBox()
            {
                Location = new Point(20, 50),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "SEKAI Tech Store"
            };

            var lblAddress = new Label()
            {
                Text = "Dirección:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 100),
                AutoSize = true
            };

            var txtAddress = new TextBox()
            {
                Location = new Point(20, 130),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Av. Principal #123, Ciudad"
            };

            var lblPhone = new Label()
            {
                Text = "Teléfono:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 180),
                AutoSize = true
            };

            var txtPhone = new TextBox()
            {
                Location = new Point(20, 210),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "+1 234 567 8900"
            };

            var lblTax = new Label()
            {
                Text = "IVA (%):",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 260),
                AutoSize = true
            };

            var numTax = new NumericUpDown()
            {
                Location = new Point(20, 290),
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

            var btnSaveGeneral = new IconButton()
            {
                IconChar = IconChar.Save,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Guardar Cambios",
                Size = new Size(200, 45),
                Location = new Point(20, 360),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnSaveGeneral.FlatAppearance.BorderSize = 0;
            btnSaveGeneral.Click += (s, e) => MessageBox.Show("Configuración guardada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            panel.Controls.Add(lblStoreName);
            panel.Controls.Add(txtStoreName);
            panel.Controls.Add(lblAddress);
            panel.Controls.Add(txtAddress);
            panel.Controls.Add(lblPhone);
            panel.Controls.Add(txtPhone);
            panel.Controls.Add(lblTax);
            panel.Controls.Add(numTax);
            panel.Controls.Add(btnSaveGeneral);

            tab.Controls.Add(panel);
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
                Text = "Gestión de Usuarios del Sistema",
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

            var dgvUsers = new DataGridView()
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

            dgvUsers.Columns.Add("Username", "Usuario");
            dgvUsers.Columns.Add("IsAdmin", "Administrador");
            dgvUsers.Rows.Add("admin", "Sí");

            panel.Controls.Add(lblInfo);
            panel.Controls.Add(btnAddUser);
            panel.Controls.Add(dgvUsers);

            tab.Controls.Add(panel);
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

            var lblTheme = new Label()
            {
                Text = "Tema de Color:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var cmbTheme = new ComboBox()
            {
                Location = new Point(20, 50),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTheme.Items.AddRange(new[] { "Oscuro (Actual)", "Claro", "Alto Contraste" });
            cmbTheme.SelectedIndex = 0;

            var lblAccent = new Label()
            {
                Text = "Color de Acento:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 100),
                AutoSize = true
            };

            var panel1 = CreateColorOption("Verde Neón (Actual)", Color.FromArgb(0, 255, 127), 20, 130);
            var panel2 = CreateColorOption("Azul Eléctrico", Color.FromArgb(0, 191, 255), 170, 130);
            var panel3 = CreateColorOption("Púrpura", Color.FromArgb(138, 43, 226), 320, 130);
            var panel4 = CreateColorOption("Naranja", Color.FromArgb(255, 140, 0), 470, 130);

            panel.Controls.Add(lblTheme);
            panel.Controls.Add(cmbTheme);
            panel.Controls.Add(lblAccent);
            panel.Controls.Add(panel1);
            panel.Controls.Add(panel2);
            panel.Controls.Add(panel3);
            panel.Controls.Add(panel4);

            tab.Controls.Add(panel);
        }

        private Panel CreateColorOption(string name, Color color, int x, int y)
        {
            var container = new Panel()
            {
                Size = new Size(140, 100),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(30, 30, 30),
                Cursor = Cursors.Hand
            };

            var colorBox = new Panel()
            {
                Size = new Size(100, 50),
                Location = new Point(20, 10),
                BackColor = color
            };

            var label = new Label()
            {
                Text = name,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.White,
                Location = new Point(5, 70),
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            container.Controls.Add(colorBox);
            container.Controls.Add(label);

            container.Click += (s, e) => MessageBox.Show($"Tema {name} seleccionado", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                Text = "Versión 1.0.0",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(180, 180, 180),
                Location = new Point(350, 200),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDescription = new Label()
            {
                Text = "Sistema de Punto de Venta para Tiendas de Tecnología\n\n" +
                       "Desarrollado con .NET 10 y Windows Forms\n" +
                       "© 2025 - Todos los derechos reservados",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(250, 250),
                Size = new Size(550, 100),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnGitHub = new IconButton()
            {
                IconChar = IconChar.Github,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Ver en GitHub",
                Size = new Size(180, 45),
                Location = new Point(460, 370),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnGitHub.FlatAppearance.BorderSize = 0;
            btnGitHub.Click += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/Johann3421/SekaiPOS_1.0",
                UseShellExecute = true
            });

            panel.Controls.Add(picLogo);
            panel.Controls.Add(lblAppName);
            panel.Controls.Add(lblVersion);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(btnGitHub);

            tab.Controls.Add(panel);
        }

        private void BtnAddUser_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad de agregar usuario en desarrollo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
