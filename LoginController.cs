using System.Reflection;
using System.Security;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class LoginController
{

    const string MENU_NAME = "What type of user are you?";
    BasicConsole io = new IOConsole();
    const int ADMINISTRATOR_OPTION = 1, CUSTOMER_OPTION = 2, EXIT_OPTION = 0; // These values relate to how they will be displayed to the user
    const int USERNAME_INDEX = 0, PASSWORD_INDEX = 1; // These will get their element in the credentials array
    const string ADMINISTRATOR = "Administrator", CUSTOMER = "Customer", EXIT = "Exit Program";

    // C# Does only allows primitive types to be declared as const
    // after a readonly field is used, it cannot be modified



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
                        AdminController ac = new AdminController();
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
                        CustomerController cc = new CustomerController();
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
        return PasswordUtils.VerifyAdmin(credentials[USERNAME_INDEX], credentials[PASSWORD_INDEX]);
    }

    public bool VerifyCustomer()
    {
        string[] credentials = io.GetCredentials(CUSTOMER);
        return PasswordUtils.VerifyCustomer(credentials[USERNAME_INDEX], credentials[PASSWORD_INDEX]);
    }

}