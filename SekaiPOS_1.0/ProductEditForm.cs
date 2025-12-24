using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class ProductEditForm : Form
    {
        private DatabaseHelper db;
        private int? productId;
        private TextBox txtName = null!;
        private TextBox txtDescription = null!;
        private TextBox txtPrice = null!;
        private TextBox txtQuantity = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public ProductEditForm(DatabaseHelper database, int? id)
        {
            db = database;
            productId = id;
            InitializeComponent();

            if (productId.HasValue)
            {
                LoadProductData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = productId.HasValue ? "Editar Producto" : "Agregar Producto";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(34, 33, 74);

            // Title
            var lblTitle = new Label()
            {
                Text = productId.HasValue ? "Editar Producto" : "Nuevo Producto",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Icon
            var icon = new IconPictureBox()
            {
                IconChar = IconChar.BoxOpen,
                IconColor = Color.FromArgb(52, 152, 219),
                IconSize = 40,
                Size = new Size(40, 40),
                Location = new Point(this.Width - 80, 20),
                BackColor = Color.Transparent
            };

            // Name
            var lblName = CreateLabel("Nombre del Producto:", 30, 80);
            txtName = CreateTextBox(30, 110);

            // Description
            var lblDescription = CreateLabel("Descripción:", 30, 160);
            txtDescription = CreateTextBox(30, 190);
            txtDescription.Multiline = true;
            txtDescription.Height = 60;

            // Price
            var lblPrice = CreateLabel("Precio:", 30, 270);
            txtPrice = CreateTextBox(30, 300);
            txtPrice.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
            };

            // Quantity
            var lblQuantity = CreateLabel("Cantidad:", 260, 270);
            txtQuantity = CreateTextBox(260, 300);
            txtQuantity.Width = 190;
            txtQuantity.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Buttons
            btnSave = new Button()
            {
                Text = "Guardar",
                Size = new Size(200, 45),
                Location = new Point(30, 360),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button()
            {
                Text = "Cancelar",
                Size = new Size(200, 45),
                Location = new Point(250, 360),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(icon);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(txtQuantity);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnSave;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private TextBox CreateTextBox(int x, int y)
        {
            return new TextBox()
            {
                Location = new Point(x, y),
                Size = new Size(420, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(46, 45, 89),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void LoadProductData()
        {
            try
            {
                var dt = db.GetAllProducts();
                var rows = dt.Select($"Id = {productId}");

                if (rows.Length > 0)
                {
                    var row = rows[0];
                    txtName.Text = row["Name"].ToString();
                    txtDescription.Text = row["Description"].ToString();
                    txtPrice.Text = row["Price"].ToString();
                    txtQuantity.Text = row["Quantity"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("El precio es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("La cantidad es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            try
            {
                string name = txtName.Text.Trim();
                string description = txtDescription.Text.Trim();
                decimal price = decimal.Parse(txtPrice.Text);
                int quantity = int.Parse(txtQuantity.Text);

                if (productId.HasValue)
                {
                    db.UpdateProduct(productId.Value, name, description, price, quantity);
                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    db.AddProduct(name, description, price, quantity);
                    MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
