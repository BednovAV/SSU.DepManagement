using Microsoft.EntityFrameworkCore.Storage;
using SSU.DM.DataAccessLayer.Core.Interface;

namespace SSU.DM.DataAccessLayer.Core;

public class TransactionManager : ITransactionManager
{
    private readonly ApplicationContext _context;

    public TransactionManager(ApplicationContext context)
    {
        _context = context;
    }

    public async Task TransactionScope(Action<IDbContextTransaction> action)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        action(transaction);
        await transaction.CommitAsync();
    }
}