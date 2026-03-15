using System;
using Microsoft.Data.SqlClient;

namespace ConsoleApp7;

class Program
{
    static void Main()
    {
        string connectionString =
            "Server=localhost;Database=database_name;User Id=sa;Password=SqlPass2026;TrustServerCertificate=True;";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            Console.WriteLine("✅ Connected to database!\n");

            string createTable = @"
                IF OBJECT_ID('Products', 'U') IS NULL
                BEGIN
                    CREATE TABLE Products (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100) NOT NULL,
                        Price DECIMAL(18,2) NOT NULL,
                        Category NVARCHAR(50) NOT NULL
                    )
                END";
            new SqlCommand(createTable, conn).ExecuteNonQuery();
            Console.WriteLine("✅ Products table ready!\n");
            
            string insert = @"
                IF NOT EXISTS (SELECT 1 FROM Products)
                BEGIN
                    INSERT INTO Products (Name, Price, Category) VALUES
                    ('Laptop', 999.99, 'Electronics'),
                    ('Gaming Mouse', 49.99, 'Accessories'),
                    ('Smartphone', 799.00, 'Electronics'),
                    ('Book - Clean Code', 29.50, 'Books'),
                    ('Book - Atomic Habits', 18.99, 'Books')
                END";
            new SqlCommand(insert, conn).ExecuteNonQuery();
            Console.WriteLine("✅ Products inserted!\n");

            string select = "SELECT * FROM Products";
            SqlDataReader reader = new SqlCommand(select, conn).ExecuteReader();

            Console.WriteLine("🛒 Products in database:");
            Console.WriteLine("-----------------------------------");
            while (reader.Read())
            {
                Console.WriteLine(
                    $"ID: {reader["Id"]} | {reader["Name"]} | ${reader["Price"]} | {reader["Category"]}");
            }
            reader.Close();

            Product.GetProductById(conn);
            Product.CreateProduct(conn);
        } 
        
    }
    
    
}