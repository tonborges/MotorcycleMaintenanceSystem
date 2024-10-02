using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Fixtures;

public abstract class DbContextFixture<TDbContext>
    : IDisposable where TDbContext : DbContext
{
    public IApplicationDbContext BuildDbContext(string dbName)
    {
        try
        {
            var options = new DbContextOptionsBuilder<TDbContext>()
                            .UseInMemoryDatabase(dbName)
                            .EnableSensitiveDataLogging()
                            .Options;
            
            return BuildDbContext(options);
        }
        catch (Exception ex)
        {
            throw new Exception($"unable to connect to db.", ex);
        }
    }
    
    protected abstract IApplicationDbContext BuildDbContext(DbContextOptions<TDbContext> options);
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}