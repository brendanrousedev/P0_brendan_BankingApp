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
    P0BrendanBankingDbContext Context;
    Admin admin;

    public AdminController(P0BrendanBankingDbContext Context, Admin admin)
    {
        this.Context = Context;
        this.admin = admin;
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
                    DeleteAccount();
                    break;
                case UPDATE_ACCOUNT_OPTION:
                    UpdateAccountDetails();
                    break;
                case DISPLAY_SUMMARY_OPTION:
                    DisplaySummary();
                    break;
                case RESET_PASSWORD_OPTION:
                    ResetPassword();
                    break;
                case APPROVE_CHECKBOOK_OPTION:
                    ApproveCheckbook();
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

    private void ApproveCheckbook()
    {
        throw new NotImplementedException();
    }

    private void ResetPassword()
    {
        throw new NotImplementedException();
    }

    private void DisplaySummary()
    {
        throw new NotImplementedException();
    }

    private void UpdateAccountDetails()
    {
        AccountController ac = new AccountController(Context, admin);
        const string UPDATE = "Update";
        ac.AccountSelection(UPDATE);
    }

    public void CreateAccount()
    {
        io.DisplayMenuName("Create a New Account");
        io.DisplayNote("If the customer does not already exist, you will be able to create them" +
                        "\nin the database. After that, enter their username again to Create their account.");

        string username = io.GetLine("Enter Customer username");
        Customer customer = CustomerDao.GetCustomerByUsername(Context, username);
        if (customer == null)
        {
            io.DisplayDoesNotExist(username);
            if (io.Confirm($"Add {username} to the Customer Database?"))
            {
                byte[] salt = PasswordUtils.GenerateSalt();
                customer = new Customer()
                {
                    CustomerUsername = username,
                    Salt = salt,
                    PasswordHash = PasswordUtils.HashPassword("password1", salt)
                };
                Context.Customers.Add(customer);
                Context.SaveChanges();
                io.DisplayMessageWithPauseOutput($"{username} was added to the database." +
                                "\nEnter the username again to create an account");
            }

        }
        else
        {
            AccountController ac = new AccountController(Context, admin, customer);
            ac.RunCreateAccount();
        }

    }


    public void DeleteAccount()
    {
        AccountController ac = new AccountController(Context, admin);
        const string DELETE = "Delete";
        ac.AccountSelection(DELETE);
    }




}