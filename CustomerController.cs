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
                REQUEST_CHECKBOOK_OPTION = 6,
                UPDATE_PASSWORD = 7,
                EXIT_OPTION = 0;
    const string ACCOUNT_DETAILS = "Check Account Details",
                WITHDRAW = "Withdraw Funds",
                DEPOSIT = "Deposit Funds",
                TRANSFER = "Transfer Funds",
                GET_TRANSACTIONS = "View Last 5 Transactions",
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
        string[] options = { ACCOUNT_DETAILS, WITHDRAW, DEPOSIT, TRANSFER, GET_TRANSACTIONS, REQUEST_CHECKBOOK, EXIT };

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
                case REQUEST_CHECKBOOK_OPTION:
                    RequestCheckbook();
                    break;
                case UPDATE_PASSWORD:
                    ResetPassword();
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
        throw new NotImplementedException();
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