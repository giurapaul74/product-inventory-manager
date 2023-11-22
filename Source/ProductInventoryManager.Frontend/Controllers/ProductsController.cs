using Microsoft.AspNetCore.Mvc;
using ProductInventoryManager.Frontend.Models;
using System.Text;

namespace ProductInventoryManager.Frontend.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //Generate a controller method that returns all products from the database by making a call to the Minimal API.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri("/products", UriKind.Relative);

                // Make a GET request to your Minimal API's endpoint
                var response = await client.GetAsync(relativeUri);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of products
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();

                    var productViewModel = new ProductListViewModel
                    {
                        Products = products
                    };

                    return View(productViewModel);
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to retrieve products.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                return StatusCode(500, $"An error occurred while fetching products. {ex.Message}");
            }
        }

        //Generate a controller method that returns a single product from the database by making a call to the Minimal API.
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri($"/products/{id}", UriKind.Relative);

                // Make a GET request to your Minimal API's endpoint
                var response = await client.GetAsync(relativeUri);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a product
                    var product = await response.Content.ReadFromJsonAsync<Product>();

                    var productViewModel = new ProductDetailsViewModel
                    {
                        Product = product
                    };

                    return View(productViewModel);
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to retrieve product.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                return StatusCode(500, $"An error occurred while fetching product. {ex.Message}");
            }
        }

        //Generate a controller method that returns the view for adding a product.
        [HttpGet("addProduct")]
        public IActionResult AddProduct()
        {
            return View("AddProduct");
        }

        //Generate a controller method that adds a product to the database by making a call to the Minimal API.
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri("/products/addProduct", UriKind.Relative);

                // Make a POST request to your Minimal API's endpoint
                var response = await client.PostAsJsonAsync(relativeUri, product);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of products
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();

                    var productListViewModel = new ProductListViewModel
                    {
                        Products = products
                    };

                    return View("Index", productListViewModel);
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to add product.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                return StatusCode(500, $"An error occurred while adding product. {ex.Message}");
            }
        }


        [HttpGet("updateProductPage/{id}")]
        public async Task<IActionResult> UpdateProductPage(int id)
        {
            // Create an HTTP client using the factory
            var client = _httpClientFactory.CreateClient("MinimalApi");

            var relativeUri = new Uri($"/products/{id}", UriKind.Relative);

            var response = await client.GetAsync(relativeUri);

            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<Product>();

                var productUpdateViewModel = new ProductUpdateViewModel
                {
                    // Map properties from Product to ProductUpdateViewModel
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductCategory = product.ProductCategory,
                    ProductStockKeepingUnit = product.ProductStockKeepingUnit,
                    ProductPrice = product.ProductPrice,
                    ProductQuantity = product.ProductQuantity,
                    SupplierInformation = product.SupplierInformation,
                };

                return View("UpdateProduct", productUpdateViewModel);
            }

            // Handle the case where the Minimal API request is not successful
            return StatusCode((int)response.StatusCode, "Failed to retrieve product.");
        }


        [HttpPost("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateViewModel updatedProductUpdateViewModel)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri($"/products/updateProduct/{updatedProductUpdateViewModel.ProductId}", UriKind.Relative);

                // Make a GET request to your Minimal API's endpoint
                var response = await client.PutAsJsonAsync(relativeUri, updatedProductUpdateViewModel);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to retrieve product.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                return StatusCode(500, $"An error occurred while fetching product. {ex.Message}");
            }
        }

        //Generate a controller method that deletes a product from the database by making a call to the Minimal API.
        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri($"/products/{id}", UriKind.Relative);

                // Make a DELETE request to your Minimal API's endpoint
                var response = await client.DeleteAsync(relativeUri);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to a list of products
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();

                    var productListViewModel = new ProductListViewModel
                    {
                        Products = products
                    };

                    return View("Index", productListViewModel);
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to delete product.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                return StatusCode(500, $"An error occurred while deleting product. {ex.Message}");
            }
        }

        //Generate a controller method that will sell stock from a specific product by making a call to the Minimal API.
        [HttpPut("sellStock")]
        public async Task<IActionResult> SellStock([FromBody] ProductSellStockViewModel productSellStockViewModel)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri($"/products/sellStock/{productSellStockViewModel.ProductId}?quantityToSell={productSellStockViewModel.QuantityToSell}", UriKind.Relative);

                // Make a POST request to your Minimal API's endpoint
                var response = await client.PutAsJsonAsync(relativeUri, productSellStockViewModel.QuantityToSell);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to sell stock.");
            }
            catch (Exception ex)
            {
                // If an error occurs, return a JSON response with the error message
                return Json(new { success = false, message = $"Failed to sell stock. {ex.Message}" });
            }
        }

        //Generate a controller method that will add stock to a specific product by making a call to the Minimal API.
        [HttpPut("addStock")]
        public async Task<IActionResult> AddStock([FromBody] ProductAddStockViewModel productAddStockViewModel)
        {
            try
            {
                // Create an HTTP client using the factory
                var client = _httpClientFactory.CreateClient("MinimalApi");

                var relativeUri = new Uri($"/products/addStock/{productAddStockViewModel.ProductId}?quantityToAdd={productAddStockViewModel.QuantityToAdd}", UriKind.Relative);

                // Make a POST request to your Minimal API's endpoint
                var response = await client.PutAsJsonAsync(relativeUri, productAddStockViewModel.QuantityToAdd);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }

                // Handle the case where the Minimal API request is not successful
                return StatusCode((int)response.StatusCode, "Failed to add stock.");
            }
            catch (Exception ex)
            {
                // If an error occurs, return a JSON response with the error message
                return Json(new { success = false, message = $"Failed to add stock. {ex.Message}" });
            }
        }
    }
}
