using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class LoginController
{

    const string INDENT = "    ";
    const string MENU_NAME = "User Login";
    BasicConsole io = new IOConsole();

    // C# Does only allows primitive types to be declared as const
    // after a readonly field is used, it cannot be modified
    readonly string[] options = { "Administrator", "Customer", "Exit"}; 
    

    public void Run()
    {
        while (true)
        {
            

        }
    }

    // BuildMenu will return a string array with all the possible options
    

    private void DisplayMenu(string[] options)
    {
        Console.WriteLine();
        Console.WriteLine("Choose an option from the menu below:");
        Console.WriteLine();
        for (int i = 0; i < options.Length; i++)
        {
            if (i == options.Length - 1)
            {
                Console.WriteLine($"{INDENT}{i - i}. {options[i]}"); //i - i will print 0, the default option to print exit
            }
            else
            {
                Console.WriteLine($"{INDENT}{i + 1}. {options[i]}");
            }
        }

        Console.Write("\nEnter your selection: ");
    }
}