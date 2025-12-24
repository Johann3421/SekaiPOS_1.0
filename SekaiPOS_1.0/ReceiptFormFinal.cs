using FontAwesome.Sharp;
using System.Drawing.Printing;

namespace SekaiPOS_1._0
{
    public class ReceiptFormFinal : Form
    {
        private DatabaseHelper db;
        private int saleId;
        private RichTextBox txtReceipt = null!;
        private PrintDocument printDocument = null!;

        public ReceiptFormFinal(DatabaseHelper database, int id)
        {
            db = database;
            saleId = id;
            InitializeComponent();
            GenerateReceipt();
        }

        private void InitializeComponent()
        {
            this.Text = "Boleta de Venta";
            this.Size = new Size(400, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(34, 33, 74);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblTitle = new Label()
            {
                Text = "Boleta de Venta",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtReceipt = new RichTextBox()
            {
                Location = new Point(20, 60),
                Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 180),
                Font = new Font("Courier New", 9F),
                BackColor = Color.White,
                ForeColor = Color.Black,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnPrint = new IconButton()
            {
                IconChar = IconChar.Print,
                IconColor = Color.White,
                IconSize = 20,
                Text = "Imprimir",
                Size = new Size(160, 45),
                Location = new Point(20, this.ClientSize.Height - 100),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

            var btnClose = new Button()
            {
                Text = "Cerrar",
                Size = new Size(160, 45),
                Location = new Point(200, this.ClientSize.Height - 100),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtReceipt);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void GenerateReceipt()
        {
            var (date, total, paymentMethod) = db.GetSaleInfo(saleId);
            var items = db.GetSaleDetails(saleId);
            var settings = db.GetSettings();

            var receipt = new System.Text.StringBuilder();
            receipt.AppendLine("========================================");

            // Center Store Name
            string storeName = settings.StoreName.Length > 40 ? settings.StoreName.Substring(0, 40) : settings.StoreName;
            int padding = Math.Max(0, (40 - storeName.Length) / 2);
            receipt.AppendLine(storeName.PadLeft(storeName.Length + padding));

            // Address and Phone
            if (!string.IsNullOrEmpty(settings.Address))
            {
                string addr = settings.Address.Length > 40 ? settings.Address.Substring(0, 40) : settings.Address;
                receipt.AppendLine(addr.PadLeft(addr.Length + Math.Max(0, (40 - addr.Length) / 2)));
            }
            if (!string.IsNullOrEmpty(settings.Phone))
            {
                string phone = settings.Phone;
                receipt.AppendLine(phone.PadLeft(phone.Length + Math.Max(0, (40 - phone.Length) / 2)));
            }

            receipt.AppendLine("========================================");

            if (!string.IsNullOrEmpty(settings.ReceiptHeader))
            {
                receipt.AppendLine(settings.ReceiptHeader);
                receipt.AppendLine("========================================");
            }

            receipt.AppendLine();
            receipt.AppendLine($"Boleta No: {saleId:D6}");
            receipt.AppendLine($"Fecha: {DateTime.Parse(date):dd/MM/yyyy HH:mm:ss}");
            receipt.AppendLine($"Cajero: {CurrentUser.Username}");
            receipt.AppendLine();
            receipt.AppendLine("========================================");
            receipt.AppendLine("PRODUCTO             CANT  PRECIO  SUBT");
            receipt.AppendLine("========================================");

            foreach (System.Data.DataRow row in items.Rows)
            {
                string name = row["ProductName"].ToString()!;
                int qty = Convert.ToInt32(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["Price"]);
                decimal subtotal = Convert.ToDecimal(row["Subtotal"]);

                if (name.Length > 20)
                    name = name.Substring(0, 17) + "...";

                receipt.AppendLine($"{name,-20} {qty,4}  ${price,6:F2} ${subtotal,6:F2}");
            }

            receipt.AppendLine("========================================");
            receipt.AppendLine();
            receipt.AppendLine($"                   TOTAL:  ${total,10:F2}");
            receipt.AppendLine();
            receipt.AppendLine($"Método de Pago: {paymentMethod}");
            receipt.AppendLine();
            receipt.AppendLine("========================================");

            if (!string.IsNullOrEmpty(settings.ReceiptFooter))
            {
                receipt.AppendLine(settings.ReceiptFooter);
            }
            else
            {
                receipt.AppendLine("      ¡GRACIAS POR SU COMPRA!         ");
            }

            receipt.AppendLine("========================================");

            txtReceipt.Text = receipt.ToString();
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                    MessageBox.Show("Boleta enviada a imprimir", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object? sender, PrintPageEventArgs e)
        {
            if (e.Graphics != null)
            {
                Font font = new Font("Courier New", 9);
                float yPos = 50;
                float leftMargin = e.MarginBounds.Left;

                foreach (string line in txtReceipt.Lines)
                {
                    e.Graphics.DrawString(line, font, Brushes.Black, leftMargin, yPos);
                    yPos += font.GetHeight(e.Graphics);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                printDocument?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
