using System.Reflection;
using System.Security;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class LoginController
{

    const string INDENT = "    ";
    const string MENU_NAME = "What type of user are you?";
    BasicConsole io = new IOConsole();
    const int ADMINISTRATOR_OPTION = 1, CUSTOMER_OPTION = 2, EXIT_OPTION = 0; // These values relate to how they will be displayed to the user
    const int USERNAME_INDEX = 0, PASSWORD_INDEX = 1; // These will get their element in the credentials array
    const string ADMINISTRATOR = "Administrator", CUSTOMER = "Customer";

    // C# Does only allows primitive types to be declared as const
    // after a readonly field is used, it cannot be modified
    readonly string[] options = { "Administrator", "Customer", "Exit"}; 
    

    public void Run()
    {
        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            switch (selection) 
            {
                case ADMINISTRATOR_OPTION:
                    string[] credentials = io.GetCredentials(ADMINISTRATOR);
                    if (PasswordUtils.VerifyAdmin(credentials[USERNAME_INDEX], credentials[PASSWORD_INDEX]))
                    {
                        Console.WriteLine("SUCCESSFULLY LOGGED");
                        io.PauseOutput();
                    }
                    else
                    {
                        io.PrintInvalidCredentials();
                        io.PauseOutput();
                    }
                    break;
                case CUSTOMER_OPTION:
                    Console.WriteLine("MUST IMPLEMENT CUSTOMER_CONTROLLER");
                    break;
                case EXIT_OPTION:
                    isRunning = false;
                    break;
            }
            
        }
    }

    // BuildMenu will return a string array with all the possible options
    
}