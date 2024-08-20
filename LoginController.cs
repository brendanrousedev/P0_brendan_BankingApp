using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class LoginController
{

    const string INDENT = "    ";
    const string MENU_NAME = "What type of user are you?";
    BasicConsole io = new IOConsole();

    // C# Does only allows primitive types to be declared as const
    // after a readonly field is used, it cannot be modified
    readonly string[] options = { "Administrator", "Customer", "Exit"}; 
    

    public void Run()
    {
        bool isRunning = true;
        while (isRunning)
        {
            int selection = io.GetMenuSelection(MENU_NAME, options);
            
        }
    }

    // BuildMenu will return a string array with all the possible options
    
}