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
                        AccentColor TEXT NOT NULL DEFAULT '#00FF7F',
                        ReceiptHeader TEXT DEFAULT '',
                        ReceiptFooter TEXT DEFAULT '¡Gracias por su compra!'
                    );
                    CREATE TABLE IF NOT EXISTS CashRegisterSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        StartTime TEXT NOT NULL,
                        EndTime TEXT,
                        StartBalance REAL NOT NULL,
                        EndBalance REAL,
                        Status TEXT NOT NULL DEFAULT 'Open'
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
                    insertCommand.CommandText = @"INSERT INTO Settings (StoreName, Address, Phone, TaxRate, Theme, AccentColor, ReceiptHeader, ReceiptFooter) 
                                                  VALUES ('SEKAI Tech Store', 'Av. Principal #123, Ciudad', '+1 234 567 8900', 0.16, 'Dark', '#00FF7F', 'Bienvenido a SEKAI POS', '¡Gracias por su compra!')";
                    insertCommand.ExecuteNonQuery();
                }

                // Ensure new columns exist (migration for existing DB)
                try
                {
                    var alterCmd = connection.CreateCommand();
                    alterCmd.CommandText = "ALTER TABLE Settings ADD COLUMN ReceiptHeader TEXT DEFAULT ''";
                    alterCmd.ExecuteNonQuery();
                }
                catch { } // Ignore if exists

                try
                {
                    var alterCmd = connection.CreateCommand();
                    alterCmd.CommandText = "ALTER TABLE Settings ADD COLUMN ReceiptFooter TEXT DEFAULT '¡Gracias por su compra!'";
                    alterCmd.ExecuteNonQuery();
                }
                catch { } // Ignore if exists
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

        public void AddProductFull(string name, string description, decimal price, int quantity, string category, string barcode)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Products (Name, Description, Price, Quantity, Category, Barcode) VALUES (@name, @desc, @price, @qty, @cat, @bar)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@desc", description);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@qty", quantity);
                command.Parameters.AddWithValue("@cat", category);
                command.Parameters.AddWithValue("@bar", barcode);
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

        public void DeleteAllProducts()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Products";
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

        // NEW: Settings methods updated
        public (string StoreName, string Address, string Phone, decimal TaxRate, string ReceiptHeader, string ReceiptFooter) GetSettings()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT StoreName, Address, Phone, TaxRate, ReceiptHeader, ReceiptFooter FROM Settings LIMIT 1";

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3),
                            reader.IsDBNull(4) ? "" : reader.GetString(4),
                            reader.IsDBNull(5) ? "" : reader.GetString(5)
                        );
                    }
                }
            }
            return ("SEKAI Tech Store", "Av. Principal #123", "+1 234 567 8900", 0.16m, "", "");
        }

        public (string Theme, string AccentColor) GetThemeSettings()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Theme, AccentColor FROM Settings LIMIT 1";

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.IsDBNull(0) ? "Dark" : reader.GetString(0),
                            reader.IsDBNull(1) ? "#00FF7F" : reader.GetString(1)
                        );
                    }
                }
            }
            return ("Dark", "#00FF7F");
        }

        public void UpdateSettings(string storeName, string address, string phone, decimal taxRate, string header, string footer)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE Settings SET 
                                   StoreName = @name, 
                                   Address = @address, 
                                   Phone = @phone, 
                                   TaxRate = @tax,
                                   ReceiptHeader = @header,
                                   ReceiptFooter = @footer
                                   WHERE Id = 1";
                cmd.Parameters.AddWithValue("@name", storeName);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@tax", taxRate);
                cmd.Parameters.AddWithValue("@header", header);
                cmd.Parameters.AddWithValue("@footer", footer);
                cmd.ExecuteNonQuery();
            }
        }

        // NEW: Cash Register Methods
        public void OpenRegister(int userId, decimal startBalance)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO CashRegisterSessions (UserId, StartTime, StartBalance, Status) 
                                   VALUES (@userId, @startTime, @balance, 'Open')";
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@startTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@balance", startBalance);
                cmd.ExecuteNonQuery();
            }
        }

        public void CloseRegister(int sessionId, decimal endBalance)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE CashRegisterSessions SET 
                                   EndTime = @endTime, 
                                   EndBalance = @balance, 
                                   Status = 'Closed' 
                                   WHERE Id = @id";
                cmd.Parameters.AddWithValue("@endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@balance", endBalance);
                cmd.Parameters.AddWithValue("@id", sessionId);
                cmd.ExecuteNonQuery();
            }
        }

        public int? GetOpenRegisterSessionId()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Id FROM CashRegisterSessions WHERE Status = 'Open' ORDER BY Id DESC LIMIT 1";
                var result = cmd.ExecuteScalar();
                return result == null ? null : Convert.ToInt32(result);
            }
        }

        public bool IsRegisterOpen()
        {
            return GetOpenRegisterSessionId().HasValue;
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

        public void UpdateThemeSettings(string theme, string accentColor)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Settings SET Theme = @theme, AccentColor = @color WHERE Id = 1";
                cmd.Parameters.AddWithValue("@theme", theme);
                cmd.Parameters.AddWithValue("@color", accentColor);
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