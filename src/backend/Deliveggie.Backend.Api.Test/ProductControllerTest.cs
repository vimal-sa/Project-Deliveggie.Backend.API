using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Deliveggie.Backend.Controllers;
using Deliveggie.Backend.Services;
using Deliveggie.Shared.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Deliveggie.Backend.Api.Test
{
    public class ProductControllerTest : IDisposable
    {
        private Mock<ILogger<ProductController>> _mockLogger;

        private Mock<IRabbitMqService> _mockRabbitMqService;

        private ProductController productController;

        private ProductRequest productRequest;

        private ProductsResponse productsResponse;

        private ProductDetailsRequest productDetailsRequest;

        private ProductDetailsResponse productDetailsResponse;

        public ProductControllerTest()
        {
            _mockLogger = new Mock<ILogger<ProductController>>(MockBehavior.Loose);
            _mockRabbitMqService = new Mock<IRabbitMqService>(MockBehavior.Loose);
        }

        [Fact]
        public void GetProductsPostiveTest()
        {
            SetRequestAndResponse();
            _mockRabbitMqService.Setup(p => p.GetProductList(It.IsAny<ProductRequest>())).Returns(productsResponse);
            productController = new ProductController(_mockLogger.Object, _mockRabbitMqService.Object);
            var result = productController.GetProducts();
            Assert.NotNull(result);
        }
        [Fact]
        public void GetProductsNegativeTest()
        {
            SetRequestAndResponse();
            _mockRabbitMqService.Setup(p => p.GetProductList(It.IsAny<ProductRequest>())).Returns(new ProductsResponse());
            productController = new ProductController(_mockLogger.Object, _mockRabbitMqService.Object);
            var result = productController.GetProducts();
            var okObjectResult = result as OkObjectResult;            
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public void GetProductDetailsPostiveTest()
        {
            SetRequestAndResponse();
            _mockRabbitMqService.Setup(p => p.GetDetails(It.IsAny<ProductDetailsRequest>())).Returns(productDetailsResponse);
            productController = new ProductController(_mockLogger.Object, _mockRabbitMqService.Object);
            var result = productController.GetProductById("SampleId");
            Assert.NotNull(result);
        }
        [Fact]
        public void GetProductDetailsNegativeTest()
        {
            SetRequestAndResponse();
            _mockRabbitMqService.Setup(p => p.GetDetails(It.IsAny<ProductDetailsRequest>())).Returns(new ProductDetailsResponse());
            productController = new ProductController(_mockLogger.Object, _mockRabbitMqService.Object);
            var result = productController.GetProductById("SampleId");
            var okObjectResult = result as OkObjectResult;
            var detailsResponse = okObjectResult.Value as ProductDetailsResponse;
            Assert.Null(detailsResponse?.Product);
        }


        private void SetRequestAndResponse()
        {
            productRequest = new ProductRequest { };
            productDetailsRequest = new ProductDetailsRequest { Id = "SampleId" };

            var product = new Product
            {
                Id = "SampleId",
                Name = "Brinjal",
                EntryDate = DateTime.Now,
                PriceWithDeduction = 10
            };

            productsResponse = new ProductsResponse { Products = new List<Product> { product } };
            productDetailsResponse = new ProductDetailsResponse { Product = product };
        }

        public void Dispose()
        {
            _mockLogger = null;
            _mockRabbitMqService = null;
            productRequest = null;
            productDetailsRequest = null;
            productsResponse = null;
            productDetailsResponse = null;
        }
    }
}
