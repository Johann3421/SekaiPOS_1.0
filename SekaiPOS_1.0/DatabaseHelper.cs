using Microsoft.Data.Sqlite;
using System.Data;

namespace SekaiPOS_1._0
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=pos.db";

        public DatabaseHelper()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        Price REAL NOT NULL,
                        Quantity INTEGER NOT NULL DEFAULT 0,
                        Category TEXT,
                        Barcode TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        IsAdmin INTEGER NOT NULL DEFAULT 0
                    );
                    CREATE TABLE IF NOT EXISTS Sales (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SaleDate TEXT NOT NULL,
                        Total REAL NOT NULL,
                        Username TEXT NOT NULL,
                        PaymentMethod TEXT
                    );
                    CREATE TABLE IF NOT EXISTS SaleItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SaleId INTEGER NOT NULL,
                        ProductId INTEGER NOT NULL,
                        ProductName TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Price REAL NOT NULL,
                        Subtotal REAL NOT NULL,
                        FOREIGN KEY (SaleId) REFERENCES Sales(Id)
                    );
                    CREATE TABLE IF NOT EXISTS Settings (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StoreName TEXT NOT NULL DEFAULT 'SEKAI Tech Store',
                        Address TEXT NOT NULL DEFAULT 'Av. Principal #123',
                        Phone TEXT NOT NULL DEFAULT '+1 234 567 8900',
                        TaxRate REAL NOT NULL DEFAULT 0.16,
                        Theme TEXT NOT NULL DEFAULT 'Dark',
                        AccentColor TEXT NOT NULL DEFAULT '#00FF7F'
                    );
                ";
                command.ExecuteNonQuery();

                // Insert super user admin if not exists
                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
                long count = (long)checkCommand.ExecuteScalar()!;
                if (count == 0)
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO Users (Username, Password, IsAdmin) VALUES ('admin', 'admin123', 1)";
                    insertCommand.ExecuteNonQuery();
                }

                // Insert default settings if not exists
                checkCommand.CommandText = "SELECT COUNT(*) FROM Settings";
                long settingsCount = (long)checkCommand.ExecuteScalar()!;
                if (settingsCount == 0)
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"INSERT INTO Settings (StoreName, Address, Phone, TaxRate, Theme, AccentColor) 
                                                  VALUES ('SEKAI Tech Store', 'Av. Principal #123, Ciudad', '+1 234 567 8900', 0.16, 'Dark', '#00FF7F')";
                    insertCommand.ExecuteNonQuery();
                }

                // Insert sample products for electronics store if empty
                checkCommand.CommandText = "SELECT COUNT(*) FROM Products";
                long productCount = (long)checkCommand.ExecuteScalar()!;
                if (productCount == 0)
                {
                    InsertSampleProducts(connection);
                }
            }
        }

        private void InsertSampleProducts(SqliteConnection connection)
        {
            var products = new[]
            {
                ("Laptop HP Pavilion 15", "Intel Core i5, 8GB RAM, 256GB SSD", 599.99m, 15, "Computadoras", "7501234567890"),
                ("Mouse Logitech MX Master 3", "Mouse inalámbrico ergonómico", 99.99m, 30, "Accesorios", "7501234567891"),
                ("Teclado Mecánico Corsair K95", "RGB, Cherry MX Red", 179.99m, 20, "Accesorios", "7501234567892"),
                ("Monitor Samsung 27\" 4K", "Panel IPS, 60Hz", 349.99m, 12, "Monitores", "7501234567893"),
                ("Auriculares Sony WH-1000XM4", "Cancelación de ruido activa", 299.99m, 25, "Audio", "7501234567894"),
                ("SSD Kingston 1TB NVMe", "Lectura 3500MB/s", 89.99m, 40, "Almacenamiento", "7501234567895"),
                ("Memoria RAM Corsair 16GB DDR4", "3200MHz", 79.99m, 35, "Componentes", "7501234567896"),
                ("Webcam Logitech C920", "Full HD 1080p", 69.99m, 18, "Accesorios", "7501234567897"),
                ("Router TP-Link Archer AX50", "WiFi 6, Dual Band", 129.99m, 22, "Redes", "7501234567898"),
                ("Impresora HP LaserJet Pro", "Monocromática, WiFi", 199.99m, 10, "Impresoras", "7501234567899"),
                ("Hub USB-C Anker 7-en-1", "HDMI, USB 3.0, SD", 49.99m, 50, "Accesorios", "7501234567900"),
                ("Disco Duro Externo Seagate 2TB", "USB 3.0, Portátil", 69.99m, 28, "Almacenamiento", "7501234567901"),
                ("Tarjeta Gráfica NVIDIA RTX 3060", "12GB GDDR6", 399.99m, 8, "Componentes", "7501234567902"),
                ("Fuente de Poder EVGA 750W", "80+ Gold Modular", 119.99m, 15, "Componentes", "7501234567903"),
                ("Case Corsair 4000D Airflow", "Mid Tower ATX", 89.99m, 12, "Componentes", "7501234567904"),
                ("Tablet Samsung Galaxy Tab S7", "11\", 128GB", 549.99m, 10, "Tablets", "7501234567905"),
                ("Smartwatch Apple Watch Series 7", "GPS, 45mm", 399.99m, 14, "Wearables", "7501234567906"),
                ("Cámara Web Razer Kiyo", "Ring Light integrado", 99.99m, 16, "Accesorios", "7501234567907"),
                ("Micrófono Blue Yeti", "USB, Multipropósito", 129.99m, 20, "Audio", "7501234567908"),
                ("Cable HDMI 2.1 Belkin 2m", "8K, 48Gbps", 29.99m, 60, "Cables", "7501234567909")
            };

            foreach (var (name, desc, price, qty, category, barcode) in products)
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Products (Name, Description, Price, Quantity, Category, Barcode) 
                                   VALUES (@name, @desc, @price, @qty, @category, @barcode)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@barcode", barcode);
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAllProducts()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Products ORDER BY Name";

                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }

        public void AddProduct(string name, string description, decimal price, int quantity)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Products (Name, Description, Price, Quantity) VALUES (@name, @desc, @price, @qty)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@desc", description);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@qty", quantity);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(int id, string name, string description, decimal price, int quantity)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Products SET Name = @name, Description = @desc, Price = @price, Quantity = @qty WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@desc", description);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@qty", quantity);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Products WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(string username, string password)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @user AND Password = @pass";
                command.Parameters.AddWithValue("@user", username);
                command.Parameters.AddWithValue("@pass", password);
                long count = (long)command.ExecuteScalar()!;
                return count > 0;
            }
        }

        public bool IsUserAdmin(string username)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT IsAdmin FROM Users WHERE Username = @user";
                command.Parameters.AddWithValue("@user", username);
                var result = command.ExecuteScalar();
                return result != null && (long)result == 1;
            }
        }

        public int SaveSale(decimal total, string paymentMethod, List<SaleItem> items)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var saleCmd = connection.CreateCommand();
                        saleCmd.CommandText = @"INSERT INTO Sales (SaleDate, Total, Username, PaymentMethod) 
                                               VALUES (@date, @total, @user, @payment);
                                               SELECT last_insert_rowid();";
                        saleCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        saleCmd.Parameters.AddWithValue("@total", total);
                        saleCmd.Parameters.AddWithValue("@user", CurrentUser.Username);
                        saleCmd.Parameters.AddWithValue("@payment", paymentMethod);
                        
                        long saleId = (long)saleCmd.ExecuteScalar()!;

                        foreach (var item in items)
                        {
                            var itemCmd = connection.CreateCommand();
                            itemCmd.CommandText = @"INSERT INTO SaleItems (SaleId, ProductId, ProductName, Quantity, Price, Subtotal)
                                                   VALUES (@saleId, @productId, @name, @qty, @price, @subtotal)";
                            itemCmd.Parameters.AddWithValue("@saleId", saleId);
                            itemCmd.Parameters.AddWithValue("@productId", item.ProductId);
                            itemCmd.Parameters.AddWithValue("@name", item.ProductName);
                            itemCmd.Parameters.AddWithValue("@qty", item.Quantity);
                            itemCmd.Parameters.AddWithValue("@price", item.Price);
                            itemCmd.Parameters.AddWithValue("@subtotal", item.Subtotal);
                            itemCmd.ExecuteNonQuery();

                            var updateCmd = connection.CreateCommand();
                            updateCmd.CommandText = "UPDATE Products SET Quantity = Quantity - @qty WHERE Id = @id";
                            updateCmd.Parameters.AddWithValue("@qty", item.Quantity);
                            updateCmd.Parameters.AddWithValue("@id", item.ProductId);
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return (int)saleId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable GetSaleDetails(int saleId)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT ProductName, Quantity, Price, Subtotal 
                                   FROM SaleItems WHERE SaleId = @id";
                cmd.Parameters.AddWithValue("@id", saleId);
                
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public (string Date, decimal Total, string PaymentMethod) GetSaleInfo(int saleId)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT SaleDate, Total, PaymentMethod FROM Sales WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", saleId);
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (reader.GetString(0), reader.GetDecimal(1), reader.GetString(2));
                    }
                }
            }
            return (string.Empty, 0, string.Empty);
        }

        public int GetTotalProducts()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Products";
                return Convert.ToInt32((long)cmd.ExecuteScalar()!);
            }
        }

        public decimal GetTodaySales()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COALESCE(SUM(Total), 0) FROM Sales WHERE DATE(SaleDate) = DATE('now')";
                return Convert.ToDecimal(cmd.ExecuteScalar()!);
            }
        }

        public int GetTotalSales()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Sales";
                return Convert.ToInt32((long)cmd.ExecuteScalar()!);
            }
        }

        // NEW: Settings methods
        public (string StoreName, string Address, string Phone, decimal TaxRate) GetSettings()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT StoreName, Address, Phone, TaxRate FROM Settings LIMIT 1";
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3)
                        );
                    }
                }
            }
            return ("SEKAI Tech Store", "Av. Principal #123", "+1 234 567 8900", 0.16m);
        }

        public void UpdateSettings(string storeName, string address, string phone, decimal taxRate)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE Settings SET 
                                   StoreName = @name, 
                                   Address = @address, 
                                   Phone = @phone, 
                                   TaxRate = @tax 
                                   WHERE Id = 1";
                cmd.Parameters.AddWithValue("@name", storeName);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@tax", taxRate);
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAllUsers()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Id, Username, IsAdmin FROM Users ORDER BY Username";
                
                using (var reader = cmd.ExecuteReader())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public void AddUser(string username, string password, bool isAdmin)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@user, @pass, @admin)";
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);
                cmd.Parameters.AddWithValue("@admin", isAdmin ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public void ChangePassword(string username, string newPassword)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Users SET Password = @pass WHERE Username = @user";
                cmd.Parameters.AddWithValue("@pass", newPassword);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class SaleItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }
}