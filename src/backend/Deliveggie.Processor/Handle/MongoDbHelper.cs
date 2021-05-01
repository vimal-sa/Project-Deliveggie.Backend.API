using Deliveggie.Processor.Model;
using Deliveggie.Shared.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deliveggie.Processor.Handle
{
    public class MongoDbHelper
    {
        private readonly IConfiguration Configuration;
        private readonly string _mongoClient;
        private readonly string _mongoDatabase;
        private string _productsCollection = "Products";
        private string _priceReductionsCollection = "PriceReductions";
        private IMongoDatabase iMongoDb;

        public MongoDbHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _mongoClient = Configuration["MongoDbHost:MongoClient"];
            //Console.WriteLine($"_mongoClient : {_mongoClient}");
            _mongoDatabase = Configuration["MongoDbHost:Mongodatabase"];
            //Console.WriteLine($"_mongoDatabase : {_mongoDatabase}");
            Initialize();
        }

        #region Public methods

        public ProductsResponse GetProducts(ProductRequest request)
        {
            //Initialize();
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
            //Initialize();
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

        #endregion

        #region private methods

        private void Initialize()
        {
            try
            {
                IMongoClient mongoClient = new MongoClient(_mongoClient);
                iMongoDb = mongoClient.GetDatabase(_mongoDatabase);
                var collection = iMongoDb.GetCollection<Products>(_productsCollection);
                var result = collection?.Find<Products>(x => true)?.ToList();
                if (result == null || !result.Any())
                {
                    Console.WriteLine("No data found in database. Adding default data in progress....");
                    MongoDbInitializer.AddDefaultData(iMongoDb, _productsCollection, _priceReductionsCollection);
                    Console.WriteLine("Data setup completed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed with error: {ex.Message}");                
            }
        }

        private PriceReductions GetPriceDeduction()
        {
            var day = DateTime.Today.DayOfWeek;
            var dayofWeek = Convert.ToInt32(day);
            var priceDeductions = iMongoDb.GetCollection<PriceReductions>(_priceReductionsCollection);
            var PriceReduction = priceDeductions?.Find(Builders<PriceReductions>.Filter.Eq("DayOfWeek", dayofWeek))?.FirstOrDefault();
            return PriceReduction;
        }

        private static ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        #endregion
    }
}
