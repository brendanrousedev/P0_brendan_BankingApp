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

    const string CONFIRM_EXIT = "Are you sure you want to return to the main menu?";

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
                    Console.WriteLine("WITHDRAW");
                    break;
                case WITHDRAW_OPTION:
                    Console.WriteLine("DEPOSIT");
                    break;
                case DEPOSIT_OPTION:
                    Console.WriteLine("UPDATE ACCOUNT");
                    break;
                case TRANSFER_OPTION:
                    Console.WriteLine("DISPLAY SUMMARY");
                    break;
                case GET_TRANSACTIONS_OPTION:
                    Console.WriteLine("RESET PASSWORD");
                    break;
                case REQUEST_CHECKBOOK_OPTION:
                    Console.WriteLine("APPROVE CHECKBOOK");
                    break;
                case UPDATE_PASSWORD:
                    Console.WriteLine("UPDATE PASSWORD");
                    break;
                case EXIT_OPTION:
                    if (io.ConfirmExit(CONFIRM_EXIT))
                    {
                        isRunning = false;
                    }
                    break;
            }
        }
    }

}