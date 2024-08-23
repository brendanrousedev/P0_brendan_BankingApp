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
                  + "\nWhat type of user are you?"
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

        
        
    }

    private void RunDeleteAccount()
    {
        AnsiConsole.Markup("[red] ERROR: METHOD NOT FINISH[/]");
    }

    private void RunCreateAccount()
    {
        AnsiConsole.Markup("[red] ERROR: METHOD NOT FINISH[/]");
    }
}