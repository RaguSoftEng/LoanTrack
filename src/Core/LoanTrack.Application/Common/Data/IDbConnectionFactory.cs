using System.Data.Common;

namespace LoanTrack.Application.Common.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
