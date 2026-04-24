using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public void Commit();
    public IQueryable<TEntity> AsNoTracking(IQueryable<TEntity> query);
    public TEntity GetById(long id);
    public TEntity[] GetByIds(long[] ids);
    public TEntity[] GetAll(bool IncludeDeleted = false);
    public TEntity Create(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
    public PagedQueryResult<TEntity> GetPagedResult(BaseSearchParameters parameters);
    public void CommitTransaction();
    public void BeginTransaction();
    public void Rollback();
}
