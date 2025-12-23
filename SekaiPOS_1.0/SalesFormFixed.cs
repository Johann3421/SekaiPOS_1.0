using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class SalesFormFixed : Form
    {
        private DatabaseHelper db;
        private DataGridView dgvProducts = null!;
        private DataGridView dgvCart = null!;
        private TextBox txtSearch = null!;
        private TextBox txtBarcode = null!;
        private Label lblTotal = null!;
        private Label lblSubtotal = null!;
        private Label lblTax = null!;
        private ComboBox cmbPayment = null!;
        private NumericUpDown numQuantity = null!;
        
        private List<SaleItem> cartItems = new List<SaleItem>();
        private const decimal TAX_RATE = 0.16m;

        public SalesFormFixed(DatabaseHelper database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
                
            db = database;
            InitializeComponent();
            
            this.Shown += (s, e) => LoadProducts();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.Padding = new Padding(20);

            var lblProductsTitle = new Label()
            {
                Text = "Catálogo de Productos",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var searchPanel = new Panel()
            {
                Size = new Size(600, 50),
                Location = new Point(20, 60),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            var lblSearch = new Label()
            {
                Text = "?? Buscar:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true
            };

            txtSearch = new TextBox()
            {
                Location = new Point(100, 12),
                Size = new Size(240, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += (s, e) => FilterProducts();

            var lblBarcode = new Label()
            {
                Text = "Código:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(360, 15),
                AutoSize = true
            };

            txtBarcode = new TextBox()
            {
                Location = new Point(440, 12),
                Size = new Size(140, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtBarcode.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    AddProductByBarcode();
                    e.Handled = true;
                }
            };

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(lblBarcode);
            searchPanel.Controls.Add(txtBarcode);

            dgvProducts = new DataGridView()
            {
                Location = new Point(20, 125),
                Size = new Size(600, 400),
                BackgroundColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(50, 50, 50),
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(0, 120, 212),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    SelectionBackColor = Color.FromArgb(0, 120, 212),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(25, 25, 25),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(0, 150, 255),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F)
                },
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                RowHeadersVisible = false
            };
            dgvProducts.DoubleClick += (s, e) => AddSelectedProductToCart();

            var lblCartTitle = new Label()
            {
                Text = "Carrito de Compra",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(650, 20),
                AutoSize = true
            };

            var controlsPanel = new Panel()
            {
                Size = new Size(500, 50),
                Location = new Point(650, 60),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            var lblQty = new Label()
            {
                Text = "Cantidad:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true
            };

            numQuantity = new NumericUpDown()
            {
                Location = new Point(100, 12),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };

            var btnAddToCart = new IconButton()
            {
                IconChar = IconChar.CartPlus,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Agregar",
                Size = new Size(130, 35),
                Location = new Point(200, 8),
                BackColor = Color.FromArgb(0, 200, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Click += (s, e) => AddSelectedProductToCart();

            controlsPanel.Controls.Add(lblQty);
            controlsPanel.Controls.Add(numQuantity);
            controlsPanel.Controls.Add(btnAddToCart);

            dgvCart = new DataGridView()
            {
                Location = new Point(650, 125),
                Size = new Size(500, 250),
                BackgroundColor = Color.FromArgb(25, 25, 25),
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
                    BackColor = Color.FromArgb(25, 25, 25),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(192, 0, 0),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F)
                },
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                RowHeadersVisible = false
            };

            InitializeCartGrid();

            var totalsPanel = new Panel()
            {
                Size = new Size(500, 130),
                Location = new Point(650, 390),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            lblSubtotal = new Label()
            {
                Text = "Subtotal: $0.00",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            lblTax = new Label()
            {
                Text = "IVA (16%): $0.00",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 180),
                Location = new Point(20, 45),
                AutoSize = true
            };

            lblTotal = new Label()
            {
                Text = "TOTAL: $0.00",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 255, 127),
                Location = new Point(20, 75),
                AutoSize = true
            };

            totalsPanel.Controls.Add(lblSubtotal);
            totalsPanel.Controls.Add(lblTax);
            totalsPanel.Controls.Add(lblTotal);

            var lblPayment = new Label()
            {
                Text = "Método de Pago:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(650, 535),
                AutoSize = true
            };

            cmbPayment = new ComboBox()
            {
                Location = new Point(800, 532),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPayment.Items.AddRange(new[] { "Efectivo", "Tarjeta de Débito", "Tarjeta de Crédito", "Transferencia" });
            cmbPayment.SelectedIndex = 0;

            var btnRemoveFromCart = new IconButton()
            {
                IconChar = IconChar.TrashAlt,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Quitar",
                Size = new Size(150, 45),
                Location = new Point(650, 575),
                BackColor = Color.FromArgb(192, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnRemoveFromCart.FlatAppearance.BorderSize = 0;
            btnRemoveFromCart.Click += (s, e) =>
            {
                if (dgvCart.SelectedRows.Count > 0)
                {
                    int index = dgvCart.SelectedRows[0].Index;
                    cartItems.RemoveAt(index);
                    UpdateCartDisplay();
                }
            };

            var btnClearCart = new IconButton()
            {
                IconChar = IconChar.Broom,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Limpiar",
                Size = new Size(150, 45),
                Location = new Point(820, 575),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnClearCart.FlatAppearance.BorderSize = 0;
            btnClearCart.Click += (s, e) =>
            {
                cartItems.Clear();
                UpdateCartDisplay();
            };

            var btnCompleteSale = new IconButton()
            {
                IconChar = IconChar.CheckCircle,
                IconColor = Color.White,
                IconSize = 22,
                Text = "Completar Venta",
                Size = new Size(150, 45),
                Location = new Point(990, 575),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnCompleteSale.FlatAppearance.BorderSize = 0;
            btnCompleteSale.Click += CompleteSale;

            this.Controls.Add(lblProductsTitle);
            this.Controls.Add(searchPanel);
            this.Controls.Add(dgvProducts);
            this.Controls.Add(lblCartTitle);
            this.Controls.Add(controlsPanel);
            this.Controls.Add(dgvCart);
            this.Controls.Add(totalsPanel);
            this.Controls.Add(lblPayment);
            this.Controls.Add(cmbPayment);
            this.Controls.Add(btnRemoveFromCart);
            this.Controls.Add(btnClearCart);
            this.Controls.Add(btnCompleteSale);
        }

        private void InitializeCartGrid()
        {
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add("ProductName", "Producto");
            dgvCart.Columns.Add("Quantity", "Cant.");
            dgvCart.Columns.Add("Price", "Precio Unit.");
            dgvCart.Columns.Add("Subtotal", "Subtotal");
            
            dgvCart.Columns["ProductName"].Width = 200;
            dgvCart.Columns["Quantity"].Width = 70;
            dgvCart.Columns["Price"].Width = 100;
            dgvCart.Columns["Price"].DefaultCellStyle.Format = "C2";
            dgvCart.Columns["Subtotal"].Width = 100;
            dgvCart.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
        }

        private void LoadProducts()
        {
            try
            {
                var dt = db.GetAllProducts();
                if (dt == null || dt.Rows.Count == 0)
                {
                    dgvProducts.DataSource = null;
                    return;
                }

                dgvProducts.DataSource = null;
                dgvProducts.Columns.Clear();
                dgvProducts.DataSource = dt;
                
                if (dgvProducts.Columns.Count > 0)
                {
                    if (dgvProducts.Columns.Contains("Id"))
                        dgvProducts.Columns["Id"].Visible = false;
                    
                    if (dgvProducts.Columns.Contains("Name"))
                    {
                        dgvProducts.Columns["Name"].HeaderText = "Producto";
                        dgvProducts.Columns["Name"].Width = 200;
                    }
                    
                    if (dgvProducts.Columns.Contains("Price"))
                    {
                        dgvProducts.Columns["Price"].HeaderText = "Precio";
                        dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
                        dgvProducts.Columns["Price"].Width = 100;
                    }
                    
                    if (dgvProducts.Columns.Contains("Quantity"))
                    {
                        dgvProducts.Columns["Quantity"].HeaderText = "Stock";
                        dgvProducts.Columns["Quantity"].Width = 70;
                    }
                    
                    if (dgvProducts.Columns.Contains("Description"))
                        dgvProducts.Columns["Description"].Visible = false;
                    
                    if (dgvProducts.Columns.Contains("Category"))
                    {
                        dgvProducts.Columns["Category"].HeaderText = "Categoría";
                        dgvProducts.Columns["Category"].Width = 120;
                    }
                    
                    if (dgvProducts.Columns.Contains("Barcode"))
                        dgvProducts.Columns["Barcode"].Visible = false;
                }

                dgvProducts.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                var dt = db.GetAllProducts();
                if (dt == null) return;

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    var dv = dt.DefaultView;
                    dv.RowFilter = $"Name LIKE '%{txtSearch.Text}%' OR Category LIKE '%{txtSearch.Text}%'";
                    dgvProducts.DataSource = null;
                    dgvProducts.DataSource = dv.ToTable();
                }
                else
                {
                    dgvProducts.DataSource = null;
                    dgvProducts.DataSource = dt;
                }

                if (dgvProducts.Columns.Contains("Price"))
                    dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
            }
            catch { }
        }

        private void AddProductByBarcode()
        {
            string barcode = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(barcode)) return;

            try
            {
                var dt = db.GetAllProducts();
                var rows = dt.Select($"Barcode = '{barcode}'");
                
                if (rows.Length > 0)
                {
                    var row = rows[0];
                    AddProductToCart(
                        Convert.ToInt32(row["Id"]),
                        row["Name"].ToString()!,
                        Convert.ToDecimal(row["Price"]),
                        Convert.ToInt32(row["Quantity"])
                    );
                    txtBarcode.Clear();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSelectedProductToCart()
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var row = dgvProducts.SelectedRows[0];
                if (row.Cells["Id"].Value != null)
                {
                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    string name = row.Cells["Name"].Value.ToString()!;
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    int stock = Convert.ToInt32(row.Cells["Quantity"].Value);

                    AddProductToCart(id, name, price, stock);
                }
            }
        }

        private void AddProductToCart(int id, string name, decimal price, int stock)
        {
            if (stock <= 0)
            {
                MessageBox.Show("Producto sin stock disponible", "Sin Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qtyToAdd = (int)numQuantity.Value;

            var existingItem = cartItems.FirstOrDefault(x => x.ProductId == id);
            if (existingItem != null)
            {
                if (existingItem.Quantity + qtyToAdd <= stock)
                {
                    existingItem.Quantity += qtyToAdd;
                }
                else
                {
                    MessageBox.Show($"No hay suficiente stock. Disponible: {stock}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (qtyToAdd <= stock)
                {
                    cartItems.Add(new SaleItem
                    {
                        ProductId = id,
                        ProductName = name,
                        Price = price,
                        Quantity = qtyToAdd
                    });
                }
                else
                {
                    MessageBox.Show($"No hay suficiente stock. Disponible: {stock}", "Stock Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            UpdateCartDisplay();
            numQuantity.Value = 1;
        }

        private void UpdateCartDisplay()
        {
            dgvCart.Rows.Clear();
            decimal subtotal = 0;

            foreach (var item in cartItems)
            {
                dgvCart.Rows.Add(item.ProductName, item.Quantity, item.Price, item.Subtotal);
                subtotal += item.Subtotal;
            }

            decimal tax = subtotal * TAX_RATE;
            decimal total = subtotal + tax;

            lblSubtotal.Text = $"Subtotal: {subtotal:C2}";
            lblTax.Text = $"IVA (16%): {tax:C2}";
            lblTotal.Text = $"TOTAL: {total:C2}";
        }

        private void CompleteSale(object? sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("El carrito está vacío", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                decimal subtotal = cartItems.Sum(x => x.Subtotal);
                decimal tax = subtotal * TAX_RATE;
                decimal total = subtotal + tax;
                string paymentMethod = cmbPayment.SelectedItem?.ToString() ?? "Efectivo";

                int saleId = db.SaveSale(total, paymentMethod, cartItems);

                MessageBox.Show("Venta completada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var receiptForm = new ReceiptForm(db, saleId);
                receiptForm.ShowDialog();

                cartItems.Clear();
                UpdateCartDisplay();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al completar venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
