using Deliveggie.Processor.Model;
using Deliveggie.Shared.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deliveggie.Processor.Handle
{
    public class MongoDbHelper
    {
        private IConfiguration configuration;
        private IMongoDatabase iMongoDb;
        private string _productsCollection = "Products";
        private string _priceReductionsCollection = "PriceReductions";

        public MongoDbHelper()//(IConfiguration iconfig)
        {
            //configuration = iconfig;
            //var mongoDbHost = configuration.GetSection("MongoDbHost");
            //var mongoClient = mongoDbHost.GetSection("MongoClient").Value;
            //var mongodatabase = mongoDbHost.GetSection("Mongodatabase").Value;
            //InitializeDatabase(mongoClient, mongodatabase);
            Initialize("mongodb://localhost:27017", "DeliVeggie");
            SetDatabase();
        }

        private void Initialize(string connectionString, string database)
        {
            IMongoClient mongoClient = new MongoClient(connectionString);
            iMongoDb = mongoClient.GetDatabase(database);
        }

        private void SetDatabase()
        {
            //var collection = iMongoDb.GetCollection<Products>(_productsCollection);           
            //var priceDeductions = iMongoDb.GetCollection<PriceReductions>(_priceReductionsCollection);
        }

        public ProductsResponse GetProducts(ProductRequest request)
        {
            var products = new List<Product>();
            var collection = iMongoDb.GetCollection<Products>(_productsCollection);
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

        public ProductDetailsResponse GetProductDetails(string id)
        {
            var productDetailsResponse = new ProductDetailsResponse();
            var collection = iMongoDb.GetCollection<Products>(_productsCollection);
            ObjectId internalId = GetInternalId(id);
            PriceReductions PriceReduction = GetPriceDeduction();

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

        private PriceReductions GetPriceDeduction()
        {
            var day = DateTime.Today.DayOfWeek;
            var dayofWeek = Convert.ToInt32(day);
            var priceDeductions = iMongoDb.GetCollection<PriceReductions>(_priceReductionsCollection);
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
