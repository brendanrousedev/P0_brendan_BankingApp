using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using P0_brendan_BankingApp.POCO;

public class AccountController
{
    P0BrendanBankingDbContext Context;
    Admin admin;
    BasicConsole io;
    Customer customer;

    public AccountController(P0BrendanBankingDbContext Context, Admin admin, Customer customer)
    {
        this.Context = Context;
        this.admin = admin;
        this.io = new IOConsole();
        this.customer = customer;
    }

    public AccountController(P0BrendanBankingDbContext Context, Admin admin)
    {
        this.Context = Context;
        this.admin = admin;
        this.io = new IOConsole();
    }

    public void RunCreateAccount()
    {

        const string CHECKING = "Checking", SAVINGS = "Savings", LOAN = "Loan", EXIT = "Cancel";
        string[] OPTIONS = { CHECKING, SAVINGS, LOAN, EXIT };
        const int CHECKING_OPTION = 1, SAVINGS_OPTION = 2, LOAN_OPTION = 3, EXIT_OPTION = 0;
        const string MENU_NAME = "Select the Account Type";
        string accountType = "";

        bool isRunning = true;
        while (isRunning)
        {

            int selection = io.GetMenuSelection(MENU_NAME, OPTIONS);

            switch (selection)
            {
                case CHECKING_OPTION:
                    accountType = CHECKING;
                    break;
                case SAVINGS_OPTION:
                    accountType = SAVINGS;
                    break;
                case LOAN_OPTION:
                    accountType = LOAN;
                    break;
                case EXIT_OPTION:
                    if (!io.Confirm("Cancel creating account and return to the Admin Menu?"))
                    {
                        continue;
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(accountType))
            {
                isRunning = false;
            }
        }

        Account account = new Account();
        account.AccType = accountType;
        account.Customer = customer;
        account.Balance = 0m;
        account.CustomerId = customer.CustomerId;
        account.IsActive = true;

        account = AccountDao.CreateAccount(Context, account);
        io.DisplayAccountCreated(AccountDao.GetAccountById(Context, account.AccId));
    }

    public void DeleteAccount()
    {
        const string USERNAME = "Username", ACCOUNT_ID = "Account Id", EXIT = "Cancel";
        string[] options = { USERNAME, ACCOUNT_ID, EXIT };
        const int USERNAME_O = 1, ACCOUNT_ID_O = 2, EXIT_O = 3;
        string MENU_NAME = "Delete account by username or Account Id";

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            if (selection == USERNAME_O)
            {
                GetCustomerAccounts();
                isRunning = false;
            }
            else if (selection == ACCOUNT_ID_O)
            {
                DeleteByAccountId();
            }
            else if (selection == EXIT_O)
            {
                if (io.Confirm("Cancel account deletion?"))
                {
                    isRunning = false;
                }
            }
        }


    }

    private void GetCustomerAccounts()
    {
        customer = io.GetCustomerByName(Context, "List Customer accounts", 
                        "A list of all the customer's account  " +
                        "\nwill be given. Use the Account Id to delete");
        if (customer == null)
        {
            io.DisplayMessageWithPauseOutput("Could not find the customer");
            io.DisplayMenuSwitch("Returning to the Admin menu...");

        }
        else
        {
            io.DisplayAllCustomerAccounts(customer);
            DeleteByAccountId();

        }

    }

    private void DeleteByAccountId()
    {
        int id = -1;
        while (true)
        {
            id = io.GetSelectionAsInt("Enter Account Id to delete");
            if (!AccountDao.CheckIfAccountExists(Context, id))
            {
                io.DisplayDoesNotExist($"Account #{id}");
                if (!io.Confirm("Enter another id?"))
                {
                    io.DisplayMenuSwitch("Returning to the Admin menu...");
                    break;
                }
            }
            else
            {
                break;
            }
        }

        DeleteByAccountId(id);
    }

    private void DeleteByAccountId(int id)
    {
        Account account = Context.Accounts.Find(id);
        Context.Accounts.Remove(account);
        Context.SaveChanges();
    }


}