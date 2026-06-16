using Microsoft.EntityFrameworkCore.Storage;

namespace GoodHamburger.Domain.UnitOfWork;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}