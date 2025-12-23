using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class LoginForm : Form
    {
        private DatabaseHelper db;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblTitle = null!;
        private Panel leftPanel = null!;
        private IconPictureBox picLogo = null!;
        private Label lblWelcome = null!;
        private Label lblVersion = null!;
        private CheckBox chkRemember = null!;
        private IconButton btnTogglePassword = null!;
        private LinkLabel linkForgot = null!;
        private Panel panelUsername = null!;
        private Panel panelPassword = null!;

        private bool isPasswordVisible = false;
        private bool mouseDown = false;
        private Point lastLocation = Point.Empty;

        public LoginForm()
        {
            db = new DatabaseHelper();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Sekai POS - Iniciar Sesión";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(900, 550);
            this.BackColor = Color.FromArgb(15, 15, 15);

            // Left panel - Tech dark theme
            leftPanel = new Panel()
            {
                Size = new Size(400, this.ClientSize.Height),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(20, 20, 20)
            };

            leftPanel.Paint += (s, e) =>
            {
                if (e?.Graphics != null)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        leftPanel.ClientRectangle,
                        Color.FromArgb(0, 120, 212),
                        Color.FromArgb(0, 90, 158),
                        45F))
                    {
                        e.Graphics.FillRectangle(brush, leftPanel.ClientRectangle);
                    }
                }
            };

            picLogo = new IconPictureBox()
            {
                Size = new Size(140, 140),
                Location = new Point((leftPanel.Width - 140) / 2, 100),
                IconChar = IconChar.Microchip,
                IconColor = Color.FromArgb(0, 255, 127),
                IconSize = 140,
                BackColor = Color.Transparent
            };

            lblWelcome = new Label()
            {
                AutoSize = false,
                Size = new Size(leftPanel.Width - 60, 100),
                Location = new Point(30, 260),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point),
                Text = "SEKAI POS\nSistema de Ventas",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.Transparent
            };

            lblVersion = new Label()
            {
                AutoSize = true,
                Location = new Point(30, leftPanel.Height - 50),
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Text = "Versión 1.0 © 2025 - Tech Store",
                BackColor = Color.Transparent
            };

            leftPanel.Controls.Add(picLogo);
            leftPanel.Controls.Add(lblWelcome);
            leftPanel.Controls.Add(lblVersion);

            // Right area - Modern dark login
            int rightX = leftPanel.Width + 80;
            int topMargin = 120;

            lblTitle = new Label()
            {
                Text = "Iniciar Sesión",
                Location = new Point(rightX, topMargin),
                AutoSize = true,
                Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White
            };

            // Username panel with border
            panelUsername = new Panel()
            {
                Location = new Point(rightX, topMargin + 80),
                Size = new Size(350, 50),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.None
            };

            panelUsername.Paint += (s, e) =>
            {
                if (e?.Graphics != null)
                {
                    using (Pen pen = new Pen(Color.FromArgb(0, 120, 212), 2))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, panelUsername.Width - 1, panelUsername.Height - 1);
                    }
                }
            };

            var iconUser = new IconPictureBox()
            {
                IconChar = IconChar.UserAlt,
                IconColor = Color.FromArgb(0, 120, 212),
                IconSize = 24,
                Size = new Size(24, 24),
                Location = new Point(15, 13),
                BackColor = Color.Transparent
            };

            txtUsername = new TextBox()
            {
                Location = new Point(55, 15),
                Size = new Size(280, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White
            };

            panelUsername.Controls.Add(iconUser);
            panelUsername.Controls.Add(txtUsername);

            // Password panel with border
            panelPassword = new Panel()
            {
                Location = new Point(rightX, topMargin + 150),
                Size = new Size(350, 50),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.None
            };

            panelPassword.Paint += (s, e) =>
            {
                if (e?.Graphics != null)
                {
                    using (Pen pen = new Pen(Color.FromArgb(0, 120, 212), 2))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, panelPassword.Width - 1, panelPassword.Height - 1);
                    }
                }
            };

            var iconLock = new IconPictureBox()
            {
                IconChar = IconChar.Lock,
                IconColor = Color.FromArgb(0, 120, 212),
                IconSize = 24,
                Size = new Size(24, 24),
                Location = new Point(15, 13),
                BackColor = Color.Transparent
            };

            txtPassword = new TextBox()
            {
                Location = new Point(55, 15),
                Size = new Size(230, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                UseSystemPasswordChar = true
            };

            btnTogglePassword = new IconButton()
            {
                Location = new Point(295, 10),
                Size = new Size(40, 30),
                IconChar = IconChar.Eye,
                IconColor = Color.FromArgb(0, 120, 212),
                IconSize = 20,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 30, 30),
                Cursor = Cursors.Hand
            };
            btnTogglePassword.FlatAppearance.BorderSize = 0;

            panelPassword.Controls.Add(iconLock);
            panelPassword.Controls.Add(txtPassword);
            panelPassword.Controls.Add(btnTogglePassword);

            chkRemember = new CheckBox()
            {
                Text = "Recordarme",
                Location = new Point(rightX, topMargin + 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(180, 180, 180)
            };

            linkForgot = new LinkLabel()
            {
                Text = "¿Olvidaste tu contraseña?",
                Location = new Point(rightX + 200, topMargin + 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                LinkColor = Color.FromArgb(0, 120, 212),
                ActiveLinkColor = Color.FromArgb(0, 255, 127)
            };

            btnLogin = new Button()
            {
                Text = "ENTRAR",
                Location = new Point(rightX, topMargin + 270),
                Size = new Size(350, 55),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 140, 232);

            // Close button
            var btnClose = new IconButton()
            {
                Location = new Point(this.ClientSize.Width - 50, 15),
                Size = new Size(40, 40),
                IconChar = IconChar.Xmark,
                IconColor = Color.White,
                IconSize = 24,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 0, 0);
            btnClose.Click += (s, e) => this.Close();

            // Events
            txtUsername.Enter += (s, e) =>
            {
                panelUsername.Invalidate();
            };
            txtUsername.Leave += (s, e) =>
            {
                panelUsername.Invalidate();
            };
            
            txtPassword.Enter += (s, e) =>
            {
                panelPassword.Invalidate();
            };
            txtPassword.Leave += (s, e) =>
            {
                panelPassword.Invalidate();
            };

            btnTogglePassword.Click += (s, e) =>
            {
                isPasswordVisible = !isPasswordVisible;
                txtPassword.UseSystemPasswordChar = !isPasswordVisible;
                btnTogglePassword.IconChar = isPasswordVisible ? IconChar.EyeSlash : IconChar.Eye;
            };

            btnLogin.Click += (s, e) =>
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Por favor ingresa usuario y contraseña.", "Error de inicio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (db.AuthenticateUser(username, password))
                    {
                        CurrentUser.Username = username;
                        CurrentUser.IsAdmin = db.IsUserAdmin(username);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Inicio fallido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            linkForgot.Click += (s, e) =>
            {
                MessageBox.Show("Contacta al administrador para restablecer tu contraseña.", "Recuperar contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.AcceptButton = btnLogin;

            // Add all controls
            this.Controls.Add(leftPanel);
            this.Controls.Add(lblTitle);
            this.Controls.Add(panelUsername);
            this.Controls.Add(panelPassword);
            this.Controls.Add(chkRemember);
            this.Controls.Add(linkForgot);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnClose);

            // Form dragging
            this.MouseDown += (s, e) =>
            {
                if (e is MouseEventArgs me)
                {
                    mouseDown = true;
                    lastLocation = me.Location;
                }
            };
            
            this.MouseMove += (s, e) =>
            {
                if (e is MouseEventArgs me && mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + me.X,
                        (this.Location.Y - lastLocation.Y) + me.Y);
                }
            };
            
            this.MouseUp += (s, e) => mouseDown = false;
        }
    }

    public static class CurrentUser
    {
        public static string Username { get; set; } = string.Empty;
        public static bool IsAdmin { get; set; }
    }
}