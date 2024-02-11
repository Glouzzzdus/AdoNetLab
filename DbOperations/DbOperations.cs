using Database;
using System.Data.SqlClient;

namespace DbOperations
{
    public class DbOperations
    {
        private string _connectionString = "Server=(local);Database=AdoNetLab;Integrated Security=true;";

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Product", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Weight = (double)reader["Weight"],
                            Height = (double)reader["Height"],
                            Width = (double)reader["Width"],
                            Length = (double)reader["Length"]
                        };

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Product (Name, Description, Weight, Height, Width, Length) VALUES (@Name, @Description, @Weight, @Height, @Width, @Length)";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Weight", product.Weight);
                    command.Parameters.AddWithValue("@Height", product.Height);
                    command.Parameters.AddWithValue("@Width", product.Width);
                    command.Parameters.AddWithValue("@Length", product.Length);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string checkProductQuery = "SELECT COUNT(*) FROM Product WHERE Id = @ProductId";
                using (SqlCommand command = new SqlCommand(checkProductQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", order.Product.Id);
                    int existingCount = (int)command.ExecuteScalar();

                    if (existingCount == 0)
                    {
                        throw new ArgumentException("Product with provided ID does not exist", nameof(order.Product.Id));
                    }

                    string status = Enum.GetName(order.Status);
                    string sqlQuery = "INSERT INTO Order (Status, CreateDate, UpdateDate, ProductId) VALUES (@Status, @CreateDate, @UpdateDate, @ProductId)";

                    using (SqlCommand insertCommand = new SqlCommand(sqlQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Status", status);
                        insertCommand.Parameters.AddWithValue("@CreateDate", order.CreateDate);
                        insertCommand.Parameters.AddWithValue("@UpdateDate", order.UpdateDate);
                        insertCommand.Parameters.AddWithValue("@ProductId", order.Product.Id);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE Product SET Name = @Name, Description = @Description, Weight = @Weight, Height = @Height, Width = @Width, Length = @Length WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Weight", product.Weight);
                    command.Parameters.AddWithValue("@Height", product.Height);
                    command.Parameters.AddWithValue("@Width", product.Width);
                    command.Parameters.AddWithValue("@Length", product.Length);
                    command.Parameters.AddWithValue("@Id", product.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No row found to update.");
                    }
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string checkProductQuery = "SELECT COUNT(*) FROM Product WHERE Id = @ProductId";
                using (SqlCommand checkCommand = new SqlCommand(checkProductQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ProductId", order.Product.Id);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount == 0)
                    {
                        throw new ArgumentException("Product with provided ID does not exist", nameof(order.Product.Id));
                    }

                    string sqlQuery = "UPDATE Order SET Status = @Status, CreateDate = @CreateDate, UpdateDate = @UpdateDate, ProductId = @ProductId WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Status", order.Status.ToString());
                        command.Parameters.AddWithValue("@CreateDate", order.CreateDate);
                        command.Parameters.AddWithValue("@UpdateDate", order.UpdateDate);
                        command.Parameters.AddWithValue("@ProductId", order.Product.Id);
                        command.Parameters.AddWithValue("@Id", order.Id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("No row found to update.");
                        }
                    }
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "DELETE FROM Order WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", orderId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No order found to delete.");
                    }
                }
            }
        }

        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string checkOrderQuery = "SELECT COUNT(*) FROM Order WHERE ProductId = @ProductId";
                using (SqlCommand checkCommand = new SqlCommand(checkOrderQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@ProductId", productId);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        throw new InvalidOperationException("Product is used in existing order(s).");
                    }

                    string sqlQuery = "DELETE FROM Product WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", productId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("No product found to delete.");
                        }
                    }
                }
            }
        }
    }
}
