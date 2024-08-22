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
                    SelectAccount();
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
        throw new NotImplementedException();
    }

    private void GetTransactions()
    {
        throw new NotImplementedException();
    }

    private void TransferFunds()
    {
        throw new NotImplementedException();
    }

    private void DepositFunds(Account account)
    {
        throw new NotImplementedException();

    }

    private void SelectAccount()
    {
        throw new NotImplementedException();

    }

    private void WithdrawFunds()
    {
        throw new NotImplementedException();
    }

    private void GetAccountDetails()
    {
        throw new NotImplementedException();
    }
}