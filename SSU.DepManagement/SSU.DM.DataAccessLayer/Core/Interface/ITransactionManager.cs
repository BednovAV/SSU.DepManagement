using Microsoft.EntityFrameworkCore.Storage;

namespace SSU.DM.DataAccessLayer.Core.Interface;

public interface ITransactionManager
{
    Task TransactionScope(Action<IDbContextTransaction> action);
}