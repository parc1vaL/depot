using Dapper;
using Microsoft.Data.Sqlite;

namespace Depot;

public class Repository
{
    private const string Filename = "data.db";

    public async Task<Transaction[]> GetTransactionsAsync()
    {
        using var connection = new SqliteConnection($"Data Source={Filename}");
        await AssertDatabaseExists(connection);
        return (await connection
            .QueryAsync<Transaction>(
                "SELECT "
                + $"ID AS {nameof(Transaction.Id)}"
                + $", Date AS {nameof(Transaction.Date)}"
                + $", Amount AS {nameof(Transaction.Amount)}"
                + $", Remark AS {nameof(Transaction.Remark)} "
                + "FROM Transactions"))
            .ToArray();
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        using var connection = new SqliteConnection($"Data Source={Filename}");
        await AssertDatabaseExists(connection);
        await connection
            .ExecuteAsync(
                "INSERT INTO Transactions (Date, Amount, Remark) "
                + $"VALUES (@{nameof(Transaction.Date)}, @{nameof(Transaction.Amount)}, @{nameof(Transaction.Remark)});",
                transaction);
    }

    public async Task DeleteTransactionAsync()
    {
        using var connection = new SqliteConnection($"Data Source={Filename}");
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