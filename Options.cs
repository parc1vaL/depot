using System.CommandLine;

namespace Depot;

public static class Options
{
    private static string DefaultFileName = $".{Path.DirectorySeparatorChar}data.db";

    static Options()
    {
        File = new Option<string>(new[] { "--file", "-f", }, () => DefaultFileName, "The data file.")
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

        Value = new Option<double?>(new[] { "--value", "-v" }, "The current portfolio value.")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        ShowAll = new Option<bool>(new[] { "--all", "-a", }, () => false, "Shows all transactions.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
    }

    // Shared options
    public static Option<string> File;

    // Options for "Add" command
    public static Option<DateTime> Date ;
    public static Option<double> Amount;
    public static Option<string> Remark;

    // Options for "Evaluate" command
    public static Option<double?> Value;

    // Options for "List" command
    public static Option<bool> ShowAll;
}