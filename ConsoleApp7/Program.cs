using System;
using Microsoft.Data.SqlClient;

namespace ConsoleApp7;

class Program
{
    static string connectionString =
        "Server=localhost;Database=SchoolDB;User Id=sa;Password=SqlPass2026;TrustServerCertificate=True;";

    static void Main()
    {
        /*
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
        */
        // homework starts here

        while (true)
        {
            Console.WriteLine("\n========== MENU ==========");
            Console.WriteLine("1 - Test Connection");
            Console.WriteLine("2 - Insert Student");
            Console.WriteLine("3 - Get All Students");
            Console.WriteLine("4 - Search Student");
            Console.WriteLine("5 - Update Student Age");
            Console.WriteLine("6 - Delete Student");
            Console.WriteLine("7 - Pagination");
            Console.WriteLine("0 - Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Task1_TestConnection(); break;
                case "2": Task2_InsertStudent(); break;
                case "3": Task3_GetAllStudents(); break;
                case "4": Task4_SearchStudent(); break;
                case "5": Task5_UpdateStudent(); break;
                case "6": Task6_DeleteStudent(); break;
                case "7": Task7_Pagination(); break;
                case "0": return;
                default: Console.WriteLine("Invalid choice!"); break;
            }
        }
    } // closes Main()

    static void Task1_TestConnection()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Connection successful");
        }
    }

    static void Task2_InsertStudent()
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Age: ");
        int age = int.Parse(Console.ReadLine());
        string query = "INSERT INTO Students (Name, Age) VALUES (@Name, @Age)";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Age", age);
            connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Student added successfully!");
        }
    }

    static void Task3_GetAllStudents()
    {
        string query = "SELECT Id, Name, Age FROM Students";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine($"{reader["Id"]} {reader["Name"]} {reader["Age"]}");
            reader.Close();
        }
    }

    static void Task4_SearchStudent()
    {
        Console.Write("Enter student name: ");
        string name = Console.ReadLine();
        string query = "SELECT Id, Name, Age FROM Students WHERE Name = @Name";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                Console.WriteLine($"{reader["Id"]} {reader["Name"]} {reader["Age"]}");
            else
                Console.WriteLine("Student not found.");
            reader.Close();
        }
    }

    static void Task5_UpdateStudent()
    {
        Console.Write("Enter student Id: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter new Age: ");
        int newAge = int.Parse(Console.ReadLine());
        string query = "UPDATE Students SET Age = @Age WHERE Id = @Id";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Age", newAge);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Student updated successfully!" : "Student not found.");
        }
    }

    static void Task6_DeleteStudent()
    {
        Console.Write("Enter student id: ");
        int id = int.Parse(Console.ReadLine());
        string query = "DELETE FROM Students WHERE Id = @Id";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Student deleted" : "Student not found.");
        }
    }

    static void Task7_Pagination()
    {
        Console.Write("Enter page size: ");
        int pageSize = int.Parse(Console.ReadLine());
        Console.Write("Enter page number: ");
        int pageNumber = int.Parse(Console.ReadLine());
        int offset = (pageNumber - 1) * pageSize;
        string query = @"
            SELECT Id, Name, Age FROM Students
            ORDER BY Id
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Offset", offset);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Console.WriteLine($"\nPage {pageNumber}");
            while (reader.Read())
                Console.WriteLine($"{reader["Id"]} {reader["Name"]} {reader["Age"]}");
            reader.Close();
        }
    }
} 