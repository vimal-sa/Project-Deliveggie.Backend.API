using Deliveggie.Processor.Model;
using Deliveggie.Shared.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Deliveggie.Processor.Handle
{
    public static class MongoDbInitializer
    {
        public static void AddDefaultData(IMongoDatabase iMongoDb, string products, string priceReductions)
        {
            try
            {
                string directoryPath = Directory.GetCurrentDirectory();

                //products collection
                var productsJson = JsonConvert.DeserializeObject<List<ProductJson>>(File.ReadAllText(directoryPath + @"/Data/Products.json"));
                var productCollection = iMongoDb.GetCollection<ProductJson>(products);
                productCollection.InsertMany(productsJson);

                //priceReductions collection
                var priceReductionsJson = JsonConvert.DeserializeObject<List<PriceReductionsJson>>(File.ReadAllText(directoryPath + @"/Data/PriceDeductions.json"));
                var priceReductionsCollection = iMongoDb.GetCollection<PriceReductionsJson>(priceReductions);
                priceReductionsCollection.InsertMany(priceReductionsJson);
            }
            catch
            {
                throw;
            }
        }
    }

    public class ProductJson
    {
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public double Price { get; set; }
    }

    public class PriceReductionsJson
    {
        public int DayOfWeek { get; set; }
        public double Reduction { get; set; }
    }
}
