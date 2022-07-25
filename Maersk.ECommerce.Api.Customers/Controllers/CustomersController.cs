using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maersk.ECommerce.Api.Customers.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = new Customer
        {
            Id = id,
            Name = $"Customer_{id}"
        };

        return await Task.FromResult(Ok(customer));
    }

}

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
