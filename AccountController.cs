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
    Account currentAccount;

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
                    isRunning = false;
                    break;
                case SAVINGS_OPTION:
                    accountType = SAVINGS;
                    isRunning = false;
                    break;
                case LOAN_OPTION:
                    accountType = LOAN;
                    isRunning = false;
                    break;
                case EXIT_OPTION:
                    if (io.Confirm("Cancel creating account and return to the Admin Menu?"))
                    {
                        isRunning = false;
                    }
                    break;
            }


            {
                isRunning = false;
            }
        }

        if (!string.IsNullOrEmpty(accountType))
        {
            Account account = new Account();
            account.AccType = accountType;
            account.Customer = customer;
            account.Balance = 0m;
            account.CustomerId = customer.CustomerId;
            account.IsActive = true;

            account = AccountDao.CreateAccount(Context, account);
            io.DisplayAccountCreated(AccountDao.GetAccountById(Context, account.AccId));
        }

        else
        {
            io.DisplayMessageWithPauseOutput("No account was created...");
        }
    }

    public void AccountSelection(string purpose)
    {
        const string UPDATE = "Update";
        const string DELETE = "Delete";

        const string USERNAME = "Username", ACCOUNT_ID = "Account Id", EXIT = "Cancel";
        string[] options = { USERNAME, ACCOUNT_ID, EXIT };
        const int USERNAME_O = 1, ACCOUNT_ID_O = 2, EXIT_O = 0;
        string MENU_NAME = $"Find the Account to {purpose} by:";

        bool isRunning = true;
        while (isRunning)
        {

            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection)
            {
                case USERNAME_O:
                    GetCustomerAccounts(purpose);
                    isRunning = false;
                    break;
                case ACCOUNT_ID_O:
                    FindByAccountId(purpose);
                    isRunning = false;
                    break;
                case EXIT_O:
                    if (io.Confirm($"Cancel {purpose} operation?"))
                    {
                        isRunning = false;
                    }
                    break;
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
            FindByAccountId(purpose);

        }

    }

    private void FindByAccountId(string purpose)
    {
        const string UPDATE = "Update", DELETE = "Delete";
        int id = -1;
        id = io.GetSelectionAsInt($"Enter Account Id to {purpose}");
        if (!AccountDao.CheckIfAccountExists(Context, id))
        {
            io.DisplayDoesNotExist($"Account No. {id}");
            if (io.Confirm("Enter another Id?"))
            {
                FindByAccountId(purpose);
            }
        }

        currentAccount = Context.Accounts.Find(id);
        if (currentAccount == null)
        {
            io.DisplayMenuSwitch($"Cancelling Account {purpose} and returning to Menu...");
        }
        else
        {
            switch (purpose)
            {
                case UPDATE:
                    UpdateAccountDetails();
                    break;
                case DELETE:
                    DeleteByAccountId();
                    break;
            }
        }
    }

    private void DeleteByAccountId()
    {
        io.DisplayDeleteWarning();
        if (io.Confirm($"Are you sure you want to delete Account No. {currentAccount.AccId}?"))
        {
            DeleteTransactionsLogs();
            DeleteRequests();
            Context.Accounts.Remove(currentAccount);
            Context.SaveChanges();
            io.DisplayMenuName("Account Was Deleted");
            io.DisplayAccountDetails(currentAccount);
            io.PauseOutput();
        }
        else
        {
            io.DisplayMenuSwitch("Cancelling Account Deletion and returning to the Admin menu...");
        }
    }

    private void DeleteTransactionsLogs()
    {
        foreach (var transaction in currentAccount.TransactionLogs)
        {
            Context.Remove(transaction);
        }
        Context.SaveChanges();
    }

    private void DeleteRequests()
    {
        foreach (var request in currentAccount.Requests)
        {
            Context.Remove(request);
        }
        Context.SaveChanges();
    }

    private void UpdateAccountDetails()
    {
        if (currentAccount == null)
        {
            io.DisplayDoesNotExist($"Account with ID {currentAccount.AccId}");
            io.DisplayMenuSwitch("Returning to the admin menu...");
            return;
        }


        const string ACC_TYPE = "Account Type", BALANCE = "Balance", IS_ACTIVE = "Is Active", EXIT = "Finish Updating";
        string MENU_NAME = $"Update Account No. {currentAccount.AccId} Details for User {currentAccount.Customer.CustomerUsername}";
        string[] OPTIONS = { ACC_TYPE, BALANCE, IS_ACTIVE, EXIT };
        const int ACC_TYPE_O = 1, BALANCE_O = 2, IS_ACTIVE_O = 3, EXIT_O = 0;

        bool isRunning = true;
        while (isRunning)
        {

            io.DisplayMenuName("Update Account Details");
            int selection = io.GetMenuSelection(MENU_NAME, OPTIONS);
            switch (selection)
            {
                case ACC_TYPE_O:
                    UpdateAccountType();
                    break;
                case BALANCE_O:
                    UpdateAccountBalance();
                    break;
                case IS_ACTIVE_O:
                    UpdateAccountIsActive();
                    break;
                case EXIT_O:
                    if (io.Confirm($"Finish Updating Account No. {currentAccount.AccId}?"))
                    {
                        isRunning = false;
                    }
                    break;
            }


        }

        io.DisplayMenuSwitch("Returning to the Admin menu...");

    }

    private void UpdateAccountType()
    {
        const string CHECKING = "Checking", SAVINGS = "Savings", LOAN = "Loan", CANCEL = "Cancel";
        string MENU_NAME_TYPE = $"Select account type for Account No.{currentAccount.AccId}";
        string[] OPTIONS_TYPE = { CHECKING, SAVINGS, LOAN, CANCEL };
        const int CHECKING_O = 1, SAVINGS_O = 2, LOAN_O = 3, CANCEL_O = 0;

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME_TYPE, OPTIONS_TYPE);
            switch (selection)
            {
                case CHECKING_O:
                    currentAccount.AccType = CHECKING;
                    isRunning = false;
                    break;
                case SAVINGS_O:
                    currentAccount.AccType = SAVINGS;
                    isRunning = false;
                    break;
                case LOAN_O:
                    currentAccount.AccType = LOAN;
                    isRunning = false;
                    break;
                case CANCEL_O:
                    if (io.Confirm("Cancel updating the account type?"))
                    {
                        isRunning = false; ;
                    }
                    break;
            }
            Context.SaveChanges();
        }

        io.DisplayMenuName($"Account No. is now a {currentAccount.AccType} Account");
        io.PauseOutput();

    }

    private void UpdateAccountBalance()
    {
        decimal oldBalance = currentAccount.Balance;
        io.DisplayMenuName($"Update Balance for Account No. {currentAccount.AccId}");
        decimal amount = io.GetDecimalFromUser();
        currentAccount.Balance = amount;
        TransactionLog tl = new TransactionLog()
        {
            AccId = currentAccount.AccId,
            TransactionType = "Adjustment",
            TransactionDate = DateTime.Now,
            Amount = amount

        };
        Context.TransactionLogs.Add(tl);
        Context.SaveChanges();
        io.NewLine();
        io.DisplayMessage("Previous Balance: $" + oldBalance);
        io.DisplayMessage("Updated Balance: $" + currentAccount.Balance);
        io.PauseOutput();

    }

    private void UpdateAccountIsActive()
    {
        const string ACTIVE = "Active", NOT_ACTIVE = "Not Active", CANCEL = "Cancel";
        string[] OPTIONS = { ACTIVE, NOT_ACTIVE, CANCEL };
        string MENU_NAME = $"Update Status for Account No. {currentAccount.AccId}";
        const int ACTIVE_OPTION = 1, NOT_ACTIVE_OPTION = 2, CANCEL_OPTION = 0;

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, OPTIONS);
            switch (selection)
            {
                case ACTIVE_OPTION:
                    currentAccount.IsActive = true;
                    isRunning = false;
                    break;
                case NOT_ACTIVE_OPTION:
                    isRunning = false;
                    currentAccount.IsActive = false;
                    break;
                case CANCEL_OPTION:
                    if (io.Confirm("Cancel Account Status update?"))
                    {
                        isRunning = false;
                    }
                    break;
            }

        }
        Context.SaveChanges();
        io.DisplayMessage($"Account status is now {currentAccount.IsActive}");
        io.PauseOutput();
    }




}