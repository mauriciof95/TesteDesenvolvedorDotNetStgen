using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Extensions;
using GoodHamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GoodHamburger.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApiDbContext _context;
    protected readonly DbSet<TEntity> _db;

    public BaseRepository(ApiDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _db = context.Set<TEntity>();
    }

    private IQueryable<TEntity> Queryable(bool includeDeleted = false)
        => !includeDeleted
            ? _db.AsQueryable()
            : _db.IgnoreQueryFilters().AsQueryable();

    public virtual async Task<TEntity?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _db.FindAsync(new object[] { id }, ct);

    public virtual async Task<TEntity[]> GetByIdsAsync(long[] ids, CancellationToken ct = default)
    {
        if (ids == null || ids.Length == 0) return Array.Empty<TEntity>();
        return await _db.Where(x => ids.Contains(x.Id)).ToArrayAsync(ct);
    }

    public virtual async Task<TEntity[]> GetAllAsync(bool includeDeleted = false, CancellationToken ct = default)
        => await Queryable(includeDeleted).AsNoTracking().ToArrayAsync(ct);

    public virtual async Task<PagedQueryResult<TEntity>> GetPagedResultAsync(BaseSearchParameters<TEntity> parameters, CancellationToken ct = default)
        => await _db.AsNoTracking().PaginateAsync(parameters, ct);

    public virtual TEntity Create(TEntity entity)
    {
        _db.Add(entity);
        return entity;
    }

    public virtual void Update(TEntity entity)
        => _context.Update(entity);

    public virtual void Delete(TEntity entity)
        => _db.Remove(entity);
}