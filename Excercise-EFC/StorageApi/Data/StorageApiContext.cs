using Microsoft.EntityFrameworkCore;

public class StorageApiContext(DbContextOptions<StorageApiContext> options) : DbContext(options)
{
    public DbSet<Product> Product { get; set; } = default!;
}
