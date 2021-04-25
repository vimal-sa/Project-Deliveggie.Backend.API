using Deliveggie.Processor.Handle;
using Deliveggie.Processor.Model;
using Deliveggie.Shared.Models;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;

namespace Deliveggie.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Deliveggie Processor Running!");
            
            MongoDbHelper mongoDbHelper = new MongoDbHelper();
            
            Func<ProductRequest, ProductsResponse> ProductList = request =>
            {
                Console.WriteLine($"Message recieved ProductList {request.Id }");
                return mongoDbHelper.GetProducts(request);
            };

            Func<ProductDetailsRequest, ProductDetailsResponse> ProductDetails = request =>
            {
                Console.WriteLine($"Message recieved ProductDetails {request.Id }");
                return mongoDbHelper.GetProductDetails(request.Id);
            };

            using (var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest"))
            {
                bus.Rpc.Respond<ProductRequest, ProductsResponse>(ProductList);
                bus.Rpc.Respond<ProductDetailsRequest, ProductDetailsResponse>(ProductDetails);
                Console.ReadKey();
            }
        }

    }
}
