using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using P0_brendan_BankingApp.POCO;
using Spectre.Console;

/******************************************************************************
* Author: Brendan Rouse
*
* AdminController orceshtrates all of its operations as well as implements all the 
* necessary operations for an Admin user
*
*******************************************************************************/


public class AdminController
{
    P0BrendanBankingDbContext Context;
    Admin admin;

    public AdminController(P0BrendanBankingDbContext Context, Admin admin)
    {
        this.Context = Context;
        this.admin = admin;
    }

    public void RunMainMenu()
    {
        const string CREATE = "Create a New Account",
        DELETE = "Delete Account",
        UPDATE = "Update Account Details",
        SUMMARY = "Display Summary",
        RESET = "Reset Customer Password",
        APPROVE = "Approve Checkbook Request",
        EXIT = "Exit to Main Menu";


        var menu = new SelectionPrompt<string>()
            .Title("***************************"
                  + $"\nAdmin menu - {admin.AdminUsername}"
                  + "\n***************************")
            .PageSize(10)
            .HighlightStyle(new Style(foreground: Color.Green, background: Color.Black))
            .AddChoices(new[] {
                CREATE,
                DELETE,
                UPDATE,
                SUMMARY,
                RESET,
                APPROVE,
                EXIT
            });

        bool isRunning = true;
        while (isRunning)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(menu);
            switch (choice)
            {
                case CREATE:
                    RunCreateAccount();
                    break;
                case DELETE:
                    RunDeleteAccount();
                    break;
                case UPDATE:
                    UpdateAccount();
                    break;
                case SUMMARY:
                    GetSummary();
                    break;
                case RESET:
                    ResetCustomerPassword();
                    break;
                case APPROVE:
                    ApproveAllCheckbookRequests();
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

    private void ApproveAllCheckbookRequests()
    {
        AnsiConsole.Clear();
        int openRequestCount = Context.Requests.Where(r => r.Status == "Open").Count();
        if (openRequestCount == 0)
        {
            AnsiConsole.Markup("[red]There are no open requests. Press any key to return to the Admin menu...[/]");
            Console.ReadKey();
        }
        else if (AnsiConsole.Confirm($"There are {openRequestCount} request(s) open, approve all?"))
        {
            foreach (var request in Context.Requests)
            {
                if (request.RequestType == "CHECKBOOK" && request.Status == "OPEN")
                {
                    request.Status = "APPROVED";

                }

            }
            Context.SaveChanges();
            AnsiConsole.Markup($"[blue]All {openRequestCount} request(s) have been approved! Press any key to continue...[/]");
            Console.ReadKey();
        }

        else
        {
            AnsiConsole.Write("No checkbook requests were approved...");
            Console.ReadKey();

        }
    }

    private void ResetCustomerPassword()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("************************"
                           + "\nReset Customer Password"
                           + "\n************************");
        AnsiConsole.WriteLine();
        var username = AnsiConsole.Prompt(new TextPrompt<string>("Enter customer username: ")
                        .PromptStyle("green"));
        Customer? customer = Context.Customers.SingleOrDefault(c => c.CustomerUsername == username);
        if (customer == null)
        {
            AnsiConsoleHelper.WriteCouldNotFindInDb(username);
            return;
        }
        
        if (AnsiConsole.Confirm($"Reset password for {username}?"))
        {
            string defaultPassword = "password1";
            byte[] salt = PasswordUtils.GenerateSalt();
            customer.PasswordHash = PasswordUtils.HashPassword(defaultPassword, salt);
            customer.Salt = salt;
            Console.WriteLine();
            AnsiConsole.Markup($"[blue] Password is now set to the default for {username}. Enter any key to continue...[/]");
            Console.ReadKey();
        }

    }

    private void GetSummary()
    {
        AnsiConsole.WriteLine("********************"
                             +"\nDatabase Summary"
                             +"\n*****************");
        AnsiConsole.WriteLine();
        var tableCount = new Table();
        tableCount.AddColumn("Table");
        tableCount.AddColumn("Count");

        const string ADMINS = "Administrators", CUSTOMERS = "Customers", ACCOUNTS = "Accounts", TRANSACTION_LOGS = "Transactions Logs", REQUESTS = "Requests";

        int adminCount = Context.Admins.Count();
        int customerCount = Context.Customers.Count();
        int accountCount = Context.Accounts.Count();
        int transactionCount = Context.TransactionLogs.Count();
        int requestCount = Context.Requests.Count();

        tableCount.AddRow(ADMINS, adminCount.ToString());
        tableCount.AddRow(CUSTOMERS, customerCount.ToString());
        tableCount.AddRow(ACCOUNTS, accountCount.ToString());
        tableCount.AddRow(TRANSACTION_LOGS, transactionCount.ToString());
        tableCount.AddRow(REQUESTS, requestCount.ToString());

        AnsiConsole.Write(tableCount);
        AnsiConsole.WriteLine();

        var tableAverage = new Table();
        tableAverage.AddColumn("Average Account Balance");
        tableAverage.AddColumn("Average Transaction Balance");

        decimal balanceAvg = Context.Accounts.Average(a => a.Balance);
        decimal transactionAvg = Context.TransactionLogs.Average(t => t.Amount);

        tableAverage.AddRow(balanceAvg.ToString(), transactionAvg.ToString());
        AnsiConsole.Write(tableAverage);
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[blue]Finished querying database. Press any key to continue...[/]");
        Console.ReadKey();


    }

    private void UpdateAccount()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("******************"
                            +"\nUpdate Account"
                             +"\n****************");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        var accountId = AnsiConsole.Prompt(new TextPrompt<string>("Enter Account ID: ")
                        .PromptStyle("green"));
        Account? account = Context.Accounts.Find(accountId);

        if (account == null)
        {
            AnsiConsoleHelper.WriteCouldNotFindInDb($"Account with Id {accountId}");
            return;
        }

        const string TYPE = "Account Type", IS_ACTIVE = "Active Status", BALANCE = "Balance", CANCEL = "Cancel";
        var menu = new SelectionPrompt<string>()
                    .Title("***********************************************************"
                  + $"\nUpdate Account No. {account.AccId} for {account.Customer.CustomerUsername}"
                  + "\n****************************************************************")
                  .PageSize(10)
                  .HighlightStyle(new Style(foreground: Color.Green, background: Color.Black))
                  .AddChoices(new[] {
                        TYPE,
                        IS_ACTIVE,
                        BALANCE,
                        CANCEL,
                  });

        var choice = AnsiConsole.Prompt(menu);

        switch(choice)
        {
            case TYPE:
                UpdateAccountType(account);
                break;
            case IS_ACTIVE:
                UpdateIsActive(account);
                break;
            case BALANCE:
                UpdateBalance(account);
                break;
            case CANCEL:
                if(AnsiConsole.Confirm("Cancel account update?"))
                {
                    AnsiConsole.WriteLine("Cancelling account update. Press any key to reutn to the Admin menu...");
                    Console.ReadKey();
                    return;
                }
                break;
        }

        
        
    }

    private void UpdateBalance(Account account)
    {
        decimal beforeBalance = account.Balance;
        var amount = AnsiConsole.Prompt(
            new TextPrompt<decimal>("What is the new account balance? $")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid number.[/]")
                        .Validate(amount =>
                        {
                            return amount switch
                            {
                                < 0m => ValidationResult.Error("Amount cannot be negative.[/]"),
                                _ => ValidationResult.Success(),

                            };
                        }));
        if (AnsiConsole.Confirm($"Set account balance to ${amount}?"))
        {
            TransactionLog tl = new TransactionLog {
                    AccId = account.AccId,
                    Account = account,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Adjustment"
            };
            Context.TransactionLogs.Add(tl);
            account.Balance = amount;
            
            
            Context.SaveChanges();
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[blue]Balance Before: ${beforeBalance}[/]");
            AnsiConsole.MarkupLine($"[blue]Balance After: ${account.Balance}[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[blue]Enter any key to return to the menu...[/]");
            Console.ReadKey();
        }
        else
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[yellow]Balance was not updated. Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
    }

    private void UpdateAccountType(Account account)
    {
        const string CHECKING = "Checking", SAVINGS = "Savings", LOAN = "Loan";
        var menu = new SelectionPrompt<string>()
                    .Title("******************"
                    +"\nSetting Account Type"
                        +"\n******************")
                    .HighlightStyle(new Style(foreground: Color.Green, background: Color.Black))
                    .PageSize(10)
                    .AddChoices(new [] {
                       CHECKING,
                       SAVINGS,
                       LOAN 
                    });

        var choice = AnsiConsole.Prompt(menu);
        AnsiConsole.WriteLine();
        if (AnsiConsole.Confirm($"Set the account type to {choice}?"))
        {
            account.AccType = choice;
            Context.SaveChanges();
            AnsiConsole.MarkupLine($"[blue]Account type was set to {choice}! Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]The account type was not updated. Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
    }

    private void UpdateIsActive(Account account)
    {
        bool isActive = AnsiConsole.Confirm("Is the Account Active?");
        account.IsActive = isActive;
        Context.SaveChanges();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[blue]Successfully updated Is Active: {account.IsActive}. Press any key to return to the menu...");
    }

    private void RunDeleteAccount()
    {
        Account account = GetAccountById();
        if (account == null)
        {
            return;
        }
        else 
        {
            AnsiConsoleHelper.WriteAllAccountDetails(account);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[red]WARNING: Account deletion will also delete all related transactions and requests.");
            AnsiConsole.WriteLine();
            if(AnsiConsole.Confirm("Are you sure you want to delete this account?"))
            {
                foreach(var request in Context.Requests)
                {
                    if (request.AccId == account.AccId)
                    {
                        Context.Remove(request);
                    }
                }

                foreach(var tl in Context.TransactionLogs)
                {
                    if (tl.AccId == account.AccId)
                    {
                        Context.Remove(tl);
                    }
                }

                Context.Remove(account);
                Context.SaveChanges();
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[blue]The account was successfully deleted. Press any key to return to the menu...[/]");
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Account deletion was cancelled. Press any key to return to the main menu...");
                Console.ReadKey();
            }
        }

    }

    private void RunCreateAccount()
    {
        AnsiConsole.Markup("[red] ERROR: METHOD NOT FINISH[/]");
    }

    private Account GetAccountById()
    {
        var accountId = AnsiConsole.Prompt(new TextPrompt<string>("Enter Account ID: ")
                        .PromptStyle("green"));
        Account? account = Context.Accounts.Find(accountId);

        if (account == null)
        {
            AnsiConsoleHelper.WriteCouldNotFindInDb($"Account with Id {accountId}");
            return account;
        }
        else
        {
            return account;
        }
    }
}