namespace Depot;

public static class Operations
{
    private static Repository repository = new Repository();

    public static Task AddTransactionAsync(string date, double amount, string? remark)
    {
        return repository.AddTransactionAsync(
            new Transaction
            {
                Date = DateTime.Parse(date),
                Amount = amount,
                Remark = remark ?? string.Empty,
            }
        );
    }

    public static Task RemoveTransactionAsync()
    {
        return repository.DeleteTransactionAsync();
    }

    public static async Task EvaluateTransactionsAsync(double? value)
    {
        var transactions = await repository.GetTransactionsAsync();

        if (value.HasValue)
        {
            transactions = transactions
                .Append(new Transaction { Date = DateTime.Today, Amount = value.Value, })
                .ToArray();
        }

        var result = Financial.RateOfReturn(transactions);

        Console.WriteLine(result.ToString("P2"));
    }

    public static async Task ListTransactionsAsync(bool all)
    {
        var transactions = (await repository.GetTransactionsAsync()).OrderBy(t => t.Date).AsEnumerable();

        if (!all)
        {
            transactions = transactions.TakeLast(10);
        }

        Console.WriteLine("  ID  |     Date    |   Amount   | Remark");
        foreach (var transaction in transactions)
        {
            Console.WriteLine(
                string.Format("{0,5} | {1,11:d} | {2,10:N2} | {3}", 
                transaction.Id,
                transaction.Date, 
                transaction.Amount, 
                transaction.Remark));
        }
    }
}