namespace Depot;

public class Transaction
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double Amount { get; set; }

    public string Remark { get; set; } = string.Empty;
}