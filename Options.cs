using System.CommandLine;

namespace Depot;

public static class Options
{
    private static readonly FileInfo DefaultFile = new("data.db");

    static Options()
    {
        File = new Option<FileInfo>(new[] { "--file", "-f", }, () => DefaultFile, "The data file.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        Date = new Option<DateTime>(new[] { "--date", "-d", }, "The transaction date.")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true,
        };

        Amount = new Option<double>(new[] { "--amount", "-a", }, "The transaction amount.") 
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true,
        };
        Remark = new Option<string>(new[] { "--remark", "-r", }, "A description for the transaction.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        Value = new Argument<double?>("value", "The current portfolio value.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };

        Count = new Option<int>(new[] { "--number", "-n", }, () => 10, "Show only the top n transactions.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };        
        SortValue = new Option<SortValue>(new[] { "--sort", "-s", }, () => Depot.SortValue.Date, "The property to sort by.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        SortDirection = new Option<SortDirection>(new[] { "--direction", "-d", }, () => Depot.SortDirection.Desc, "The sort direction.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        Reverse = new Option<bool>(new[] { "--reverse", "-r", }, () => false, "Reverses the order of the output.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };

        Id = new Argument<int>("id", "The transaction ID.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        DateModification = new Option<DateTime?>(new[] { "--date", "-d", }, "A new date for the transaction.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        AmountModification = new Option<double?>(new[] { "--amount", "-a", }, "A new amount for the transaction.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };
        RemarkModification = new Option<string?>(new[] { "--remark", "-r", }, "A new remark for the transaction.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        RemoveId = new Argument<int?>("id", "The transaction ID.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
    }

    // Shared options
    public static Option<FileInfo> File;

    // Options for "Add" command
    public static Option<DateTime> Date;
    public static Option<double> Amount;
    public static Option<string> Remark;

    // Options for "Remove" command
    public static Argument<int?> RemoveId;

    // Options for "Evaluate" command
    public static Argument<double?> Value;

    // Options for "List" command
    public static Option<int> Count;
    public static Option<SortValue> SortValue;
    public static Option<SortDirection> SortDirection;
    public static Option<bool> Reverse;

    // Options for "Modify" command
    public static Argument<int> Id;
    public static Option<DateTime?> DateModification;
    public static Option<double?> AmountModification;
    public static Option<string?> RemarkModification;
}