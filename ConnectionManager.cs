using Dapper;
using Microsoft.Data.Sqlite;

namespace Depot;

public static class ConnectionManager
{
    public static async Task<SqliteConnection> Connect(FileInfo file)
    {
        var connection = new SqliteConnection($"Data Source={file.FullName}");

        await connection.ExecuteAsync(
            "CREATE TABLE IF NOT EXISTS Transactions("
            + "ID INTEGER PRIMARY KEY AUTOINCREMENT"
            + ",Date TEXT NOT NULL"
            + ",Amount REAL NOT NULL"
            + ",Remark TEXT NOT NULL"
            + ");");

        return connection;
    }
}