using System.Reflection;
using System.Security;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using P0_brendan_BankingApp.POCO;

public class LoginController
{

    const string MENU_NAME = "What type of user are you?";
    BasicConsole io = new IOConsole();
    const int ADMINISTRATOR_OPTION = 1, CUSTOMER_OPTION = 2, EXIT_OPTION = 0; // These values relate to how they will be displayed to the user
    const int USERNAME_INDEX = 0, PASSWORD_INDEX = 1; // These will get their element in the credentials array
    const string ADMINISTRATOR = "Administrator", CUSTOMER = "Customer", EXIT = "Exit Program";
    P0BrendanBankingDbContext Context;
    string username = "";
    

    // C# Does only allows primitive types to be declared as const
    // after a readonly field is used, it cannot be modified

    public LoginController(P0BrendanBankingDbContext Context)
    {
        this.Context = Context;
    }



    public void Run()
    {
        string[] options = { ADMINISTRATOR, CUSTOMER, EXIT };

        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection)
            {
                case ADMINISTRATOR_OPTION:

                    if (VerifyAdmin())
                    {
                        int adminId = Context.Admins.Where(a => a.AdminUsername == username).Select(a => a.AdminId).FirstOrDefault();
                        Admin? admin = Context.Admins.Find(adminId); // If VerifyAdmin is true, then we can guarantee admin will not be null
                        AdminController ac = new AdminController(Context, admin);
                        io.PrintLoginSuccess();
                        io.PauseOutput();
                        ac.Run();
                    }
                    else
                    {
                        io.PrintInvalidCredentials();
                        io.PauseOutput();
                    }
                    break;
                case CUSTOMER_OPTION:
                    if (VerifyCustomer())
                    {
                        int customerId = Context.Customers.Where(c => c.CustomerUsername == username).Select(c => c.CustomerId).FirstOrDefault();
                        Customer? customer = Context.Customers.Find(customerId);
                        CustomerController cc = new CustomerController(Context, customer);
                        io.PrintLoginSuccess();
                        io.PauseOutput();
                        cc.Run();
                    }
                    else
                    {
                        io.PrintInvalidCredentials();
                        io.PauseOutput();
                    }
                    break;
                case EXIT_OPTION:
                    isRunning = false;
                    break;
            }

        }
    }

    public bool VerifyAdmin()
    {
        string[] credentials = io.GetCredentials(ADMINISTRATOR);
        username = credentials[USERNAME_INDEX];
        return PasswordUtils.VerifyAdmin(credentials[USERNAME_INDEX], credentials[PASSWORD_INDEX]);
    }

    public bool VerifyCustomer()
    {
        string[] credentials = io.GetCredentials(CUSTOMER);
        username = credentials[USERNAME_INDEX];
        return PasswordUtils.VerifyCustomer(credentials[USERNAME_INDEX], credentials[PASSWORD_INDEX]);
    }

}