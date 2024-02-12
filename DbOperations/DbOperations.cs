using Database;
using System.Data.SqlClient;

namespace DbOperations
{
    public class DbOperations
    {
        private IDatabaseAccess _databaseAccess;

        public DbOperations(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        } 

        public List<Product> GetAllProducts()
        {
            return _databaseAccess.GetAllProducts();
        }

        public Product GetProduct(int productId)
        {
           return _databaseAccess.GetProduct(productId);
        }

        public Order GetOrder(int orderId)
        {
            return _databaseAccess.GetOrder(orderId);
        }

        public void AddProduct(Product product)
        {
            _databaseAccess.AddProduct(product);
        }

        public void AddOrder(Order order)
        {
            _databaseAccess.AddOrder(order);
        }

        public void UpdateProduct(Product product)
        {
            _databaseAccess.UpdateProduct(product);
        }

        public void UpdateOrder(Order order)
        {
            _databaseAccess.UpdateOrder(order);
        }

        public void DeleteOrder(int orderId)
        {
            _databaseAccess.DeleteOrder(orderId);
        }

        public void DeleteProduct(int productId)
        {
            _databaseAccess.DeleteProduct(productId);
        }
    }
}
