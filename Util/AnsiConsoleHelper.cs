using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public static class AnsiConsoleHelper
{
    public static void WriteCouldNotFindInDb(string message)
    {
        AnsiConsole.MarkupLine($"[red]Error: Could not locate {message}...[/]");
        Console.ReadKey();
    }

    public static void WriteReturningToMainManu()
    {
        AnsiConsole.WriteLine("Returning to the main manu...");
        Console.ReadKey();
    }

    public static void WriteAllAccountDetails(Account account)
    {
        var table = new Table();
        const string ACC_ID = "AccId",
            CUSTOMER_NAME = "CustomerUsername",
            CUSTOMER_ID = "CustomerId",
            ACC_TYPE = "AccType",
            BALANCE = "Balance",
            IS_ACTIVE = "Is Active";

        table.AddColumn(ACC_ID);
        table.AddColumn(CUSTOMER_NAME);
        table.AddColumn(CUSTOMER_ID);
        table.AddColumn(ACC_TYPE);
        table.AddColumn(BALANCE);
        table.AddColumn(IS_ACTIVE);

        table.AddRow(account.AccId.ToString(),
                account.Customer.CustomerUsername,
                account.CustomerId.ToString(),
                account.AccType,
                "$" + account.Balance.ToString(),
                account.IsActive.ToString()
        );

        AnsiConsole.Write(table);
    }

    public static void WritePartialAccountDetails(Account account)
    {
        var table = new Table();
        const string ACC_ID = "AccId",
            ACC_TYPE = "AccType",
            BALANCE = "Balance",
            IS_ACTIVE = "Is Active";

        table.AddColumn(ACC_ID);
        table.AddColumn(ACC_TYPE);
        table.AddColumn(BALANCE);
        table.AddColumn(IS_ACTIVE);

        table.AddRow(account.AccId.ToString(),
                account.AccType,
                "$" + account.Balance.ToString(),
                account.IsActive.ToString()
        );

        AnsiConsole.Write(table);
    }

    public static void WriteTransactionDetails(TransactionLog tl)
    {
        var table = new Table();
        const string TL_ID = "TransActionId",
            TL_TYPE = "TransactionType",
            AMOUNT = "Amount",
            TL_DATE = "TransactionDate";

        table.AddColumn(TL_ID);
        table.AddColumn(TL_TYPE);
        table.AddColumn(AMOUNT);
        table.AddColumn(TL_DATE);

        table.AddRow(tl.TransactionId.ToString(),
                tl.TransactionType,
                "$" + tl.Amount.ToString(),
                tl.TransactionDate.ToString()
        );

        AnsiConsole.Write(table);
    }
}