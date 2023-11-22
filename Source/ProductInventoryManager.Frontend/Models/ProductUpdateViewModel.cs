namespace ProductInventoryManager.Frontend.Models
{
    public class ProductUpdateViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductStockKeepingUnit { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public string? SupplierInformation { get; set; }
    }
}
