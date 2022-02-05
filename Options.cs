using System.CommandLine;

namespace Depot;

public static class Options
{
    private static FileInfo DefaultFile = new FileInfo("data.db");

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

        ShowAll = new Option<bool>(new[] { "--all", "-a", }, () => false, "Shows all transactions.");
        Count = new Option<int>(new[] { "--count", "-c", }, () => 10, "Show this number of transactions.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
        Count.AddValidator(r =>
        {
            if (!string.IsNullOrEmpty(r.Token.Value) && r.FindResultFor(ShowAll)?.GetValueOrDefault<bool>() == true)
            {
                return "Options \"--all\" and \"--count\" are mutually exclusive.";
            }

            return null;
        });

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
    }

    // Shared options
    public static Option<FileInfo> File;

    // Options for "Add" command
    public static Option<DateTime> Date ;
    public static Option<double> Amount;
    public static Option<string> Remark;

    // Options for "Evaluate" command
    public static Argument<double?> Value;

    // Options for "List" command
    public static Option<bool> ShowAll;
    public static Option<int> Count;

    // Options for "Modify" command
    public static Argument<int> Id;
    public static Option<DateTime?> DateModification;
    public static Option<double?> AmountModification;
    public static Option<string?> RemarkModification;
}