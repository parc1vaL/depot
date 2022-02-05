using System.CommandLine;
using System.CommandLine.Invocation;

namespace Depot;

public static class Operations
{
    private static Repository repository = new Repository();

    public static async Task AddTransactionAsync(FileInfo file, DateTime date, double amount, string remark)
    {
        using var connection = await ConnectionManager.Connect(file);
        
        await repository.AddTransactionAsync(
            connection,
            new Transaction
            {
                Date = date,
                Amount = amount,
                Remark = remark ?? string.Empty,
            }
        );
    }

    public static async Task RemoveTransactionAsync(FileInfo file)
    {
        using var connection = await ConnectionManager.Connect(file);

        await repository.DeleteTransactionAsync(connection);
    }

    public static async Task EvaluateTransactionsAsync(FileInfo file, double? value)
    {
        using var connection = await ConnectionManager.Connect(file);

        IEnumerable<Transaction> transactions = await repository.GetTransactionsAsync(connection);

        if (value.HasValue)
        {
            transactions = transactions.Append(new Transaction { Date = DateTime.Today, Amount = value.Value, });
        }

        var result = Financial.RateOfReturn(transactions);

        Console.WriteLine($"{result:P2} p.a.");
    }

    public static async Task ListTransactionsAsync(FileInfo file, bool all, SortValue sortValue, SortDirection sortDirection, int? bottom, int? top, InvocationContext invocationContext, IConsole console)
    {
        using var connection = await ConnectionManager.Connect(file);

        IEnumerable<Transaction> transactions = await repository.GetTransactionsAsync(connection);

        transactions = (sortValue, sortDirection) switch
        {
            (SortValue.Id, SortDirection.Asc) => transactions.OrderBy(t => t.Id),
            (SortValue.Id, SortDirection.Desc) => transactions.OrderByDescending(t => t.Id),
            (SortValue.Date, SortDirection.Asc) => transactions.OrderBy(t => t.Date),
            (SortValue.Date, SortDirection.Desc) => transactions.OrderByDescending(t => t.Date),
            (SortValue.Amount, SortDirection.Asc) => transactions.OrderBy(t => t.Amount),
            (SortValue.Amount, SortDirection.Desc) => transactions.OrderByDescending(t => t.Amount),
            (SortValue.Remark, SortDirection.Asc) => transactions.OrderBy(t => t.Remark),
            (SortValue.Remark, SortDirection.Desc) => transactions.OrderByDescending(t => t.Remark),
            _ => throw new InvalidOperationException("Invalid sorting attempt."),
        };

        if (top.HasValue)
        {
            transactions = transactions.Take(top.Value);
        }
        if (bottom.HasValue)
        {
            transactions = transactions.TakeLast(bottom.Value);
        }

        Console.WriteLine("  ID  |     Date    |   Amount   | Remark");
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"{transaction.Id,5} | {transaction.Date,11:d} | {transaction.Amount,10:N2} | {transaction.Remark}");
        }
    }

    public static async Task ModifyTransactionAsync(int id, FileInfo file, DateTime? date, double? amount, string? remark, InvocationContext invocationContext, IConsole console)
    {
        using var connection = await ConnectionManager.Connect(file);

        if (!date.HasValue && ! amount.HasValue && remark is null)
        {
            console.Error.Write("At least one modified value is required.");
            console.Error.Write(Environment.NewLine);
            invocationContext.ExitCode = 1;
            return;
        }

        var transaction = await repository.GetTransactionAsync(connection, id);

        if (transaction is null)
        {
            console.Error.Write($"Transaction ID {id} does not exist.");
            console.Error.Write(Environment.NewLine);
            invocationContext.ExitCode = 1;
            return;
        }

        transaction.Date = date ?? transaction.Date;
        transaction.Amount = amount ?? transaction.Amount;
        transaction.Remark = remark ?? transaction.Remark;

        await repository.UpdateTransactionAsync(connection, transaction);

        return;
    }
}