using System;
using Microsoft.Data.SqlClient;

namespace ConsoleApp7;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;

    public static void GetProductById(SqlConnection conn)
    {
        Console.Write("Enter product ID: ");
        string? input = Console.ReadLine();
        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        using SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
            Console.WriteLine($"Found: {reader["Name"]} | ${reader["Price"]} | {reader["Category"]}");
        else
            Console.WriteLine("Product not found!");
    }
    public static void CreateProduct(SqlConnection conn)
    {
        Console.Write("Product name: ");
        string? nameInput = Console.ReadLine();
        string name = string.IsNullOrWhiteSpace(nameInput) ? "Unnamed Product" : nameInput;
    
        Console.Write("Price: ");
        string? priceInput = Console.ReadLine();
        if (!decimal.TryParse(priceInput, out decimal price))
        {
            Console.WriteLine("Invalid price.");
            return;
        }
    
        Console.Write("Category: ");
        string? categoryInput = Console.ReadLine();
        string category = string.IsNullOrWhiteSpace(categoryInput) ? "Uncategorized" : categoryInput;

        using SqlCommand cmd = new SqlCommand(
            "INSERT INTO Products (Name, Price, Category) VALUES (@name, @price, @category)", conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@price", price);
        cmd.Parameters.AddWithValue("@category", category);
        cmd.ExecuteNonQuery();

        Console.WriteLine("✅ Product created successfully!");
    }

    public static void UpdateProduct(Product product)
    {
        string connectionString =
            "Server=localhost;Database=database_name;User Id=sa;Password=SqlPass2026;TrustServerCertificate=True;";

        using SqlConnection connection = new SqlConnection(connectionString);
        string query = "UPDATE PRODUCTS SET Name = @name, Price = @price, Category = @category WHERE Id = @id";
        using SqlCommand sqlCommand = new SqlCommand(query, connection);
        sqlCommand.Parameters.AddWithValue("@name", product.Name);
        sqlCommand.Parameters.AddWithValue("@price", product.Price);
        sqlCommand.Parameters.AddWithValue("@category", product.Category);
        sqlCommand.Parameters.AddWithValue("@id", product.Id);
        connection.Open();
        sqlCommand.ExecuteNonQuery();
    }
}