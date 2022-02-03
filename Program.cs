using System.CommandLine;
using Depot;

var dateOption = new Option<string>(new[] { "--date", "-d", }, "The transaction date.");
var amountOption = new Option<double>(new[] { "--amount", "-a", }, "The transaction amount.");
var remarkOption = new Option<string?>(new[] { "--remark", "-r", }, "A description for the transaction.");

var addCommand = new Command("add", "Adds a transaction.");
addCommand.AddOption(dateOption);
addCommand.AddOption(amountOption);
addCommand.AddOption(remarkOption);
addCommand.SetHandler(
    (string date, double amount, string? remark) => Operations.AddTransactionAsync(date, amount, remark),
    dateOption,
    amountOption,
    remarkOption);

var removeCommand = new Command("remove", "Removes the last transaction.");
removeCommand.SetHandler(() => Operations.RemoveTransactionAsync());

var valueOption = new Option<double?>(new[] { "--value", "-v" }, "The current portfolio value.");

var evaluateCommand = new Command("evaluate", "Calculates the current IRR.");
evaluateCommand.AddOption(valueOption);
evaluateCommand.SetHandler(
    (double? value) => Operations.EvaluateTransactionsAsync(value),
    valueOption);

var showAllOption = new Option<bool>(new[] { "--all", "-a", }, () => false, "Shows all transactions.");

var listCommand = new Command("list", "Lists transactions.");
listCommand.AddOption(showAllOption);
listCommand.SetHandler(
    (bool all) => Operations.ListTransactionsAsync(all),
    showAllOption
);

var cmd = new RootCommand();
cmd.AddCommand(addCommand);
cmd.AddCommand(removeCommand);
cmd.AddCommand(evaluateCommand);
cmd.AddCommand(listCommand);

return await cmd.InvokeAsync(args);

