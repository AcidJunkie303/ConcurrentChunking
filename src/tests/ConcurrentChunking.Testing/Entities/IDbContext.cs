using Microsoft.EntityFrameworkCore;

namespace ConcurrentChunking.Testing.Entities;

public interface IDbContext
{
    DbSet<SimpleEntity> SimpleEntities { get; set; }
}
