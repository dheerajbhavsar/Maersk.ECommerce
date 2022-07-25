using Microsoft.ServiceFabric.Services.Remoting;

namespace Maersk.ECommerce.Api.Core;

public interface IProductsService : IService
{
    Task<(bool IsSuccess, Product product, string ErrorMessage)> GetProductsAsync(int id);
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
}