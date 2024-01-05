using Moq;
using ProductInventoryManager.Backend;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace PIM.Tests
{
    [TestFixture]
    public class FrontendTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task GetIndex_ReturnsNotEmptyProductList()
        {
            //Arrange
            var products = new List<Product>() 
            {
                new Product 
                { 
                    Id = 1,
                    ProductName = "Test",
                    ProductDescription = "Test Description",
                    ProductCategory = "Test Category",
                    ProductStockKeepingUnit = "TTTT111",
                    ProductPrice = 0,

                }
            };
            var backendServiceMock = new Mock<IBackendService>();
            backendServiceMock.Setup(backendServiceMock => backendServiceMock.GetProductsAsync()).Returns()

            //Act

            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var productList = await response.Content.ReadFromJsonAsync<List<Product>>();
            productList.ShouldNotBeNull();
        }

        //write a test that asserts that the Index page returns an Ok response and the Product list


    }
}