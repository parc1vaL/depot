using Dapper;
using Microsoft.Data.Sqlite;

namespace Depot;

public class Repository
{
    public async Task<Transaction[]> GetTransactionsAsync(string file)
    {
        using var connection = new SqliteConnection($"Data Source={file}");
        await AssertDatabaseExists(connection);
        return (await connection
            .QueryAsync<Transaction>(
                "SELECT "
                + $"ID AS {nameof(Transaction.Id)}"
                + $",Date AS {nameof(Transaction.Date)}"
                + $",Amount AS {nameof(Transaction.Amount)}"
                + $",Remark AS {nameof(Transaction.Remark)} "
                + "FROM Transactions"))
            .OrderBy(t => t.Date)
            .ToArray();
    }

    public async Task AddTransactionAsync(string file, Transaction transaction)
    {
        using var connection = new SqliteConnection($"Data Source={file}");
        await AssertDatabaseExists(connection);
        await connection
            .ExecuteAsync(
                "INSERT INTO Transactions (Date,Amount,Remark) "
                + $"VALUES (@{nameof(Transaction.Date)},@{nameof(Transaction.Amount)},@{nameof(Transaction.Remark)});",
                transaction);
    }

    public async Task DeleteTransactionAsync(string file)
    {
        using var connection = new SqliteConnection($"Data Source={file}");
        await AssertDatabaseExists(connection);
        await connection.ExecuteAsync(
            "DELETE FROM Transactions "
            + "WHERE ID = (SELECT MAX(ID) FROM Transactions);");
    }

    private static async Task AssertDatabaseExists(SqliteConnection connection)
    {
        await connection.ExecuteAsync(
            "CREATE TABLE IF NOT EXISTS Transactions("
            + "ID INTEGER PRIMARY KEY AUTOINCREMENT"
            + ",Date TEXT NOT NULL"
            + ",Amount REAL NOT NULL"
            + ",Remark TEXT NOT NULL"
            + ");");
    }
}