using Deliveggie.Shared.Models;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;

namespace Deliveggie.Backend.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        public readonly string _rmqConnectionString;

        public RabbitMqService(IConfiguration configuration)
        {
            _rmqConnectionString = configuration["rabbittMQConnectionstring"];
        }

        public ProductsResponse GetProductList(ProductRequest request)
        {
            var product = new ProductsResponse();
            try
            {
                using (var bus = RabbitHutch.CreateBus(_rmqConnectionString))
                {
                    product = bus.Rpc.Request<ProductRequest, ProductsResponse>(request);
                }                
            }
            catch (Exception ex)
            {
               // log exception here
            }
            return product;
        }

        public ProductDetailsResponse GetDetails(ProductDetailsRequest request)
        {
            var product = new ProductDetailsResponse();
            try
            {
                using (var bus = RabbitHutch.CreateBus(_rmqConnectionString))
                {
                    product = bus.Rpc.Request<ProductDetailsRequest, ProductDetailsResponse>(request);
                }
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return product;
        }
    }
}
