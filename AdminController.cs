using System.Security.AccessControl;
using P0_brendan_BankingApp.POCO;

/******************************************************************************
* Author: Brendan Rouse
*
* AdminController orceshtrates all of its operations as well as implements all the 
* necessary operations for an Admin user
*
*******************************************************************************/

public class AdminController
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

    const string CONFIRM_EXIT = "Return to the main menu?";
    // INSERT DAO objects here
    private CustomerDao? customerDao =  new CustomerDao(new P0BrendanBankingDbContext());
    private AccountDao? accountDao = new AccountDao(new P0BrendanBankingDbContext());

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
                    CreateAccount();
                    break;
                case DELETE_ACCOUNT_OPTION:
                    DeleteCustomer();
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
                    if (io.Confirm(CONFIRM_EXIT))
                    {
                        isRunning = false;
                    }
                    break;
            }
        }

        io.DisplayReturnToMainMenu();
    }

    public void CreateAccount()
{
    string? username = "";
    int customerId = -1;
    Customer? customer = null;

    io.DisplayMenuName("Create a New Account");
    io.DisplayNote("If the customer does not yet exist in the database,\n" +
                   "you will be given an opportunity to create that Customer.");

    username = io.GetLineFromUser("Enter the username for the customer");

    // Check if the username was entered
    if (string.IsNullOrEmpty(username))
    {
        Console.WriteLine("Username cannot be empty.");
        return;
    }

    customer = customerDao.GetCustomerByUsername(username);

    if (customer == null)
    {
        io.DisplayDoesNotExist(username);
        if (io.Confirm($"Add {username} to the database?"))
        {
            customerId = CreateCustomer(username);
            customer = customerDao.GetCustomerById(customerId); // Re-fetch the newly created customer by ID
        }
        else
        {
            Console.WriteLine("Customer creation cancelled.");
            return;
        }
    }

    if (customer != null)
    {
        AccountController ac = new AccountController(customer);
        ac.Run();
        
    }
    else
    {
        Console.WriteLine("Failed to retrieve or create customer.");
    }
}

    public int CreateCustomer(string username)
    {
        io.DisplayMenuName("Create a New Customer");
        Customer newCustomer = new Customer();
        newCustomer.CustomerUsername = username;

        // All new customers will be assigned 'password1' and will have the opportunity to reset their password
        string password = "password1";
        byte[] salt = PasswordUtils.GenerateSalt();
        newCustomer.PasswordHash = PasswordUtils.HashPassword(password, salt);
        newCustomer.Salt = salt;


        return customerDao.CreateNewCustomer(newCustomer);
    }

    public void DeleteCustomer()
    {
        io.DisplayMenuName("Delete Customer by Username");
        io.DisplayNote("Note: Due to foreign key constraints, deleting a customer will" +
                       "also remove all their accounts and the transactions from those accounts.");
        string username = io.GetLineFromUser("Username of customer to be deleted");
        customerDao.DeleteCustomerByUsername(username);
        io.PauseOutput();
    }

    

}