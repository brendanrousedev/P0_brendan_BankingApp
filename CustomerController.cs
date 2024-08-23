using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class CustomerController
{
    P0BrendanBankingDbContext Context;
    Customer customer;
    string username;

    public CustomerController(P0BrendanBankingDbContext Context, Customer customer)
    {
        this.Context = Context;
        this.customer = customer;
        this.username = customer.CustomerUsername;
    }

    public void RunMainMenu()
    {
        const string CHECK_DETAILS = "Check Account Details",
                    WITHDRAW = "Withdraw Funds",
                    DEPOSIT = "Deposit Funds",
                    TRANSFER = "Transfer Funds",
                    HISTORY = "View Last 5 Transactions",
                    RESET = "Reset Password",
                    REQUEST = "Request a New Checkbook",
                    EXIT = "Exit to Main Menu";

        var menu = new SelectionPrompt<string>()
                    .Title($"Customer Menu - {customer.CustomerUsername}")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Green, background: Color.Black))
                    .AddChoices(new[] {
                            CHECK_DETAILS,
                            WITHDRAW,
                            DEPOSIT,
                            TRANSFER,
                            HISTORY,
                            RESET,
                            REQUEST,
                            EXIT
                    });

        bool isRunning = true;
        while (isRunning)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(menu);
            switch (choice)
            {
                case CHECK_DETAILS:
                    CheckDetails();
                    break;
                case WITHDRAW:
                    WithdrawFunds();
                    break;
                case DEPOSIT:
                    DepositFunds();
                    break;
                case TRANSFER:
                    TransferFunds();
                    break;
                case HISTORY:
                    ViewHistory();
                    break;
                case RESET:
                    ResetPassword();
                    break;
                case REQUEST:
                    RequestCheckbook();
                    break;
                case EXIT:
                    if (AnsiConsole.Confirm("Return to the main menu?"))
                    {
                        isRunning = false;
                    }
                    break;
            }
        }
    }

    private void RequestCheckbook()
    {
        var accountId = AnsiConsole.Prompt(
            new TextPrompt<int>("What is the Account ID?")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid Id.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                _ => ValidationResult.Success(),

                            };
                        }));
        Account account = GetAccount();
        if (account == null)
        {
            AnsiConsole.MarkupLine("[red]Cannot find the account. Returning to customer menu...[/]");
            Console.ReadKey();
            return;
        }
        else if (account.AccType == "Savings" || account.AccType == "Loan")
        {
            AnsiConsole.MarkupLine("[red]Cannot Request a checkbook for this account type. Returning to Customer menu...[/]");
            Console.ReadKey();
            return;
        }
        else if (account.CustomerId == customer.CustomerId)
        {
            AnsiConsole.MarkupLine("[red]This account does not belong to you. Cannot request checkbook.[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
        }

        const int DEFAULT_ADMIN_ID = 2; // Right now only one admin exists in the database. AdminId is 2

        Request request = new Request()
        {
            CustomerId = customer.CustomerId,
            AccId = account.AccId,
            AdminId = DEFAULT_ADMIN_ID,
            RequestType = "CHECKBOOK",
            RequestDate = DateTime.Now,
            Status = "OPEN"
        };

        Context.Requests.Add(request);
        Context.SaveChanges();
        Console.WriteLine();
        AnsiConsole.MarkupLine("[blue]Request sent to an administrator. Returning to main menu...[/]");
    }

    public Account GetAccount()
    {
        var accountId = AnsiConsole.Prompt(
            new TextPrompt<int>("What is the Account ID?")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid Id.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                _ => ValidationResult.Success(),

                            };
                        }));
        return Context.Accounts.Find(accountId);
    }

    private void ResetPassword()
    {
        var oldPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your old password: ")
            .PromptStyle("green")
        );

        if (!PasswordUtils.VerifyCustomer(customer.CustomerUsername, oldPassword))
        {
            AnsiConsole.MarkupLine("[red]Your password was incorrect.[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;
        }

        var newPassword1 = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a new password: ")
            .PromptStyle("green")
            .Validate(input =>
                {
                    return input.Length > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Password cannot be empty[/]");
                }));

        var newPassword2 = AnsiConsole.Prompt(
            new TextPrompt<string>("Reenter your new password: ")
            .PromptStyle("green").Validate(input =>
                {
                    return input.Length > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Password cannot be empty[/]");
                }));

        if (newPassword1 != newPassword2)
        {
            AnsiConsole.MarkupLine("[red]Your passwords do not match.[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;
        }

        byte[] salt = PasswordUtils.GenerateSalt();
        customer.Salt = salt;
        customer.PasswordHash = PasswordUtils.HashPassword(newPassword1, salt);
        Context.SaveChanges();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[blue]Your password was successfully changed![/]");
        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
        Console.ReadKey();


    }

    private void ViewHistory()
    {
        // This will display all transactions for the user across ALL accounts
        var recentTransactions = Context.TransactionLogs
            .Where(t => t.Account.CustomerId == customer.CustomerId)
            .OrderByDescending(t => t.TransactionDate)
            .Take(5) // Similar to SQL Command TOP
            .ToList();

        foreach (var tl in recentTransactions)
        {
            AnsiConsole.WriteLine();
            AnsiConsoleHelper.WriteTransactionDetails(tl);
            AnsiConsole.WriteLine();

        }

        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
        Console.ReadKey();

    }

    private void TransferFunds()
    {
        Account fromAccount = GetAccount();
        if (fromAccount == null)
        {
            AnsiConsole.MarkupLine("[red]The account does not exist...[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;
        }
        else if (fromAccount.CustomerId != customer.CustomerId)
        {
            AnsiConsole.MarkupLine("[red]Cannot perform transfer because the from account doesn't belong to you.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }
        else if (fromAccount.AccType == "Loan")
        {
            AnsiConsole.MarkupLine("[red]Cannot perform transfer because the from account is a loan.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }

        Account toAccount = GetAccount();
        if (toAccount == null)
        {
            AnsiConsole.MarkupLine("[red]The account does not exist...[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;
        }
        else if (toAccount.CustomerId != customer.CustomerId)
        {
            if (!AnsiConsole.Confirm("The account does not belong to you. Continue?"))
            {
                AnsiConsole.MarkupLine("[yellow]Cancelling the transfer...[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
                Console.ReadKey();
                return;
            }
        }

        var amount = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Transfer Amount: $")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid amount. Enter zero to cancel.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                _ => ValidationResult.Success(),

                            };
                        }));
        if (amount > fromAccount.Balance)
        {
            AnsiConsole.MarkupLine("[red]Cannot withdraw an amount greater than total Account Balance.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }

        fromAccount.Balance -= amount;
        if (toAccount.AccType == "Loan")
        {
            toAccount.Balance -= amount;
        }
        else
        {
            toAccount.Balance += amount;
        }

        TransactionLog tlFrom = new TransactionLog()
        {
            AccId = fromAccount.AccId,
            TransactionType = "Transfer",
            TransactionDate = DateTime.Now,
            Amount = -amount
        };

        TransactionLog tlTo = new TransactionLog()
        {
            AccId = toAccount.AccId,
            TransactionType = "Transfer",
            TransactionDate = DateTime.Now,
            Amount = amount
        };

        Context.TransactionLogs.Add(tlFrom);
        Context.TransactionLogs.Add(tlTo);
        Context.SaveChanges();
        Console.WriteLine();
        AnsiConsole.MarkupLine("[blue]Transfer completed![/]");
        AnsiConsole.WriteLine("Sending Account");
        AnsiConsoleHelper.WritePartialAccountDetails(fromAccount);
        AnsiConsole.WriteLine("Receiving Account");
        AnsiConsoleHelper.WritePartialAccountDetails(toAccount);
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");


    }

    private void DepositFunds()
    {
        Account account = GetAccount();
        if (account == null)
        {
            AnsiConsole.MarkupLine("[red]The account does not exist...[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;

        }
        else if (account.CustomerId != customer.CustomerId)
        {
            if (!AnsiConsole.Confirm("The account does not belong to you. Continue?"))
            {
                AnsiConsole.MarkupLine("[yellow]Cancelling the deposit...[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
                Console.ReadKey();
                return;
            }

        }

        var amount = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Deposit Amount: $")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid amount. Enter zero to cancel.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                _ => ValidationResult.Success(),

                            };
                        }));
        if (account.AccType == "Loan")
        {
            account.Balance -= amount;
            Context.SaveChanges();
        }
        else
        {
            account.Balance += amount;
            Context.SaveChanges();
        }

        TransactionLog tl = new TransactionLog()
        {
            AccId = account.AccId,
            TransactionType = "Deposit",
            TransactionDate = DateTime.Now,
            Amount = amount
        };

        Context.TransactionLogs.Add(tl);
        Context.SaveChanges();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[blue]Your new account balance: ${account.Balance}");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
        Console.ReadKey();

    }

    private void WithdrawFunds()
    {
        Account account = GetAccount();
        if (account == null)
        {
            AnsiConsole.MarkupLine("[red]The account does not exist...[/]");
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            Console.ReadKey();
            return;

        }
        else if (account.CustomerId != customer.CustomerId)
        {
            AnsiConsole.MarkupLine("[red]Cannot perform withdraw because the account doesn't belong to you.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }

        else if (account.AccType == "Loan")
        {
            AnsiConsole.MarkupLine("[red]Cannot withdraw from this account type.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }

        var amount = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Withdraw Amount: $")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid amount. Enter zero to cancel.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                _ => ValidationResult.Success(),

                            };
                        }));

        if (amount > account.Balance)
        {
            AnsiConsole.MarkupLine("[red]Cannot withdraw an amount greater than total Account Balance.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
            return;
        }

        TransactionLog tl = new TransactionLog()
        {
            AccId = account.AccId,
            TransactionType = "Withdraw",
            TransactionDate = DateTime.Now,
            Amount = -amount
        };

        account.Balance -= amount;
        Context.TransactionLogs.Add(tl);
        Context.SaveChanges();

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[blue]Your new account balance: ${account.Balance}[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
        Console.ReadKey();


    }

    private void CheckDetails()
    {
        var allAccounts = Context.Accounts
            .Where(a => a.CustomerId == customer.CustomerId);

        foreach (var account in allAccounts)
        {
            AnsiConsole.WriteLine();
            AnsiConsoleHelper.WritePartialAccountDetails(account);
            AnsiConsole.WriteLine();
        }
        AnsiConsole.WriteLine("\nPress any key to return to the customer menu...");
        Console.ReadKey();
    }
}