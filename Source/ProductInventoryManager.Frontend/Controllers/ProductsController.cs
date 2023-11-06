using Microsoft.AspNetCore.Mvc;

namespace ProductInventoryManager.Frontend.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7042/");

                var response = await httpClient.GetAsync("/products");

                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                    return View(products);
                }
                else
                {
                    return View("Error");
                }
            }
        }
    }
}
