using FontAwesome.Sharp;
using System.Data;

namespace SekaiPOS_1._0
{
    public class ReportsForm : Form
    {
        private DatabaseHelper db;
        private DataGridView dgvSales = null!;
        private Label lblTotalSales = null!;
        private Label lblTotalRevenue = null!;
        private Label lblAverageTicket = null!;
        private Label lblBestSeller = null!;
        private DateTimePicker dtpStart = null!;
        private DateTimePicker dtpEnd = null!;
        private IconButton btnFilter = null!;
        private IconButton btnExport = null!;

        public ReportsForm(DatabaseHelper database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            db = database;
            InitializeComponent();

            // Load reports after form is shown
            this.Shown += (s, e) => LoadReports();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.Padding = new Padding(20);

            // Title
            var lblTitle = new Label()
            {
                Text = "Reportes y Estadísticas",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Date filter panel
            var filterPanel = new Panel()
            {
                Size = new Size(800, 70),
                Location = new Point(30, 70),
                BackColor = Color.FromArgb(25, 25, 25)
            };

            var lblFrom = new Label()
            {
                Text = "Desde:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            dtpStart = new DateTimePicker()
            {
                Location = new Point(90, 17),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };

            var lblTo = new Label()
            {
                Text = "Hasta:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(320, 20),
                AutoSize = true
            };

            dtpEnd = new DateTimePicker()
            {
                Location = new Point(390, 17),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            btnFilter = new IconButton()
            {
                IconChar = IconChar.Filter,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Filtrar",
                Size = new Size(120, 40),
                Location = new Point(620, 12),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += (s, e) => LoadReports();

            filterPanel.Controls.Add(lblFrom);
            filterPanel.Controls.Add(dtpStart);
            filterPanel.Controls.Add(lblTo);
            filterPanel.Controls.Add(dtpEnd);
            filterPanel.Controls.Add(btnFilter);

            // Statistics cards
            var card1 = CreateStatsCard("Total de Ventas", "0", Color.FromArgb(0, 120, 212), 30, 160, out lblTotalSales);
            var card2 = CreateStatsCard("Ingresos Totales", "$0.00", Color.FromArgb(0, 200, 83), 310, 160, out lblTotalRevenue);
            var card3 = CreateStatsCard("Ticket Promedio", "$0.00", Color.FromArgb(255, 159, 10), 590, 160, out lblAverageTicket);
            var card4 = CreateStatsCard("Producto Más Vendido", "N/A", Color.FromArgb(156, 39, 176), 870, 160, out lblBestSeller);

            // Sales table
            var lblSalesTable = new Label()
            {
                Text = "Historial de Ventas",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 320),
                AutoSize = true
            };

            dgvSales = new DataGridView()
            {
                Location = new Point(30, 360),
                Size = new Size(1090, 200),
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

            btnExport = new IconButton()
            {
                IconChar = IconChar.FileExcel,
                IconColor = Color.White,
                IconSize = 22,
                Text = "Exportar a CSV",
                Size = new Size(180, 45),
                Location = new Point(940, 575),
                BackColor = Color.FromArgb(0, 200, 83),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(filterPanel);
            this.Controls.Add(card1);
            this.Controls.Add(card2);
            this.Controls.Add(card3);
            this.Controls.Add(card4);
            this.Controls.Add(lblSalesTable);
            this.Controls.Add(dgvSales);
            this.Controls.Add(btnExport);
        }

        private Panel CreateStatsCard(string title, string value, Color accentColor, int x, int y, out Label lblValue)
        {
            var card = new Panel()
            {
                Size = new Size(260, 130),
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

            var lblTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 200, 200),
                Location = new Point(15, 15),
                Size = new Size(230, 40),
                BackColor = Color.Transparent
            };

            lblValue = new Label()
            {
                Text = value,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(15, 65),
                Size = new Size(230, 50),
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private void LoadReports()
        {
            try
            {
                if (db == null) return;

                var dt = GetSalesInRange();

                // Clear existing data
                dgvSales.DataSource = null;
                dgvSales.Columns.Clear();

                // Set new data
                dgvSales.DataSource = dt;

                if (dgvSales.Columns.Count > 0 && dt.Rows.Count > 0)
                {
                    if (dgvSales.Columns.Contains("Id"))
                    {
                        dgvSales.Columns["Id"].HeaderText = "ID Venta";
                        dgvSales.Columns["Id"].Width = 80;
                    }

                    if (dgvSales.Columns.Contains("SaleDate"))
                    {
                        dgvSales.Columns["SaleDate"].HeaderText = "Fecha";
                        dgvSales.Columns["SaleDate"].Width = 150;
                    }

                    if (dgvSales.Columns.Contains("Total"))
                    {
                        dgvSales.Columns["Total"].HeaderText = "Total";
                        dgvSales.Columns["Total"].DefaultCellStyle.Format = "C2";
                        dgvSales.Columns["Total"].Width = 120;
                    }

                    if (dgvSales.Columns.Contains("Username"))
                    {
                        dgvSales.Columns["Username"].HeaderText = "Usuario";
                        dgvSales.Columns["Username"].Width = 120;
                    }

                    if (dgvSales.Columns.Contains("PaymentMethod"))
                    {
                        dgvSales.Columns["PaymentMethod"].HeaderText = "Método de Pago";
                        dgvSales.Columns["PaymentMethod"].Width = 150;
                    }
                }

                int totalSales = dt.Rows.Count;
                decimal totalRevenue = 0;

                foreach (DataRow row in dt.Rows)
                {
                    totalRevenue += Convert.ToDecimal(row["Total"]);
                }

                decimal avgTicket = totalSales > 0 ? totalRevenue / totalSales : 0;

                lblTotalSales.Text = totalSales.ToString();
                lblTotalRevenue.Text = totalRevenue.ToString("C2");
                lblAverageTicket.Text = avgTicket.ToString("C2");
                lblBestSeller.Text = GetBestSeller();

                dgvSales.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reportes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetSalesInRange()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("SaleDate", typeof(string));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("PaymentMethod", typeof(string));

            try
            {
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=pos.db"))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = @"SELECT Id, SaleDate, Total, Username, PaymentMethod 
                                       FROM Sales 
                                       WHERE DATE(SaleDate) BETWEEN DATE(@start) AND DATE(@end)
                                       ORDER BY SaleDate DESC";
                    cmd.Parameters.AddWithValue("@start", dtpStart.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@end", dtpEnd.Value.ToString("yyyy-MM-dd"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dt.Rows.Add(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetDecimal(2),
                                reader.GetString(3),
                                reader.GetString(4)
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener ventas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        private string GetBestSeller()
        {
            try
            {
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=pos.db"))
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = @"SELECT ProductName, SUM(Quantity) as TotalSold 
                                       FROM SaleItems 
                                       GROUP BY ProductName 
                                       ORDER BY TotalSold DESC 
                                       LIMIT 1";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }
            catch { }

            return "N/A";
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog()
                {
                    Filter = "Archivo CSV|*.csv",
                    Title = "Exportar Reporte",
                    FileName = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd}.csv"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new System.IO.StreamWriter(saveDialog.FileName))
                    {
                        // Header
                        writer.WriteLine("ID,Fecha,Total,Usuario,Método de Pago");

                        // Data
                        foreach (DataGridViewRow row in dgvSales.Rows)
                        {
                            if (row.Cells["Id"].Value != null)
                            {
                                writer.WriteLine($"{row.Cells["Id"].Value},{row.Cells["SaleDate"].Value},{row.Cells["Total"].Value},{row.Cells["Username"].Value},{row.Cells["PaymentMethod"].Value}");
                            }
                        }
                    }

                    MessageBox.Show("Reporte exportado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
