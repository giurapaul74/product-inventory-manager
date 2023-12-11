namespace ProductInventoryManager.Backend
{
    public interface IBackendService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> AddProductAsync(Product product);
        Task<List<Product>> UpdateProductAsync(int id, Product updatedProduct);
        Task<List<Product>> DeleteProductAsync(int id);
        Task<string> SellStockAsync(int id, int quantityToSell);
        Task<string> AddStockAsync(int id, int quantityToAdd);
    }
}
