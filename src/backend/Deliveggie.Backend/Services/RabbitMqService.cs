using Deliveggie.Shared.Models;
using EasyNetQ;
using System;

namespace Deliveggie.Backend.Services
{
    public class RabbitMqService
    {
        public ProductsResponse GetProductList(ProductRequest request)
        {
            var product = new ProductsResponse();
            try
            {
                using (var bus = RabbitHutch.CreateBus("host=localhost"))
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
                using (var bus = RabbitHutch.CreateBus("host=localhost"))
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
