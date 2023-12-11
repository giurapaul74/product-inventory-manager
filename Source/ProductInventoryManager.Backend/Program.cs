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

app.MapGet("/products", async (ProductInventoryManagerContext context) =>
{
    var products = await context.Products.ToListAsync();
    return Results.Json(products);
});

app.MapGet("/products/{id}", async (ProductInventoryManagerContext context, int id) =>
{
    return await context.Products.FindAsync(id) is Product product ?
        Results.Ok(product) :
        Results.NotFound("This product doesn't exist.");
});

app.MapPost("/products/addProduct", async (ProductInventoryManagerContext context, Product product) =>
{
    try
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        var updatedProducts = await context.Products.ToListAsync();
        return Results.Ok(updatedProducts);
    }
    catch (Exception ex)
    {
        // Log the exception for debugging purposes
        Console.WriteLine($"Error adding product: {ex.Message}");
        return Results.BadRequest("Failed to add the product.");
    }
});

app.MapPut("/products/updateProduct/{id}", async (ProductInventoryManagerContext context, Product updatedProduct, int id) =>
{
    var product = await context.Products.FindAsync(id);
    if (product == null)
        return Results.NotFound($"The product with ID {id} could not be found.");

    DateTime? originalCreationDate = product.CreationDate;

    product.ProductName = updatedProduct.ProductName;
    product.ProductDescription = updatedProduct.ProductDescription;
    product.ProductCategory = updatedProduct.ProductCategory;
    product.ProductStockKeepingUnit = updatedProduct.ProductStockKeepingUnit;
    product.ProductPrice = updatedProduct.ProductPrice;
    product.ProductQuantity = updatedProduct.ProductQuantity;
    product.SupplierInformation = updatedProduct.SupplierInformation;
    product.CreationDate = originalCreationDate;

    await context.SaveChangesAsync();

    return Results.Ok(await context.Products.ToListAsync());
});

app.MapDelete("/products/{id}", async (ProductInventoryManagerContext context, int id) =>
{
    var product = await context.Products.FindAsync(id);
    if (product == null)
        return Results.NotFound($"The product with ID {id} could not be found.");

    context.Products.Remove(product);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Products.ToListAsync());
});

app.MapPut("/products/sellStock/{id}", async (ProductInventoryManagerContext context, int id, int quantityToSell) => 
{
    var product = await context.Products.FindAsync(id);

    if (product == null)
    {
        return Results.NotFound($"The product with ID {id} could not be found.");
    }

    // Check if there is enough quantity in stock to sell
    if (product.ProductQuantity < quantityToSell)
    {
        return Results.BadRequest("Not enough quantity in stock to fulfill the sale.");
    }

    // Deduct the sold quantity from the product's stock
    product.ProductQuantity -= quantityToSell;

    // Save the updated product information to the database
    await context.SaveChangesAsync();

    return Results.Ok($"Sale successful. Current {product.ProductName} stock: {product.ProductQuantity} items.");
});

app.MapPut("/products/addStock/{id}", async (ProductInventoryManagerContext context, int id, int quantityToAdd) =>
{
    var product = await context.Products.FindAsync(id);

    if (product == null)
        return Results.NotFound($"The product with ID {id} could not be found.");

    if (quantityToAdd <= 0)
        return Results.BadRequest("Sorry, you must provide a positive quantity number.");

    product.ProductQuantity += quantityToAdd;

    await context.SaveChangesAsync();

    return Results.Ok($"Process successful. {quantityToAdd} item/s added to {product.ProductName} stock.");
});

app.Run();