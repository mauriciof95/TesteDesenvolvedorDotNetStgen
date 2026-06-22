using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<TEntity[]> GetByIdsAsync(long[] ids, CancellationToken cancellationToken = default);
    Task<TEntity[]> GetAllAsync(bool IncludeDeleted = false, CancellationToken cancellationToken = default);
    TEntity Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<PagedQueryResult<TEntity>> GetPagedResultAsync(BaseSearchParameters<TEntity> parameters, CancellationToken cancellationToken);
}
