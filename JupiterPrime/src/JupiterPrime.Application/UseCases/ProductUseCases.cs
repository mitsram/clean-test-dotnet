using JupiterPrime.Application.Interfaces;
using JupiterPrime.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JupiterPrime.Application.UseCases
{
    public class ProductUseCases : IDisposable
    {
        private readonly IProductService _productService;
        private bool _disposed = false;

        public ProductUseCases(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productService.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _productService.GetProductByNameAsync(name);
        }

        public async Task AddProductToCartAsync(string productName, int quantity)
        {
            await _productService.AddProductToCartAsync(productName, quantity);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    (_productService as IDisposable)?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
