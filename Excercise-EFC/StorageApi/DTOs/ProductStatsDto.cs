namespace StorageApi.DTOs;

public class ProductStatsDto
{
    public int TotalProductCount { get; set; }

    public int TotalInventoryValue { get; set; }

    public decimal AveragePrice { get; set; }
}
