using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class MainDashboardFinal : Form
    {
        private DatabaseHelper db;
        private Panel leftMenuPanel = null!;
        private Panel topPanel = null!;
        private Panel contentPanel = null!;
        private IconButton btnDashboard = null!;
        private IconButton btnInventory = null!;
        private IconButton btnSales = null!;
        private IconButton btnReports = null!;
        private IconButton btnSettings = null!;
        private IconButton currentButton = null!;
        private Panel leftBorderBtn = null!;
        private Form? currentChildForm;
        private Label lblTitle = null!;
        private Label lblUserInfo = null!;
        private Label lblDateTime = null!;
        private IconPictureBox iconCurrentForm = null!;
        private System.Windows.Forms.Timer dateTimeTimer = null!;

        public MainDashboardFinal()
        {
            db = new DatabaseHelper();
            ThemeManager.LoadThemeFromDatabase(db);
            ThemeManager.OnThemeChanged += (s, e) => ApplyCurrentTheme();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Sekai POS - Dashboard";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1280, 720);
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.WindowState = FormWindowState.Maximized;

            leftMenuPanel = new Panel()
            {
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(20, 20, 20),
                Size = new Size(250, this.ClientSize.Height)
            };

            var logoPanel = new Panel()
            {
                Size = new Size(250, 140),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            var picLogo = new IconPictureBox()
            {
                IconChar = IconChar.Microchip,
                IconColor = Color.FromArgb(0, 255, 127),
                IconSize = 60,
                Size = new Size(60, 60),
                Location = new Point(25, 30),
                BackColor = Color.Transparent
            };

            var lblLogo = new Label()
            {
                Text = "SEKAI POS",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(95, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblSubtitle = new Label()
            {
                Text = "Tech Store System",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(0, 120, 212),
                Location = new Point(95, 70),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            logoPanel.Controls.Add(picLogo);
            logoPanel.Controls.Add(lblLogo);
            logoPanel.Controls.Add(lblSubtitle);

            btnDashboard = CreateMenuButton(IconChar.ChartLine, "Dashboard", 160);
            btnInventory = CreateMenuButton(IconChar.BoxesStacked, "Inventario", 230);
            btnSales = CreateMenuButton(IconChar.CashRegister, "Ventas", 300);
            btnReports = CreateMenuButton(IconChar.FileInvoiceDollar, "Reportes", 370);
            btnSettings = CreateMenuButton(IconChar.Gear, "Configuraci�n", 440);

            leftBorderBtn = new Panel()
            {
                Size = new Size(5, 70),
                BackColor = Color.FromArgb(0, 255, 127),
                Visible = false
            };

            leftMenuPanel.Controls.Add(logoPanel);
            leftMenuPanel.Controls.Add(leftBorderBtn);
            leftMenuPanel.Controls.Add(btnDashboard);
            leftMenuPanel.Controls.Add(btnInventory);
            leftMenuPanel.Controls.Add(btnSales);
            leftMenuPanel.Controls.Add(btnReports);
            leftMenuPanel.Controls.Add(btnSettings);

            topPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Size = new Size(this.ClientSize.Width - 250, 90),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            iconCurrentForm = new IconPictureBox()
            {
                BackColor = Color.Transparent,
                IconChar = IconChar.ChartLine,
                IconColor = Color.FromArgb(0, 255, 127),
                IconSize = 45,
                Size = new Size(45, 45),
                Location = new Point(30, 22)
            };

            lblTitle = new Label()
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(85, 28),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            lblDateTime = new Label()
            {
                Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 180),
                Location = new Point(topPanel.Width - 500, 20),
                AutoSize = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            dateTimeTimer = new System.Windows.Forms.Timer()
            {
                Interval = 1000
            };
            dateTimeTimer.Tick += (s, e) => lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            dateTimeTimer.Start();

            // CORREGIDO: Icono de usuario con emoji
            var iconUser = new IconPictureBox()
            {
                IconChar = IconChar.UserCircle,
                IconColor = Color.FromArgb(0, 120, 212),
                IconSize = 20,
                Size = new Size(20, 20),
                Location = new Point(topPanel.Width - 520, 50),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            lblUserInfo = new Label()
            {
                Text = $"{CurrentUser.Username} {(CurrentUser.IsAdmin ? "(Administrador)" : "")}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 212),
                Location = new Point(topPanel.Width - 495, 50),
                AutoSize = true,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnLogout = new IconButton()
            {
                IconChar = IconChar.ArrowRightFromBracket,
                IconColor = Color.White,
                IconSize = 22,
                Text = "Cerrar Sesi�n",
                Size = new Size(150, 45),
                Location = new Point(topPanel.Width - 170, 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(192, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 0, 0);
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("�Deseas cerrar sesi�n?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dateTimeTimer.Stop();
                    this.Close();
                }
            };

            topPanel.Controls.Add(iconCurrentForm);
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(lblDateTime);
            topPanel.Controls.Add(iconUser);
            topPanel.Controls.Add(lblUserInfo);
            topPanel.Controls.Add(btnLogout);

            contentPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(15, 15, 15),
                Padding = new Padding(15)
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(leftMenuPanel);

            btnDashboard.Click += (s, e) => { ActivateButton(s); OpenDashboardHome(); };
            btnInventory.Click += (s, e) => { ActivateButton(s); OpenInventoryForm(); };
            btnSales.Click += (s, e) => { ActivateButton(s); OpenSalesForm(); };
            btnReports.Click += (s, e) => { ActivateButton(s); OpenReportsForm(); };
            btnSettings.Click += (s, e) => { ActivateButton(s); OpenSettingsForm(); };

            ActivateButton(btnDashboard);
            OpenDashboardHome();
            ApplyCurrentTheme();
        }

        private void ApplyCurrentTheme()
        {
            try
            {
                ThemeManager.ApplyTheme(this);
                if (leftBorderBtn != null) leftBorderBtn.BackColor = ThemeManager.CurrentAccentColor;
                if (currentButton != null)
                {
                    currentButton.ForeColor = ThemeManager.CurrentAccentColor;
                    currentButton.IconColor = ThemeManager.CurrentAccentColor;
                }
                if (iconCurrentForm != null) iconCurrentForm.IconColor = ThemeManager.CurrentAccentColor;
                if (currentChildForm != null)
                {
                    ThemeManager.ApplyTheme(currentChildForm);
                    currentChildForm.Refresh();
                }
                this.Refresh();
            }
            catch { }
        }

        private IconButton CreateMenuButton(IconChar icon, string text, int yPosition)
        {
            var button = new IconButton()
            {
                IconChar = icon,
                IconColor = Color.FromArgb(180, 180, 180),
                IconSize = 28,
                ImageAlign = ContentAlignment.MiddleLeft,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Size = new Size(250, 70),
                Location = new Point(0, yPosition),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(20, 20, 20),
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 11F),
                Padding = new Padding(20, 0, 20, 0),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            return button;
        }

        private void ActivateButton(object? senderBtn)
        {
            if (senderBtn != null)
            {
                DisableButton();
                currentButton = (IconButton)senderBtn;
                currentButton.BackColor = Color.FromArgb(30, 30, 30);
                currentButton.ForeColor = ThemeManager.CurrentAccentColor;
                currentButton.IconColor = ThemeManager.CurrentAccentColor;
                leftBorderBtn.BackColor = ThemeManager.CurrentAccentColor;
                leftBorderBtn.Location = new Point(0, currentButton.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }

        private void DisableButton()
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(20, 20, 20);
                currentButton.ForeColor = Color.FromArgb(180, 180, 180);
                currentButton.IconColor = Color.FromArgb(180, 180, 180);
            }
        }

        private void OpenChildForm(Form childForm, IconChar icon, string title)
        {
            try
            {
                currentChildForm?.Close();
                currentChildForm?.Dispose();
                currentChildForm = childForm;
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                contentPanel.Controls.Clear();
                contentPanel.Controls.Add(childForm);
                childForm.BringToFront();
                childForm.Show();
                iconCurrentForm.IconChar = icon;
                lblTitle.Text = title;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenDashboardHome()
        {
            currentChildForm?.Close();
            currentChildForm = null;
            contentPanel.Controls.Clear();
            iconCurrentForm.IconChar = IconChar.ChartLine;
            lblTitle.Text = "Dashboard";

            var welcomeLabel = new Label()
            {
                Text = $"Bienvenido, {CurrentUser.Username}!",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 30),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            int totalProducts = db.GetTotalProducts();
            decimal todaySales = db.GetTodaySales();
            int totalSales = db.GetTotalSales();

            var card1 = CreateDashboardCard(IconChar.BoxesStacked, "Productos", totalProducts.ToString(), Color.FromArgb(0, 120, 212), 40, 100);
            var card2 = CreateDashboardCard(IconChar.DollarSign, "Ventas Hoy", todaySales.ToString("C2"), Color.FromArgb(0, 200, 83), 320, 100);
            var card3 = CreateDashboardCard(IconChar.ShoppingCart, "Total Ventas", totalSales.ToString(), Color.FromArgb(255, 159, 10), 600, 100);
            var card4 = CreateDashboardCard(IconChar.ChartLine, "Promedio", totalSales > 0 ? (todaySales / totalSales).ToString("C2") : "$0.00", Color.FromArgb(156, 39, 176), 880, 100);

            contentPanel.Controls.Add(welcomeLabel);
            contentPanel.Controls.Add(card1);
            contentPanel.Controls.Add(card2);
            contentPanel.Controls.Add(card3);
            contentPanel.Controls.Add(card4);
        }

        private Panel CreateDashboardCard(IconChar icon, string title, string value, Color accentColor, int x, int y)
        {
            var card = new Panel()
            {
                Size = new Size(260, 170),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            card.Paint += (s, e) =>
            {
                if (e?.Graphics != null)
                {
                    using (Pen pen = new Pen(accentColor, 3))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                    }
                }
            };

            var iconPic = new IconPictureBox()
            {
                IconChar = icon,
                IconColor = accentColor,
                IconSize = 55,
                Size = new Size(55, 55),
                Location = new Point(25, 25),
                BackColor = Color.Transparent
            };

            var lblTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(25, 95),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblValue = new Label()
            {
                Text = value,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(25, 120),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            card.Controls.Add(iconPic);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            return card;
        }

        private void OpenInventoryForm()
        {
            try
            {
                var form = new InventoryFormFinal(db);
                OpenChildForm(form, IconChar.BoxesStacked, "Inventario");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSalesForm()
        {
            try
            {
                var form = new SalesFormFinal(db);
                OpenChildForm(form, IconChar.CashRegister, "Ventas");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenReportsForm()
        {
            try
            {
                var form = new ReportsForm(db);
                OpenChildForm(form, IconChar.FileInvoiceDollar, "Reportes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSettingsForm()
        {
            try
            {
                var form = new SettingsFormFunctional(db);
                OpenChildForm(form, IconChar.Gear, "Configuraci�n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dateTimeTimer?.Stop();
            dateTimeTimer?.Dispose();
            currentChildForm?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
