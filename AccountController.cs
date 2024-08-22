using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
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

    public void AccountSelection(string purpose)
    {
        const string UPDATE = "Update";
        const string DELETE = "Delete";

        const string USERNAME = "Username", ACCOUNT_ID = "Account Id", EXIT = "Cancel";
        string[] options = { USERNAME, ACCOUNT_ID, EXIT };
        const int USERNAME_O = 1, ACCOUNT_ID_O = 2, EXIT_O = 3;
        string MENU_NAME = $"Find the Account to {purpose} by:";

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            if (selection == USERNAME_O)
            {
                GetCustomerAccounts(purpose);
                isRunning = false;
            }
            else if (selection == ACCOUNT_ID_O)
            {
                FindAccountById(purpose);
            }
            else if (selection == EXIT_O)
            {
                if (io.Confirm($"Cancel {purpose} operation?"))
                {
                    isRunning = false;
                }
            }
        }
    }


    private void GetCustomerAccounts(string purpose)
    {

        customer = io.GetCustomerByName(Context, "List Customer accounts",
                        "A list of all the customer's account will be given." +
                        "\nUse the Account Id to perform the operation");
        if (customer == null)
        {
            io.DisplayMessageWithPauseOutput("Could not find the customer");
            io.DisplayMenuSwitch("Returning to the Admin menu...");

        }
        else
        {
            io.DisplayAllCustomerAccounts(customer);
            FindAccountById(purpose);

        }

    }

    private void FindAccountById(string purpose)
    {
        const string UPDATE = "Update", DELETE = "Delete";
        int id = -1;
        id = io.GetSelectionAsInt($"Enter Account Id to {purpose}");
        if (!AccountDao.CheckIfAccountExists(Context, id))
        {
            io.DisplayDoesNotExist($"Account #{id}");
            if (!io.Confirm("Enter another Id?"))
            {
                io.DisplayMenuSwitch("Returning to the Admin menu...");
                return;

            }
            else
            {
                FindAccountById(purpose);
            }
        }

        switch (purpose)
        {
            case UPDATE:
                UpdateAccountDetails(id);
                break;
            case DELETE:
                DeleteByAccountId(id);
                break;
        }
    }

    private void DeleteByAccountId(int id)
    {
        Account account = Context.Accounts.Find(id);
        Context.Accounts.Remove(account);
        Context.SaveChanges();
        io.DisplayMenuName("Account Was Deleted");
        io.DisplayAccountDetails(account);
        io.PauseOutput();
    }

    private void UpdateAccountDetails(int id)
    {
        Account account = Context.Accounts.Find(id);
        Account updatedAccount = account;
        if (account == null)
        {
            io.DisplayDoesNotExist($"Account with ID {id}");
            io.DisplayMenuSwitch("Returning to the admin menu...");
            return;
        }


        const string ACC_TYPE = "Account Type", BALANCE = "Balance", IS_ACTIVE = "Is Active", EXIT = "Finish Updating";
        string MENU_NAME = $"Update Account No. {account.AccId} Details for User {account.Customer.CustomerUsername}";
        string[] OPTIONS = { ACC_TYPE, BALANCE, IS_ACTIVE, EXIT };
        const int ACC_TYPE_O = 1, BALANCE_O = 2, IS_ACTIVE_O = 3, EXIT_O = 0;

        bool isRunning = true;
        while (isRunning)
        {
            account = Context.Accounts.Find(id);
            updatedAccount = account;

            io.DisplayMenuName("Update Account Details");
            int selection = io.GetMenuSelection(MENU_NAME, OPTIONS);
            if (selection == ACC_TYPE_O)
            {
                const string CHECKING = "Checking", SAVINGS = "Savings", LOAN = "Loan", CANCEL = "Cancel";
                string MENU_NAME_TYPE = $"Select account type for Account No.{account.AccId}";
                string[] OPTIONS_TYPE = { CHECKING, SAVINGS, LOAN, CANCEL };
                const int CHECKING_O = 1, SAVINGS_O = 2, LOAN_O = 3, CANCEL_O = 0;

                bool isRunningType = true;

                while (isRunningType)
                {
                    int typeSelection = io.GetMenuSelection(MENU_NAME_TYPE, OPTIONS_TYPE);
                    switch (selection)
                    {
                        case CHECKING_O:
                            updatedAccount.AccType = CHECKING;
                            break;
                        case SAVINGS_O:
                            updatedAccount.AccType = SAVINGS;
                            break;
                        case LOAN_O:
                            updatedAccount.AccType = LOAN;
                            break;
                        case EXIT_O:
                            if (!io.Confirm("Cancel updating the account type?"))
                            {
                                continue;
                            }
                            break;
                    }

                    isRunning = false;
                }
            }
            if (selection == BALANCE_O)
            {
                decimal amount = io.GetDecimalFromUser();
                if (amount != 0)
                {
                    updatedAccount.Balance = amount;
                }
            }
            if (selection == IS_ACTIVE_O)
            {
                Console.WriteLine("Enter 1 for Active, 0 for Not Active");
                int option = io.GetSelectionAsInt();
                switch (option)
                {
                    case 1:
                        updatedAccount.IsActive = true;
                        break;
                    case 0:
                        updatedAccount.IsActive = false;
                        break;
                    default:
                        io.DisplayMessage("Invalid Entry");
                        io.PauseOutput();
                        break;
                }

            }
            if (selection == EXIT_O)
            {
                if (io.Confirm($"Finish updating Account No.{account.AccId}?"))
                {
                    isRunning = false;
                }
            }

            Context.Accounts.Attach(updatedAccount);
            Context.Entry(updatedAccount).State = EntityState.Modified;
            Context.SaveChanges();
            io.DisplayMenuName($"Account Id {updatedAccount.AccId} Was Modifed");
            io.NewLine();
            io.DisplayMessage("Before");
            io.DisplayAccountDetails(account);
            io.NewLine();
            io.DisplayMessage("After");
            io.DisplayAccountDetails(updatedAccount);
            io.PauseOutput();


        }

        io.DisplayMenuSwitch("Returning to the Admin menu...");

    }


}