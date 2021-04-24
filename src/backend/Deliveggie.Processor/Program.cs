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
        //public Program()
        //{
        //    var builder = new ConfigurationBuilder()
        //   .AddJsonFile("appsettings.json");

        //    var configuration = builder.Build();
        //}

        static void Main(string[] args)
        {
            Console.WriteLine("Deliveggie Processor Running!");
            Func<ProductRequest, ProductsResponse> ProductList = request =>
            {
                Console.WriteLine($"Message recieved ProductList {request.Id }");
                return GetProducts(request);
            };

            Func<ProductDetailsRequest, ProductDetailsResponse> ProductDetails = request =>
            {
                Console.WriteLine($"Message recieved ProductDetails {request.Id }");
                return GetProductDetails(request.Id);
            };

            using (var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest"))
            {
                bus.Rpc.Respond<ProductRequest, ProductsResponse>(ProductList);
                bus.Rpc.Respond<ProductDetailsRequest, ProductDetailsResponse>(ProductDetails);
                Console.ReadKey();
            }
        }

        static ProductsResponse GetProducts(ProductRequest request)
        {
            var products = new List<Product>();
            IMongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("DeliVeggie");
            var collection = database.GetCollection<Products>("Products");
            var result = collection?.Find<Products>(x => true)?.ToList();
            if (result != null && result.Count > 0)
            {
                result.ForEach(x =>
                {
                    products.Add(new Product { Id = x.id.ToString(), Name = x.Name });
                });
            }
            return new ProductsResponse { Products = products };
        }
        static ProductDetailsResponse GetProductDetails(string id)
        {
            var productDetailsResponse = new ProductDetailsResponse();
            IMongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("DeliVeggie");
            var collection = database.GetCollection<Products>("Products");
            ObjectId internalId = GetInternalId(id);
            PriceReductions PriceReduction = GetPriceDeduction(database);

            var item = collection?.Find(Builders<Products>.Filter.Eq("_id", internalId))?.FirstOrDefault();
            if (item != null)
            {
                productDetailsResponse.Product = new Product
                {
                    Id = item.id.ToString(),
                    Name = item.Name,
                    EntryDate = item.EntryDate,
                    PriceWithDeduction = item.Price - Convert.ToDouble(PriceReduction?.Reduction)
                };
            }
            return productDetailsResponse;
        }

        private static PriceReductions GetPriceDeduction(IMongoDatabase database)
        {
            var day = DateTime.Today.DayOfWeek;
            var dayofWeek = Convert.ToInt32(day);

            var priceDeductions = database.GetCollection<PriceReductions>("PriceReductions");
            var PriceReduction = priceDeductions?.Find(Builders<PriceReductions>.Filter.Eq("DayOfWeek", dayofWeek))?.FirstOrDefault();
            return PriceReduction;
        }

        public static ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

    }
}
