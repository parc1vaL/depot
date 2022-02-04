namespace Depot;

public static class Operations
{
    private static Repository repository = new Repository();

    public static Task AddTransactionAsync(string file, DateTime date, double amount, string remark)
    {
        return repository.AddTransactionAsync(
            file,
            new Transaction
            {
                Date = date,
                Amount = amount,
                Remark = remark ?? string.Empty,
            }
        );
    }

    public static Task RemoveTransactionAsync(string file)
    {
        return repository.DeleteTransactionAsync(file);
    }

    public static async Task EvaluateTransactionsAsync(string file, double? value)
    {
        IEnumerable<Transaction> transactions = await repository.GetTransactionsAsync(file);

        if (value.HasValue)
        {
            transactions = transactions.Append(new Transaction { Date = DateTime.Today, Amount = value.Value, });
        }

        var result = Financial.RateOfReturn(transactions);

        Console.WriteLine($"{result:P2} p.a.");
    }

    public static async Task ListTransactionsAsync(string file, bool all)
    {
        IEnumerable<Transaction> transactions = await repository.GetTransactionsAsync(file);

        if (!all)
        {
            transactions = transactions.TakeLast(10);
        }

        Console.WriteLine("  ID  |     Date    |   Amount   | Remark");
        foreach (var transaction in transactions)
        {
            Console.WriteLine($"{transaction.Id,5} | {transaction.Date,11:d} | {transaction.Amount,10:N2} | {transaction.Remark}");
        }
    }
}