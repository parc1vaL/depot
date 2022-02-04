using System.CommandLine;

namespace Depot;

public static class Commands
{
    static Commands()
    {
        Add = new Command("add", "Adds a transaction.")
        {
            Options.Date,
            Options.Amount,
            Options.Remark,
        };
        Add.AddAlias("a");
        Add.SetHandler<string, DateTime, double, string>(
            Operations.AddTransactionAsync,
            Options.File,
            Options.Date,
            Options.Amount,
            Options.Remark);

        Remove = new Command("remove", "Removes the last transaction.");
        Remove.AddAlias("r");
        Remove.SetHandler<string>(
            Operations.RemoveTransactionAsync,
            Options.File);

        Evaluate = new Command("evaluate", "Calculates the current IRR.")
        { 
            Options.Value,
        };
        Evaluate.AddAlias("e");
        Evaluate.SetHandler<string, double?>(
            Operations.EvaluateTransactionsAsync,
            Options.File,
            Options.Value);

        List = new Command("list", "Lists transactions.")
        {
            Options.ShowAll,
        };
        List.AddAlias("l");
        List.SetHandler<string, bool>(
            Operations.ListTransactionsAsync,
            Options.File,
            Options.ShowAll);

        Root  = new RootCommand
        {
            Add,
            Remove,
            Evaluate,
            List,
        };
        Root.AddGlobalOption(Options.File);
    }

    public static RootCommand Root;

    public static Command Add;

    public static Command Remove;

    public static Command List;

    public static Command Evaluate;
}