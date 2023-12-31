﻿
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Configure the HTTP request pipeline.

public class Product
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductCategory { get; set; }
    public string? ProductStockKeepingUnit { get; set;}
    public decimal? ProductPrice { get; set; }
    public int? ProductQuantity { get; set; }
    public string? SupplierInformation { get; set; }
    public DateTime? CreationDate { get; set; } 
}