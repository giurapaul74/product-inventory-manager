
using ProductInventoryManager.Backend.Exceptions;

namespace ProductInventoryManager.Backend
{
    public class BackendService : IBackendService
    {
        private readonly ProductInventoryManagerContext _context;

        public BackendService(ProductInventoryManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> AddProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error adding product: {ex.Message}");
                throw new Exception("Failed to add the product.", ex);
            }
        }

        public async Task<List<Product>> UpdateProductAsync(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"The product with ID {id} could not be found.");

            DateTime? originalCreationDate = product.CreationDate;

            product.ProductName = updatedProduct.ProductName;
            product.ProductDescription = updatedProduct.ProductDescription;
            product.ProductCategory = updatedProduct.ProductCategory;
            product.ProductStockKeepingUnit = updatedProduct.ProductStockKeepingUnit;
            product.ProductPrice = updatedProduct.ProductPrice;
            product.ProductQuantity = updatedProduct.ProductQuantity;
            product.SupplierInformation = updatedProduct.SupplierInformation;
            product.CreationDate = originalCreationDate;

            await _context.SaveChangesAsync();

            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"The product with ID {id} could not be found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return await _context.Products.ToListAsync();
        }

        public async Task<string> SellStockAsync(int id, int quantityToSell)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new NotFoundException($"The product with ID {id} could not be found.");

            // Check if there is enough quantity in stock to sell
            if (product.ProductQuantity < quantityToSell)
            {
                throw new BadRequestException("Not enough quantity in stock to fulfill the sale.");
            }

            // Deduct the sold quantity from the product's stock
            product.ProductQuantity -= quantityToSell;

            // Save the updated product information to the database
            await _context.SaveChangesAsync();

            return $"Sale successful. Current {product.ProductName} stock: {product.ProductQuantity} items.";
        }

        public async Task<string> AddStockAsync(int id, int quantityToAdd)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new NotFoundException($"The product with ID {id} could not be found.");

            if (quantityToAdd <= 0)
                throw new BadRequestException("Sorry, you must provide a positive quantity number.");

            product.ProductQuantity += quantityToAdd;

            await _context.SaveChangesAsync();

            return $"Process successful. {quantityToAdd} item/s added to {product.ProductName} stock.";
        }
    }
}
