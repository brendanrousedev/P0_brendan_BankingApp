using P0_brendan_BankingApp.POCO;

public class AccountController
{
    BasicConsole? io;
    AccountDao? accountDao;



    const string MENU_NAME = "Creating New Account";
    const int CHECKING_OPTION = 1,
                SAVINGS_OPTION = 2,
                LOAN_OPTION = 3,
                EXIT_OPTION = 0;
    const string CHECKING = "Checking",
                SAVINGS = "Savings",
                LOAN = "Loan",
                EXIT = "Cancel";
    const string CONFIRM_EXIT = "Cancel creation of new account?";
    Customer? Customer;

    public AccountController(Customer customer)
    {
        io = new IOConsole();
        accountDao = new AccountDao(new P0BrendanBankingDbContext());
        Customer = customer;

    }

    public void RunCreate()
    {
        string[] options = { CHECKING, SAVINGS, LOAN, EXIT };

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection)
            {
                case CHECKING_OPTION:
                    CreateAccount(CHECKING);
                    break;
                case SAVINGS_OPTION:
                    CreateAccount(SAVINGS);
                    break;
                case LOAN_OPTION:
                    CreateAccount(LOAN);
                    break;
                case EXIT_OPTION:
                    if (io.Confirm(CONFIRM_EXIT))
                    {
                        isRunning = false;
                    }
                    break;

            }

            isRunning = false;

        }

        io.PrintMessage("Returning to Admin menu...");
        io.PauseOutput();
    }

    private void CreateAccount(string accountType)
    {

        
        if (Customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Account newAccount = new Account
        {
            AccType = accountType,
            Balance = 0.00m,
            CustomerId = Customer.CustomerId,
            Customer = Customer,
            IsActive = true
        };

        accountDao.CreateNewAccount(newAccount);
        

        io.PrintMessage($"Created {newAccount.AccType} account for {newAccount.Customer.CustomerUsername}");
        io.PauseOutput();

    }

    public void RunDelete()
    {
        io.DisplayMenuName($"Delete account for {Customer.CustomerUsername}");
        Console.WriteLine("Number of accounts: " + Customer.Accounts.Count);
        io.PauseOutput();
        foreach (var account in Customer.Accounts)
        {
            Console.WriteLine($"Account Id: {account.AccId} - Account type: {account.AccType}");
            io.PauseOutput();
        }

        // TODO: Encapsulate this in IOConsole
        Console.Write("Enter the account id to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());
        accountDao.DeleteAccountById(id);
        Console.WriteLine($"Account {id} and all related transactions and requests were deleted...");
        io.PauseOutput();


    }
}

