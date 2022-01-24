using Api.Controllers;
using Api.Filter;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ApiTest
{
    public class ProductControllerTests
    {
        private ProductDbContext _productDbContext;
        private ProductController _productController;
        private IUriService _uriService;
        
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "MockDatabase")
                .Options;

            _productDbContext = new ProductDbContext(options);
            _uriService = new UriService("MockUri");
            
            _productController = new ProductController(_productDbContext, _uriService);

            for (int i = 1; i < 11; i++)
            {
                _productDbContext.Products.Add(new Product(i, $"ProductName{i}", @$"https://image_{i}", 12.2m * i));   
            }
            
            _productDbContext.SaveChanges();
        }

        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var filter = new PaginationFilter(1,10);
            var result = _productController.GetProducts(filter).Result;
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetProductById_ShouldReturnOkObjectResult()
        {
            var result = _productController.GetProductById(1);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetProductById_ShouldReturnNotFoundObjectResult()
        {
            var result = _productController.GetProductById(-1).Result;
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}