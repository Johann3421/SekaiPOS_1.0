using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class InventoryFormFinal : Form
    {
        private DatabaseHelper db;
        private DataGridView dgvProducts = null!;
        private TextBox txtSearch = null!;
        private IconButton btnAdd = null!;
        private IconButton btnEdit = null!;
        private IconButton btnDelete = null!;
        private IconButton btnRefresh = null!;

        public InventoryFormFinal(DatabaseHelper database)
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

            var lblTitle = new Label()
            {
                Text = "Gestión de Inventario",
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

            // CORREGIDO: Icono de búsqueda como IconPictureBox
            var iconSearch = new IconPictureBox()
            {
                IconChar = IconChar.MagnifyingGlass,
                IconColor = Color.White,
                IconSize = 20,
                Size = new Size(20, 20),
                Location = new Point(15, 15),
                BackColor = Color.Transparent
            };

            var lblSearch = new Label()
            {
                Text = "Buscar:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(45, 15),
                AutoSize = true
            };

            txtSearch = new TextBox()
            {
                Location = new Point(130, 12),
                Size = new Size(450, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += (s, e) => FilterProducts();

            searchPanel.Controls.Add(iconSearch);
            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);

            var buttonsPanel = new Panel()
            {
                Size = new Size(640, 60),
                Location = new Point(20, 125),
                BackColor = Color.Transparent
            };

            btnAdd = CreateActionButton(IconChar.Plus, "Agregar", Color.FromArgb(0, 200, 83), 0);
            btnEdit = CreateActionButton(IconChar.Edit, "Editar", Color.FromArgb(0, 120, 212), 160);
            btnDelete = CreateActionButton(IconChar.TrashAlt, "Eliminar", Color.FromArgb(192, 0, 0), 320);
            btnRefresh = CreateActionButton(IconChar.ArrowsRotate, "Actualizar", Color.FromArgb(100, 100, 100), 480);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => LoadProducts();

            buttonsPanel.Controls.Add(btnAdd);
            buttonsPanel.Controls.Add(btnEdit);
            buttonsPanel.Controls.Add(btnDelete);
            buttonsPanel.Controls.Add(btnRefresh);

            dgvProducts = new DataGridView()
            {
                Location = new Point(20, 200),
                Size = new Size(1100, 400),
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
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                RowHeadersVisible = false
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(searchPanel);
            this.Controls.Add(buttonsPanel);
            this.Controls.Add(dgvProducts);
        }

        private IconButton CreateActionButton(IconChar icon, string text, Color color, int x)
        {
            var button = new IconButton()
            {
                IconChar = icon,
                IconColor = Color.White,
                IconSize = 20,
                Text = text,
                Size = new Size(150, 45),
                Location = new Point(x, 5),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadProducts()
        {
            try
            {
                if (db == null) return;

                var dt = db.GetAllProducts();
                if (dt == null || dt.Rows.Count == 0)
                {
                    dgvProducts.DataSource = null;
                    dgvProducts.Columns.Clear();
                    return;
                }

                dgvProducts.DataSource = null;
                dgvProducts.Columns.Clear();
                dgvProducts.DataSource = dt;
                
                if (dgvProducts.Columns.Count > 0)
                {
                    if (dgvProducts.Columns.Contains("Id"))
                    {
                        dgvProducts.Columns["Id"].HeaderText = "ID";
                        dgvProducts.Columns["Id"].Width = 50;
                    }
                    
                    if (dgvProducts.Columns.Contains("Name"))
                    {
                        dgvProducts.Columns["Name"].HeaderText = "Nombre del Producto";
                        dgvProducts.Columns["Name"].Width = 250;
                    }
                    
                    if (dgvProducts.Columns.Contains("Description"))
                    {
                        dgvProducts.Columns["Description"].HeaderText = "Descripción";
                        dgvProducts.Columns["Description"].Width = 300;
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
                        dgvProducts.Columns["Quantity"].Width = 80;
                    }
                    
                    if (dgvProducts.Columns.Contains("Category"))
                    {
                        dgvProducts.Columns["Category"].HeaderText = "Categoría";
                        dgvProducts.Columns["Category"].Width = 120;
                    }
                    
                    if (dgvProducts.Columns.Contains("Barcode"))
                    {
                        dgvProducts.Columns["Barcode"].HeaderText = "Código de Barras";
                        dgvProducts.Columns["Barcode"].Width = 130;
                    }
                }

                dgvProducts.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                if (db == null) return;

                var dt = db.GetAllProducts();
                if (dt == null) return;

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    var dv = dt.DefaultView;
                    dv.RowFilter = $"Name LIKE '%{txtSearch.Text}%' OR Description LIKE '%{txtSearch.Text}%' OR Category LIKE '%{txtSearch.Text}%'";
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

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                using (var form = new ProductEditForm(db, null))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows.Count > 0)
                {
                    var row = dgvProducts.SelectedRows[0];
                    if (row.Cells["Id"].Value != null)
                    {
                        var productId = Convert.ToInt32(row.Cells["Id"].Value);
                        using (var form = new ProductEditForm(db, productId))
                        {
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                LoadProducts();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un producto para editar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows.Count > 0)
                {
                    var result = MessageBox.Show("¿Estás seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        var row = dgvProducts.SelectedRows[0];
                        if (row.Cells["Id"].Value != null)
                        {
                            var productId = Convert.ToInt32(row.Cells["Id"].Value);
                            db.DeleteProduct(productId);
                            LoadProducts();
                            MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un producto para eliminar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
