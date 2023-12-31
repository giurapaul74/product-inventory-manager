using ProductInventoryManager.Backend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductInventoryManagerContext>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7042")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IBackendService, BackendService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", async (IBackendService context) =>
{
    var products = await context.GetProductsAsync();
    return Results.Json(products);
});

app.MapGet("/products/{id}", async (IBackendService context, int id) =>
{
    return await context.GetProductByIdAsync(id) is Product product ?
        Results.Ok(product) :
        Results.NotFound("This product doesn't exist.");
});

app.MapPost("/products/addProduct", async (IBackendService context, Product product) =>
{
    await context.AddProductAsync(product);

    return Results.Ok(await context.GetProductsAsync());
});

app.MapPut("/products/updateProduct/{id}", async (IBackendService context, Product updatedProduct, int id) =>
{
    await context.UpdateProductAsync(id, updatedProduct);

    return Results.Ok(await context.GetProductsAsync());
});

app.MapDelete("/products/{id}", async (IBackendService context, int id) =>
{
    await context.DeleteProductAsync(id);

    return Results.Ok(await context.GetProductsAsync());
});

app.MapPut("/products/sellStock/{id}", async (IBackendService context, int id, int quantityToSell) => 
{
    await context.SellStockAsync(id, quantityToSell);

    //return Results.Ok($"Sale successful. Current {product.ProductName} stock: {product.ProductQuantity} items.");
    return Results.Ok(await context.GetProductsAsync());
});

app.MapPut("/products/addStock/{id}", async (IBackendService context, int id, int quantityToAdd) =>
{
    await context.AddStockAsync(id, quantityToAdd);

    //return Results.Ok($"Process successful. {quantityToAdd} item/s added to {product.ProductName} stock.");
    return Results.Ok(await context.GetProductsAsync());
});

app.Run();