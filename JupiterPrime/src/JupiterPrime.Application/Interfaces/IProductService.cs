using JupiterPrime.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JupiterPrime.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByNameAsync(string name);
        Task AddProductToCartAsync(string productName, int quantity);
    }
}
