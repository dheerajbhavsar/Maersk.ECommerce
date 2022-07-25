using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maersk.ECommerce.Api.Core;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Maersk.ECommerce.Products;

/// <summary>
/// An instance of this class is created for each service instance by the Service Fabric runtime.
/// </summary>
internal sealed class Products : StatelessService, IProductsService
{
    public Products(StatelessServiceContext context)
        : base(context)
    { }

    public async Task<(bool IsSuccess, Product product, string ErrorMessage)> GetProductsAsync(int id)
    {
        return await Task.FromResult((true, new Product { Id = id, Name = $"Product_{id}" }, string.Empty));
    }

    /// <summary>
    /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
    /// </summary>
    /// <returns>A collection of listeners.</returns>
    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return this.CreateServiceRemotingInstanceListeners();
    }
}
