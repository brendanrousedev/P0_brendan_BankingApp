public class ADMIN_CONTROLLER
{
    const string MENU_NAME = "Administrator Menu";
    BasicConsole io = new IOConsole();
    const int CREATE_ACCOUNT_OPTION = 1,
                DELETE_ACCOUNT_OPTION = 2,
                UPDATE_ACCOUNT_OPTION = 3,
                DISPLAY_SUMMARY_OPTION = 4,
                RESET_PASSWORD_OPTION = 5,
                APPROVE_CHECKBOOK_OPTION = 6,
                EXIT_OPTION = 0;
    const string CREATE_ACCOUNT = "Create a New Account",
                DELETE_ACCOUNT = "Delete Account",
                UPDATE_ACCOUNT = "Update Account Details",
                DISPLAY_SUMMARY = "Display Summary",
                RESET_PASSWORD = "Reset Customer Password",
                APPROVE_CHECKBOOK = "Approve Checkbook Request",
                EXIT = "Exit to Main Menu";

    const string CONFIRM_EXIT = "Are you sure you want to return to the main menu?";

    public void Run()
    {
        string[] options = { CREATE_ACCOUNT, DELETE_ACCOUNT, UPDATE_ACCOUNT, DISPLAY_SUMMARY, RESET_PASSWORD, APPROVE_CHECKBOOK, EXIT };

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection)
            {
                case CREATE_ACCOUNT_OPTION:
                    Console.WriteLine("CREATE ACCOUNT");
                    break;
                case DELETE_ACCOUNT_OPTION:
                    Console.WriteLine("DELETE ACCOUNT");
                    break;
                case UPDATE_ACCOUNT_OPTION:
                    Console.WriteLine("UPDATE ACCOUNT");
                    break;
                case DISPLAY_SUMMARY_OPTION:
                    Console.WriteLine("DISPLAY SUMMARY");
                    break;
                case RESET_PASSWORD_OPTION:
                    Console.WriteLine("RESET PASSWORD");
                    break;
                case APPROVE_CHECKBOOK_OPTION:
                    Console.WriteLine("APPROVE CHECKBOOK");
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