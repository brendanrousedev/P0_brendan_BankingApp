using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class CustomerController
{
    const string MENU_NAME = "Customer Menu";
    BasicConsole io = new IOConsole();
    const int ACCOUNT_DETAILS_OPTION = 1,
                WITHDRAW_OPTION = 2,
                DEPOSIT_OPTION = 3,
                TRANSFER_OPTION = 4,
                GET_TRANSACTIONS_OPTION = 5,
                RESET_PASSWORD_OPTION = 6,
                REQUEST_CHECKBOOK_OPTION = 7,
                UPDATE_PASSWORD = 8,
                EXIT_OPTION = 0;
    const string ACCOUNT_DETAILS = "Check Account Details",
                WITHDRAW = "Withdraw Funds",
                DEPOSIT = "Deposit Funds",
                TRANSFER = "Transfer Funds",
                GET_TRANSACTIONS = "View Last 5 Transactions",
                RESET_PASSWORD = "Reset Password",
                REQUEST_CHECKBOOK = "Request a New Check Book",

                EXIT = "Exit to Main Menu";

    const string CONFIRM_EXIT = "Return to the main menu?";
    Customer? customer;
    P0BrendanBankingDbContext Context;

    public CustomerController(P0BrendanBankingDbContext Context, Customer customer)
    {
        this.customer = customer;
        this.Context = Context;
    }

    public void Run()
    {
        string[] options = { ACCOUNT_DETAILS, WITHDRAW, DEPOSIT, TRANSFER, GET_TRANSACTIONS, RESET_PASSWORD, REQUEST_CHECKBOOK, EXIT };

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection)
            {
                case ACCOUNT_DETAILS_OPTION:
                    GetAccountDetails();
                    break;
                case WITHDRAW_OPTION:
                    WithdrawFunds();
                    break;
                case DEPOSIT_OPTION:
                    DepositFunds();
                    break;
                case TRANSFER_OPTION:
                    TransferFunds();
                    break;
                case GET_TRANSACTIONS_OPTION:
                    GetTransactions();
                    break;
                case RESET_PASSWORD_OPTION:
                    ResetPassword();
                    break;
                case REQUEST_CHECKBOOK_OPTION:
                    RequestCheckbook();
                    break;
                case EXIT_OPTION:
                    if (io.Confirm(CONFIRM_EXIT))
                    {
                        isRunning = false;
                    }
                    break;
            }
        }

        io.DisplayReturnToMainMenu();
    }

    private void ResetPassword()
    {
        string password = io.GetLine("Enter a new password");
        if (password.Length < 7 || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Password must be at elast 7 characters long");
        }
        else
        {
            byte[] salt = PasswordUtils.GenerateSalt();
            customer.PasswordHash = PasswordUtils.HashPassword(password, salt);
            Context.SaveChanges();
            customer.Salt = salt;
            Context.SaveChanges();
            io.DisplayMessage("Successfully Changed password!");
            io.PauseOutput();
        }

    }

    private void RequestCheckbook()
    {
        int DEFAULT_ADMIN_ID = 2;
        io.DisplayMenuName("Listing All Your Accounts....");
        io.DisplayAllCustomerAccounts(customer);
        io.DisplayNote("The account must be a checking account");
        int id = io.GetSelectionAsInt("Enter account id");

        Account account = Context.Accounts.Find(id);
        if (account == null)
        {
            Console.WriteLine("That account does not exist...");
            io.PauseOutput();
        }
        else if (account.AccType != "Checking")
        {
            Console.WriteLine("You can only request checkbooks for checking accounts");
            io.PauseOutput();
        }
        else
        {
            Request request = new Request()
            {
                Customer = customer,
                CustomerId = customer.CustomerId,
                AccId = account.AccId,
                AdminId = DEFAULT_ADMIN_ID, // TODO: MUST FIX THIS BY ASSIGNING IT TO AN ADMIN WHO IS NOT IN REQUEST TABLE< OR ADMIN WITH FEWEST REQUEST
                RequestType = "CHECKBOOK",
                RequestDate = DateTime.Now,
                Status = "OPEN"

            };

            Context.Requests.Add(request);
            Context.SaveChanges();
            Console.WriteLine("Successfully added request");
            io.PauseOutput();
        }

    }

    private void GetTransactions()
    {
        io.DisplayMenuName("Last Five Transactions");
        io.DisplayNote("This will display your last 5 transactions"
                        + "\nacross all of your accounts");
        io.DisplayAllCustomerAccounts(customer);


        var transactions = Context.TransactionLogs
                            .Where(t => t.AccId == customer.CustomerId)
                            .OrderByDescending(t => t.TransactionDate)
                            .Take(5)
                            .ToList();

        if (transactions.Count() == 0)
        {
            Console.WriteLine("There are no transactions to view");
        }
        else
        {

        }
    }

    private void TransferFunds()
    {
        Account fromAccount = GetAccount();
        Account toAccount = GetAccount();
        if (fromAccount == null || toAccount == null)
        {
            io.DisplayMessageWithPauseOutput("Cannot find the account(s)...");
            return;
        }

        io.DisplayMenuName($"Transfer Funds from Account No. {fromAccount.AccId} to Account No. {toAccount.AccId}");
        Console.WriteLine($"Account {fromAccount.AccId} Balance: ${fromAccount.Balance}");
        Console.WriteLine($"Account {toAccount.AccId} Balance: ${toAccount.Balance}");

        io.NewLine();
        decimal amount = io.GetAmount("Enter amount to transfer: $");
        if (amount <= 0)
        {
            io.DisplayMessageWithPauseOutput("Cancelling the transfer...");
        }
        else
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            Context.SaveChanges();

            io.DisplayMenuName("New Account Balances");
            io.DisplayAccountDetails(fromAccount);
            io.NewLine();
            io.DisplayAccountDetails(toAccount);
        }
    }

    public Account GetAccount()
    {
        io.DisplayMenuName("Select One of Your Accounts");
        int INVALID_ID = -1;
        io.DisplayAllCustomerAccounts(customer);
        int id = io.GetSelectionAsInt("Enter the account Id for the transaction");
        if (INVALID_ID == id && io.Confirm("Enter another Id?"))
        {
            GetAccount();
        }

        Account account = Context.Accounts.Find(id);
        if (account == null)
        {
            io.DisplayMessageWithPauseOutput("The account could not be found...");
            return null;
        }
        else
        {
            return account;
        }


    }

    private void DepositFunds()
    {
        Account account = GetAccount();
        if (account == null)
        {
            io.DisplayMessage("No account found...");
            io.PauseOutput();
            return;
        }
        else
        {
            decimal amount = io.GetAmount($"Deposit funds into Account No. {account.AccId}");
            if (amount <= 0m)
            {
                io.DisplayMessageWithPauseOutput("Cancelling deposit....");
            }
            else
            {
                if (account.AccType == "Loan")
                {
                    account.Balance -= amount;
                }
                else
                {
                    account.Balance += amount;
                }
                
                Context.SaveChanges();
                io.DisplayMessageWithPauseOutput($"New Account Balance: ${account.Balance}");
            }
        }

    }

    private void WithdrawFunds()
    {
        Account account = GetAccount();
        if (account == null)
        {
            io.DisplayDoesNotExist($"Account No. {account.AccId}");
            io.PauseOutput();
        }
        else
        {
            if (account.AccType == "Loan")
            {
                Console.WriteLine($"Cannot withdraw from this account because it is a Loan");
            }
            else
            {
                // This method as of now allows account overdraft
                Console.WriteLine($"Current balance: ${account.Balance}");
                Console.Write("$Withdraw funds from Account No. {account.AccId}");
                decimal amount = io.GetDecimalFromUser();
                if (amount <= 0m)
                {
                    io.DisplayMessageWithPauseOutput("Cancelling withdraw....");
                }
                else
                {
                    account.Balance -= amount;
                }
                Context.SaveChanges();
            }
        }
        
    }

    private void GetAccountDetails()
    {
        io.DisplayMenuName($"All Accounts for {customer.CustomerUsername}");
        io.DisplayAllCustomerAccounts(customer);
        io.PauseOutput();
    }
}