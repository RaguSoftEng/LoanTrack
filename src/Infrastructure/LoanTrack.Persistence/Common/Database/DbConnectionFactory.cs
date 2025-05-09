using System.Data.Common;
using LoanTrack.Application.Common.Data;
using Npgsql;

namespace LoanTrack.Persistence.Common.Database;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync() =>
        await dataSource.OpenConnectionAsync();
}
