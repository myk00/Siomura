using MySql.Data.MySqlClient;
using System;

namespace Siomura
{
    public class SiomuraDB
    {
        private string connectionString = "server=10.0.2.2;port=3306;database=SiomuraDB;user=root;password=toytoynitutoy21;";

        public bool SaveOrder(string name, string contact, string productName, int quantity, decimal totalPrice, string paymentMethod)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    
                    using (var dbCmd = new MySqlCommand("SELECT DATABASE();", conn))
                    {
                        Console.WriteLine("✅ Connected to DB: " + dbCmd.ExecuteScalar());
                    }

                    
                    int customerId = GetOrInsertCustomer(conn, name, contact);

                    
                    int productId = GetId(conn, "products", "product_name", productName);

                    
                    int paymentId = GetId(conn, "payments", "payment_method", paymentMethod);

                    
                    string orderQuery = @"
                        INSERT INTO orders_updated (customer_id, product_id, payment_id, quantity, total_price, order_date)
                        VALUES (@CustomerId, @ProductId, @PaymentId, @Quantity, @TotalPrice, NOW())";

                    using (MySqlCommand cmd = new MySqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Database Error: " + ex.Message);
                return false;
            }
        }

        private int GetOrInsertCustomer(MySqlConnection conn, string name, string contact)
        {
            try
            {
                string selectQuery = "SELECT customer_id FROM customers WHERE customer_name = @Name AND contact_number = @Contact";
                using (var cmd = new MySqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    var result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToInt32(result);
                }

                Console.WriteLine($"🚀 Inserting new customer: {name}, {contact}");

                string insertQuery = "INSERT INTO customers (customer_name, contact_number) VALUES (@Name, @Contact)";
                using (var cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.ExecuteNonQuery();
                    return (int)cmd.LastInsertedId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Insert Customer Failed: " + ex.Message);
                throw;
            }
        }

        private int GetId(MySqlConnection conn, string table, string column, string value)
        {
            string idColumn = table switch
            {
                "products" => "product_id",
                "payments" => "payment_id",
                _ => throw new ArgumentException("Unknown table")
            };

            string query = $"SELECT {idColumn} FROM {table} WHERE {column} = @Value";
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Value", value);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception($"❌ Value '{value}' not found in table '{table}'");
                return Convert.ToInt32(result);
            }
        }
    }
}
