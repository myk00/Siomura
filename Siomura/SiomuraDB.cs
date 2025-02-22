using System;
using MySql.Data.MySqlClient;

namespace Siomura 
{
    public class SiomuraDB
    {
        // ✅ Fixed connection string (Removed extra escape characters)
        private string connectionString = "server=10.0.2.2;port=3306;database=SiomuraDB;user=root;password=toytoynitutoy21;";

        public bool SaveOrder(string customerName, string contactNumber, string product, int quantity, decimal totalPrice, string paymentMethod)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO orders (CustomerName, ContactNumber, Product, Quantity, TotalPrice, PaymentMethod) " +
                                   "VALUES (@CustomerName, @ContactNumber, @Product, @Quantity, @TotalPrice, @PaymentMethod)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);
                        cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        // ✅ Ensure order was inserted
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
    }
}
