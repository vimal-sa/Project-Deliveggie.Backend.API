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
using System.Threading;
using System.Threading.Tasks;

namespace Deliveggie.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sleeping for rabbitmq");

            // waiting for rabbit to up and running
            Task.Delay(20000).Wait();

            Console.WriteLine("Deliveggie server Running!");

            //building configurations
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
           
            MongoDbHelper mongoDbHelper = new MongoDbHelper(configuration);

            Func<ProductRequest, ProductsResponse> ProductList = request =>
            {
                try
                {                    
                    Console.WriteLine($"Message recieved for ProductList {request.Id }");
                    return mongoDbHelper.GetProducts(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProductsResponse Error : " + ex.Message);
                    return new ProductsResponse { };
                }
            };

            Func<ProductDetailsRequest, ProductDetailsResponse> ProductDetails = request =>
            {
                try
                {
                    Console.WriteLine($"Message recieved for ProductDetails {request.Id }");
                    return mongoDbHelper.GetProductDetails(request.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProductDetailsResponse Error : " + ex.Message);
                    return new ProductDetailsResponse { };
                }
            };
            try
            {
                string connectionString = configuration["rabbittMQConnectionstring"];
                ////Console.WriteLine($"rabbittMQConnectionstring : {connectionString}");
                var bus = RabbitHutch.CreateBus(connectionString);
                bus.Rpc.Respond<ProductRequest, ProductsResponse>(ProductList);
                bus.Rpc.Respond<ProductDetailsRequest, ProductDetailsResponse>(ProductDetails);
                Console.WriteLine("Subscriber up");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.WriteLine("Wait here");
            Console.Read();
        }

    }
}
