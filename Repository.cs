using Dapper;
using Microsoft.Data.Sqlite;

namespace Depot;

public class Repository
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

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync(SqliteConnection connection)
    {
        return await connection
            .QueryAsync<Transaction>(
                "SELECT "
                + $"ID AS {nameof(Transaction.Id)}"
                + $",Date AS {nameof(Transaction.Date)}"
                + $",Amount AS {nameof(Transaction.Amount)}"
                + $",Remark AS {nameof(Transaction.Remark)} "
                + "FROM Transactions");
    }

    public async Task<Transaction?> GetTransactionAsync(SqliteConnection connection, int id)
    {
        return await connection
            .QuerySingleOrDefaultAsync<Transaction>(
                "SELECT "
                + $"ID AS {nameof(Transaction.Id)}"
                + $",Date AS {nameof(Transaction.Date)}"
                + $",Amount AS {nameof(Transaction.Amount)}"
                + $",Remark AS {nameof(Transaction.Remark)} "
                + "FROM Transactions "
                + $"WHERE ID = @Id;",
                new { Id = id, });
    }

    public async Task AddTransactionAsync(SqliteConnection connection, Transaction transaction)
    {
        await connection
            .ExecuteAsync(
                "INSERT INTO Transactions (Date,Amount,Remark) "
                + $"VALUES (@{nameof(Transaction.Date)},@{nameof(Transaction.Amount)},@{nameof(Transaction.Remark)});",
                transaction);
    }

    public async Task UpdateTransactionAsync(SqliteConnection connection, Transaction transaction)
    {
        await connection
            .ExecuteAsync(
                "UPDATE Transactions SET " 
                + $"Date =@{nameof(Transaction.Date)}"
                + $",Amount = @{nameof(Transaction.Amount)}"
                + $",Remark = @{nameof(Transaction.Remark)}"
                + $" WHERE ID = @{nameof(Transaction.Id)};",
                transaction);
    }

    public async Task DeleteTransactionAsync(SqliteConnection connection)
    {
        await connection.ExecuteAsync(
            "DELETE FROM Transactions "
            + "WHERE ID = (SELECT MAX(ID) FROM Transactions);");
    }

    public async Task DeleteTransactionAsync(SqliteConnection connection, int id)
    {
        await connection.ExecuteAsync(
            "DELETE FROM Transactions "
            + "WHERE ID = @Id;",
            new { Id = id, });
    }
}