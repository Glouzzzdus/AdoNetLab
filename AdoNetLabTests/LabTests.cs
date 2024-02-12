using Database;
using DbOperations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

[TestClass]
public class DatabaseAccessTests
{
    private Mock<IDatabaseAccess> _mockDatabaseAccess;
    private Product _testProduct;
    private Order _testOrder;

    [TestInitialize]
    public void SetUp()
    {
        _mockDatabaseAccess = new Mock<IDatabaseAccess>();

        _testProduct = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Weight = 1.0,
            Height = 2.0,
            Width = 3.0,
            Length = 4.0
        };

        _testOrder = new Order
        {
            Id = 1,
            Status = Status.NotStarted,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Product = _testProduct
        };
    }

    [TestMethod]
    public void AddProduct_ShouldCallAddProductOnce()
    {
        // Arrange
        _mockDatabaseAccess.Setup(x => x.AddProduct(It.IsAny<Product>()));

        // Act
        _mockDatabaseAccess.Object.AddProduct(_testProduct);

        // Assert
        _mockDatabaseAccess.Verify(x => x.AddProduct(It.Is<Product>(p => p == _testProduct)), Times.Once);
    }

    [TestMethod]
    public void AddOrder_ShouldCallAddOrderOnce()
    {
        // Arrange
        _mockDatabaseAccess.Setup(x => x.AddOrder(It.IsAny<Order>()));

        // Act
        _mockDatabaseAccess.Object.AddOrder(_testOrder);

        // Assert
        _mockDatabaseAccess.Verify(x => x.AddOrder(It.Is<Order>(o => o == _testOrder)), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException), "Product with provided ID does not exist")]
    public void AddOrder_WithNonExistingProduct_ShouldThrowArgumentException()
    {
        // Arrange
        var wrongOrder = new Order
        {
            Id = 2,
            Status = Status.NotStarted,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Product = new Product { Id = 99 }
        };

        _mockDatabaseAccess.Setup(x => x.AddOrder(It.IsAny<Order>()))
            .Throws(new ArgumentException("Product with provided ID does not exist"));

        // Act
        _mockDatabaseAccess.Object.AddOrder(wrongOrder);

        // Assert is handled by ExpectedException
    }
}
