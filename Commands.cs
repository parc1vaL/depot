using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

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
        Add.SetHandler<FileInfo, DateTime, double, string>(
            Operations.AddTransactionAsync,
            Options.File,
            Options.Date,
            Options.Amount,
            Options.Remark);

        Remove = new Command("remove", "Removes the last transaction.");
        Remove.AddAlias("r");
        Remove.SetHandler<FileInfo>(
            Operations.RemoveTransactionAsync,
            Options.File);

        Modify = new Command("modify", "Modifies a transaction.")
        { 
            Options.Id,
            Options.DateModification,
            Options.AmountModification,
            Options.RemarkModification,
        };
        Modify.AddAlias("m");
        Modify.SetHandler<int, FileInfo, DateTime?, double?, string?, InvocationContext, IConsole>(
            Operations.ModifyTransactionAsync,
            Options.Id,
            Options.File,
            Options.DateModification,
            Options.AmountModification,
            Options.RemarkModification
        );

        Evaluate = new Command("evaluate", "Calculates the current IRR.")
        { 
            Options.Value,
        };
        Evaluate.AddAlias("e");
        Evaluate.SetHandler<FileInfo, double?>(
            Operations.EvaluateTransactionsAsync,
            Options.File,
            Options.Value);

        List = new Command("list", "Lists transactions.")
        {
            Options.ShowAll,
            Options.Count,
        };
        List.AddAlias("l");
        List.SetHandler<FileInfo, bool, int, InvocationContext, IConsole>(
            Operations.ListTransactionsAsync,
            Options.File,
            Options.ShowAll,
            Options.Count);

        Root  = new RootCommand
        {
            Add,
            Remove,
            Evaluate,
            List,
            Modify,
        };
        Root.AddGlobalOption(Options.File);
    }

    public static RootCommand Root;

    public static Command Add;

    public static Command Remove;

    public static Command Modify;

    public static Command List;

    public static Command Evaluate;
}