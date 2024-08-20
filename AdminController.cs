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

    const string CONFIRM_EXIT = "Are you sure you want to return to the main menu?";
    // INSERT DAO objects here
    private CustomerDao? customerDao;

    public AdminController()
    {
        CustomerDao customerDao = new CustomerDao(new P0BrendanBankingDbContext());
    }

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
        
        // This method, along with CreateCustomer(), will create a new customer and account in the
        // DB. However, if the Customer already exists, CreateCustomer() will not run

        // The admin will start by entering the username. If that user does NOT exist in the DB,
        // The admin will then create a customer for the DB.
        // Cannot have an account unless there is a customer
        io.DisplayMenuName("Create a New Account");
        username = io.GetLineFromUser("Enter the username of an existing customer " +
            "\nor for a customer you'd like to create");
        
        Customer? customer = customerDao.GetCustomerByUsername(username);
        int customerId = 0;

        if (customer == null)
        {
            io.DisplayDoesNotExist(username);
            if (io.Confirm($"Add {username} to the database?"))
            {
                customerId = CreateCustomer(username);
            }
            
        }

        customer = customerDao.GetCustomerById(customerId);
        
        Console.WriteLine("CHECKING CUSTOMER RETRIEVED");
        Console.WriteLine("id = " + customer.CustomerId);
        Console.WriteLine("customername = " + customer.CustomerId);

    }

    public int CreateCustomer(string username)
    {
        Customer newCustomer = new Customer();
        newCustomer.CustomerUsername = username;

        // All new customers will be assigned 'password1' and will have the opportunity to reset their password
        string password = "password1";
        byte[] salt = PasswordUtils.GenerateSalt();
        newCustomer.PasswordHash = PasswordUtils.HashPassword(password, salt);
        newCustomer.Salt = salt;

        return customerDao.CreateNewCustomer(newCustomer);


    }

}