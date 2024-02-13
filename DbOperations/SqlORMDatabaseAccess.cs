using Dapper;
using Database;
using System.Data.SqlClient;


namespace DbOperations
{
    public class SqlORMDatabaseAccess : IDatabaseAccess
    {
        private string _connectionString;

        public SqlORMDatabaseAccess(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.Query<int>("SELECT COUNT(*) FROM Product WHERE Id = @Id", new { Id = order.Product.Id }).Single() == 0)
                {
                    throw new ArgumentException("The product with the provided ID does not exist", nameof(order.Product.Id));
                }

                var sqlQuery = "INSERT INTO Order (Status, CreateDate, UpdateDate, ProductId) VALUES (@Status, @CreateDate, @UpdateDate, @ProductId)";
                string? status = Enum.GetName(order.Status);
                connection.Execute(sqlQuery, new
                {
                    Status = status,
                    order.CreateDate,
                    order.UpdateDate,
                    ProductId = order.Product.Id
                });
            }
        }

        public void AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Product (Name, Description, Weight, Height, Width, Length) " +
                               "VALUES (@Name, @Description, @Weight, @Height, @Width, @Length)";
                connection.Execute(sqlQuery, new
                {
                    product.Name,
                    product.Description,
                    product.Weight,
                    product.Height,
                    product.Width,
                    product.Length
                });
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Order WHERE Id = @Id";
                connection.Execute(sqlQuery, new { Id = orderId });
            }
        }

        public void DeleteProduct(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.Query<int>("SELECT COUNT(*) FROM Order WHERE ProductId = @Id", new { Id = productId }).Single() > 0)
                {
                    throw new InvalidOperationException("The product is used in existing order(s).");
                }

                connection.Execute("DELETE FROM Product WHERE Id = @Id", new { Id = productId });
            }
        }

        public List<Product> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Product>("SELECT * FROM Product").ToList();
            }
        }
        public Order GetOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Order>("SELECT * FROM Order WHERE Id = @Id", new { Id = orderId }).FirstOrDefault();
            }
        }
        public Product GetProduct(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Product>("SELECT * FROM Product WHERE Id = @Id", new { Id = productId }).FirstOrDefault();
            }
        }
        public void UpdateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.Query<int>("SELECT COUNT(*) FROM Product WHERE Id = @Id", new { Id = order.Product.Id }).Single() == 0)
                {
                    throw new ArgumentException("The product with the provided ID does not exist", nameof(order.Product.Id));
                }

                var sqlQuery = "UPDATE Order SET Status = @Status, CreateDate = @CreateDate, UpdateDate = @UpdateDate, ProductId = @ProductId WHERE Id = @Id";
                var affectedRow = connection.Execute(sqlQuery, new
                {
                    Status = order.Status.ToString(),
                    order.CreateDate,
                    order.UpdateDate,
                    ProductId = order.Product.Id,
                    order.Id
                });
            }
        }
        public void UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Product SET Name = @Name, Description = @Description, Weight = @Weight, Height = @Height, Width = @Width, Length = @Length WHERE Id = @Id";
                connection.Execute(sqlQuery, new
                {
                    product.Name,
                    product.Description,
                    product.Weight,
                    product.Height,
                    product.Width,
                    product.Length,
                    product.Id
                });
            }
        }
    }
}
