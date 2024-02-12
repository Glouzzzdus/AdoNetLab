using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOperations
{
    public interface IDatabaseAccess
    {
        List<Product> GetAllProducts();
        Product GetProduct(int productId);
        Order GetOrder(int orderId);
        void AddProduct(Product product);
        void AddOrder(Order order);
        void UpdateProduct(Product product);
        void UpdateOrder(Order order);
        void DeleteProduct(int productId);
        void DeleteOrder(int orderId);
    }
}
