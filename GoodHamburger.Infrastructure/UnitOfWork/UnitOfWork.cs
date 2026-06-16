using GoodHamburger.Domain.UnitOfWork;
using GoodHamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApiDbContext _context;

    public UnitOfWork(ApiDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CommitAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        => await _context.Database.BeginTransactionAsync(ct);

}