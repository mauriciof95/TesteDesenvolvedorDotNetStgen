using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApiDbContext _context;
    protected readonly DbSet<TEntity> _db;

    public BaseRepository(ApiDbContext context)
    {
        _context = context;
        if (context != null)
            _db = context.Set<TEntity>();
    }

    public void Commit()
        => _context.SaveChanges();

    private IQueryable<TEntity> Queryable()
        => _db.AsQueryable();

    public IQueryable<TEntity> AsNoTracking(IQueryable<TEntity> query)
        => query.AsNoTracking();

    public virtual TEntity GetById(long id)
        => _db.Find(id);

    public virtual TEntity[] GetByIds(long[] ids)
        => _db.Where(x => ids.Contains(x.Id)).ToArray();

    public virtual TEntity[] GetAll(bool includeDeleted = false)
    {
        return (!includeDeleted
                    ? _db.Where(x => !x.DeletedAt.HasValue)
                    : _db
               ).AsNoTracking().ToArray();
    }

    public virtual PagedQueryResult<TEntity> GetPagedResult(BaseSearchParameters parameters)
    {
        var pagedResult = new PagedQueryResult<TEntity>();

        var query = Queryable().Where(x => !x.DeletedAt.HasValue);

        query = ApplyPagedFilter(query, parameters);

        pagedResult.TotalCount = query.Count();

        query = parameters.OrderType == "asc"
            ? query.OrderBy(x => x.Id)
            : query.OrderByDescending(x => x.Id);
        
        query = query.Skip(parameters.PerPage * parameters.CurrentPage).Take(parameters.PerPage);

        query = ApplyIncludes(query);

        pagedResult.Rows = query.ToList();

        return pagedResult;
    }

    public virtual IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query)
        => query;

    public virtual IQueryable<TEntity> ApplyPagedFilter(IQueryable<TEntity> query, BaseSearchParameters parameters)
        => query;

    public virtual TEntity Create(TEntity entity)
    {
        _db.Add(entity);
        return entity;
    }

    public virtual void Update(TEntity entity)
        => _context.Update(entity);

    public virtual void Delete(TEntity entity)
        => _db.Remove(entity);

    public void CommitTransaction()
    {
        var transaction = _context.Database.CurrentTransaction;
        if (transaction != null)
            transaction.Commit();
    }
    public void BeginTransaction()
        => _context.Database.BeginTransaction();

    public void Rollback()
        => _context.Database.RollbackTransaction();
}
