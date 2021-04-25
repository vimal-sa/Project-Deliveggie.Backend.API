using Deliveggie.Shared.Models;

namespace Deliveggie.Backend.Services
{
    public interface IRabbitMqService
    {
        ProductsResponse GetProductList(ProductRequest request);
        ProductDetailsResponse GetDetails(ProductDetailsRequest request);
    }
}
