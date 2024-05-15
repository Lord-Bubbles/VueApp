using VueApp1.Server.Helpers;
using VueApp1.Server.Models;

namespace VueApp1.Server.Services;

public interface IPerformanceRepository
{
    (IEnumerable<Performance>, int) GetAll(PerformanceParameters query);
    Task<Performance> GetByIdAsync(int id);
    Task<Performance> CreateAsync(Performance entity);
    Task UpdateAsync(int id, Performance entity);
    Task DeleteAsync(int id);
}

public class PerformanceRepository : IPerformanceRepository
{
    protected readonly AppDbContext context;

    public PerformanceRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Performance> CreateAsync(Performance entity)
    {
        await context.Performances.AddAsync(entity);
        context.SaveChanges();
        return entity;
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public (IEnumerable<Performance>, int) GetAll(PerformanceParameters query)
    {
        if (query.Page <= 0 || query.Limit <= 0)
        {
            throw new Exception("Error: page and limit query parameters not valid!");
        }
        var filter = context.Performances.OrderByDescending(p => p.CreatedAt);
        if (query.Type != null && query.Type != string.Empty)
        {
            filter = (IOrderedQueryable<Performance>)filter.Where(p => p.Type == query.Type);
        }
        if (query.UserID > 0)
        {
            filter = (IOrderedQueryable<Performance>)filter.Where(p => p.UserID == query.UserID);
        }
        return (filter.Skip((query.Page - 1) * query.Limit).Take(query.Limit).ToList(), filter.Count());
    }

    public Task<Performance> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(int id, Performance entity)
    {
        throw new NotImplementedException();
    }
}