using FontAwesome.Sharp;
using OfficeOpenXml;

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
        private IconButton btnImport = null!;
        private IconButton btnClearAll = null!;

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
                Size = new Size(1100, 60),
                Location = new Point(20, 125),
                BackColor = Color.Transparent
            };

            btnAdd = CreateActionButton(IconChar.Plus, "Agregar", Color.FromArgb(0, 200, 83), 0);
            btnEdit = CreateActionButton(IconChar.Edit, "Editar", Color.FromArgb(0, 120, 212), 160);
            btnDelete = CreateActionButton(IconChar.TrashAlt, "Eliminar", Color.FromArgb(192, 0, 0), 320);
            btnRefresh = CreateActionButton(IconChar.ArrowsRotate, "Actualizar", Color.FromArgb(100, 100, 100), 480);
            btnImport = CreateActionButton(IconChar.FileImport, "Importar CSV", Color.FromArgb(255, 159, 10), 640);
            btnClearAll = CreateActionButton(IconChar.Broom, "Limpiar Todo", Color.FromArgb(220, 20, 60), 800);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => LoadProducts();
            btnImport.Click += BtnImport_Click;
            btnClearAll.Click += BtnClearAll_Click;

            buttonsPanel.Controls.Add(btnAdd);
            buttonsPanel.Controls.Add(btnEdit);
            buttonsPanel.Controls.Add(btnDelete);
            buttonsPanel.Controls.Add(btnRefresh);
            buttonsPanel.Controls.Add(btnImport);
            buttonsPanel.Controls.Add(btnClearAll);

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

        private void BtnClearAll_Click(object? sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "?? ADVERTENCIA ??\n\n¿Estás ABSOLUTAMENTE SEGURO de eliminar TODOS los productos?\n\nEsta acción NO se puede deshacer.",
                    "Confirmar Eliminación Total",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    // Segunda confirmación
                    var result2 = MessageBox.Show(
                        "Esta es la última confirmación.\n\n¿Realmente deseas borrar TODOS los productos de la base de datos?",
                        "Última Confirmación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop,
                        MessageBoxDefaultButton.Button2);

                    if (result2 == DialogResult.Yes)
                    {
                        db.DeleteAllProducts();
                        LoadProducts();
                        MessageBox.Show("Todos los productos han sido eliminados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImport_Click(object? sender, EventArgs e)
        {
            // Configurar EPPlus para uso no comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos soportados|*.csv;*.xlsx;*.xls|CSV Files (*.csv)|*.csv|Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";
                openFileDialog.Title = "Importar Productos desde CSV o Excel";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Preguntar si desea limpiar la base de datos primero
                        var clearResult = MessageBox.Show(
                            "¿Desea borrar todos los productos existentes antes de importar?\n\nSí = Borrar todo y importar\nNo = Agregar a los productos existentes\nCancelar = Cancelar importación",
                            "Confirmar Importación",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);

                        if (clearResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        if (clearResult == DialogResult.Yes)
                        {
                            db.DeleteAllProducts();
                        }

                        string fileExtension = System.IO.Path.GetExtension(openFileDialog.FileName).ToLower();

                        if (fileExtension == ".csv")
                        {
                            ImportFromCSV(openFileDialog.FileName);
                        }
                        else if (fileExtension == ".xlsx" || fileExtension == ".xls")
                        {
                            ImportFromExcel(openFileDialog.FileName);
                        }
                        else
                        {
                            MessageBox.Show("Formato de archivo no soportado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al importar archivo:\n{ex.Message}\n\nPor favor verifica que:\n- El archivo sea válido\n- Use el formato correcto\n- La primera línea sea el encabezado",
                            "Error de Importación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ImportFromCSV(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
            if (lines.Length < 2)
            {
                MessageBox.Show("El archivo está vacío o no tiene datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int importedCount = 0;
            int errorCount = 0;
            var errorDetails = new System.Text.StringBuilder();

            // Formato esperado: Name,Description,Price,Quantity,Category,Barcode
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    // Detectar separador (coma o punto y coma)
                    char separator = line.Contains(';') ? ';' : ',';
                    var parts = line.Split(separator);

                    if (parts.Length < 4)
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Línea {i + 1}: Faltan columnas (mínimo: Name,Description,Price,Quantity)");
                        continue;
                    }

                    string name = parts[0].Trim().Trim('"');
                    string description = parts.Length > 1 ? parts[1].Trim().Trim('"') : "";

                    // Parseo mejorado de precio
                    string priceStr = parts.Length > 2 ? parts[2].Trim().Replace("\"", "") : "0";
                    priceStr = priceStr.Replace(",", ".");

                    if (!decimal.TryParse(priceStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price))
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Línea {i + 1}: Precio inválido '{parts[2].Trim()}'");
                        continue;
                    }

                    string quantityStr = parts.Length > 3 ? parts[3].Trim().Replace("\"", "") : "0";
                    if (!int.TryParse(quantityStr, out int quantity))
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Línea {i + 1}: Cantidad inválida '{parts[3].Trim()}'");
                        continue;
                    }

                    string category = parts.Length > 4 ? parts[4].Trim().Trim('"') : "General";
                    string barcode = parts.Length > 5 ? parts[5].Trim().Trim('"') : "";

                    // Validación básica
                    if (string.IsNullOrEmpty(name))
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Línea {i + 1}: Nombre de producto vacío");
                        continue;
                    }

                    if (price <= 0)
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Línea {i + 1}: Precio debe ser mayor a 0");
                        continue;
                    }

                    db.AddProductFull(name, description, price, quantity, category, barcode);
                    importedCount++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    errorDetails.AppendLine($"Línea {i + 1}: {ex.Message}");
                }
            }

            LoadProducts();
            ShowImportResults(importedCount, errorCount, errorDetails.ToString());
        }

        private void ImportFromExcel(string filePath)
        {
            int importedCount = 0;
            int errorCount = 0;
            var errorDetails = new System.Text.StringBuilder();

            using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    MessageBox.Show("El archivo Excel no contiene hojas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var worksheet = package.Workbook.Worksheets[0]; // Primera hoja
                int rowCount = worksheet.Dimension?.Rows ?? 0;

                if (rowCount < 2)
                {
                    MessageBox.Show("El archivo Excel está vacío o no tiene datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Formato esperado:
                // Columna A: Name
                // Columna B: Description
                // Columna C: Price
                // Columna D: Quantity
                // Columna E: Category
                // Columna F: Barcode

                for (int row = 2; row <= rowCount; row++) // Empezar desde fila 2 (fila 1 = encabezado)
                {
                    try
                    {
                        var name = worksheet.Cells[row, 1].Text?.Trim();
                        var description = worksheet.Cells[row, 2].Text?.Trim() ?? "";
                        var priceStr = worksheet.Cells[row, 3].Text?.Trim();
                        var quantityStr = worksheet.Cells[row, 4].Text?.Trim();
                        var category = worksheet.Cells[row, 5].Text?.Trim() ?? "General";
                        var barcode = worksheet.Cells[row, 6].Text?.Trim() ?? "";

                        // Saltar filas vacías
                        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(priceStr))
                        {
                            continue;
                        }

                        // Validar nombre
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            errorCount++;
                            errorDetails.AppendLine($"Fila {row}: Nombre de producto vacío");
                            continue;
                        }

                        // Parsear precio
                        if (string.IsNullOrWhiteSpace(priceStr))
                        {
                            errorCount++;
                            errorDetails.AppendLine($"Fila {row}: Precio vacío");
                            continue;
                        }

                        priceStr = priceStr.Replace(",", ".");
                        if (!decimal.TryParse(priceStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal price))
                        {
                            errorCount++;
                            errorDetails.AppendLine($"Fila {row}: Precio inválido '{priceStr}'");
                            continue;
                        }

                        if (price <= 0)
                        {
                            errorCount++;
                            errorDetails.AppendLine($"Fila {row}: Precio debe ser mayor a 0");
                            continue;
                        }

                        // Parsear cantidad
                        if (!int.TryParse(quantityStr, out int quantity))
                        {
                            quantity = 0; // Cantidad por defecto
                        }

                        db.AddProductFull(name, description, price, quantity, category, barcode);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorDetails.AppendLine($"Fila {row}: {ex.Message}");
                    }
                }
            }

            LoadProducts();
            ShowImportResults(importedCount, errorCount, errorDetails.ToString());
        }

        private void ShowImportResults(int importedCount, int errorCount, string errorDetails)
        {
            string message = $"Importación completada:\n\n? {importedCount} productos importados exitosamente";
            if (errorCount > 0)
            {
                message += $"\n? {errorCount} errores encontrados\n\nDetalles de errores:\n{errorDetails}";
            }

            MessageBox.Show(message, "Resultado de Importación", MessageBoxButtons.OK,
                errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }
    }
}
