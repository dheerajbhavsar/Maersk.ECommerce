using Maersk.ECommerce.Api.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Maersk.ECommerce.Api.Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProductsAsync(int id)
    {
        var service = GetProductsService();
        var result = await service.GetProductsAsync(id);

        if (result.IsSuccess)
            return Ok(result.product);
        return NotFound();
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var serviceName = "fabric:/Maersk.ECommerce/Maersk.ECommerce.Api.Customers";
        var serviceUri = await ResolveAysnc(serviceName);

        try
        {
            var client = new HttpClient()
            {
                BaseAddress = serviceUri
            };

            var result = await client.GetAsync($"api/customers/{id}");

            if(result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var customer = JsonSerializer.Deserialize<dynamic>(content);
            
                return Ok(customer);
            }

            return NotFound();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static IProductsService GetProductsService()
    {
        return ServiceProxy.Create<IProductsService>(
            new Uri("fabric:/Maersk.ECommerce/Maersk.ECommerce.Products"));
    }

    private static async Task<Uri> ResolveAysnc(string name)
    {
        var uri = new Uri(name);
        var resolver = ServicePartitionResolver.GetDefault();
        var service = await resolver
            .ResolveAsync(uri, ServicePartitionKey.Singleton, CancellationToken.None);

        var address = JObject.Parse(service.GetEndpoint().Address);

        var primary = (string)address["Endpoints"].First();

        return new Uri(primary);
    }
}
