using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SekaiPOS_1._0
{
    public class InventoryForm : Form
    {
        private DatabaseHelper db;
        private DataGridView dgvProducts = null!;
        private TextBox txtSearch = null!;
        private IconButton btnAdd = null!;
        private IconButton btnEdit = null!;
        private IconButton btnDelete = null!;
        private IconButton btnRefresh = null!;

        public InventoryForm(DatabaseHelper database)
        {
            db = database;
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(34, 33, 74);
            this.Padding = new Padding(20);

            // Search Panel
            var searchPanel = new Panel()
            {
                Size = new Size(this.Width - 40, 50),
                Location = new Point(20, 20),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblSearch = new Label()
            {
                Text = "Buscar:",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                Location = new Point(0, 15),
                AutoSize = true
            };

            txtSearch = new TextBox()
            {
                Location = new Point(70, 12),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(46, 45, 89),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += (s, e) => FilterProducts();

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);

            // Buttons Panel
            var buttonsPanel = new Panel()
            {
                Size = new Size(this.Width - 40, 50),
                Location = new Point(20, 80),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnAdd = CreateActionButton(IconChar.Plus, "Agregar", Color.FromArgb(46, 204, 113), 0);
            btnEdit = CreateActionButton(IconChar.Edit, "Editar", Color.FromArgb(52, 152, 219), 140);
            btnDelete = CreateActionButton(IconChar.TrashAlt, "Eliminar", Color.FromArgb(231, 76, 60), 280);
            btnRefresh = CreateActionButton(IconChar.ArrowsRotate, "Actualizar", Color.FromArgb(149, 165, 166), 420);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => LoadProducts();

            buttonsPanel.Controls.Add(btnAdd);
            buttonsPanel.Controls.Add(btnEdit);
            buttonsPanel.Controls.Add(btnDelete);
            buttonsPanel.Controls.Add(btnRefresh);

            // DataGridView
            dgvProducts = new DataGridView()
            {
                Location = new Point(20, 140),
                Size = new Size(this.Width - 40, this.Height - 180),
                BackgroundColor = Color.FromArgb(46, 45, 89),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(62, 61, 105),
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(37, 36, 81),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    SelectionBackColor = Color.FromArgb(37, 36, 81)
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(46, 45, 89),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(52, 152, 219),
                    SelectionForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F)
                },
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

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
                Size = new Size(130, 40),
                Location = new Point(x, 5),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadProducts()
        {
            try
            {
                dgvProducts.DataSource = db.GetAllProducts();
                
                if (dgvProducts.Columns.Count > 0)
                {
                    dgvProducts.Columns["Id"].HeaderText = "ID";
                    dgvProducts.Columns["Name"].HeaderText = "Nombre";
                    dgvProducts.Columns["Description"].HeaderText = "Descripción";
                    dgvProducts.Columns["Price"].HeaderText = "Precio";
                    dgvProducts.Columns["Quantity"].HeaderText = "Cantidad";

                    dgvProducts.Columns["Id"].Width = 50;
                    dgvProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
                }
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
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    var dv = dt.DefaultView;
                    dv.RowFilter = $"Name LIKE '%{txtSearch.Text}%' OR Description LIKE '%{txtSearch.Text}%'";
                    dgvProducts.DataSource = dv.ToTable();
                }
                else
                {
                    dgvProducts.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (var form = new ProductEditForm(db, null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var row = dgvProducts.SelectedRows[0];
                var productId = Convert.ToInt32(row.Cells["Id"].Value);
                
                using (var form = new ProductEditForm(db, productId))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un producto para editar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("¿Estás seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var productId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["Id"].Value);
                        db.DeleteProduct(productId);
                        LoadProducts();
                        MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona un producto para eliminar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
