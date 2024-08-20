using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class BankingAppController
{

    const string INDENT = "    ";
    private void Displ()
    {

        DisplayScreen.MenuName("User Login");
        string ADMIN_LOGIN = "Administrator";
        string CUSTOMER_LOGIN = "Customer";
        string EXIT = "Exit the program";
        string[] MENU_OPTIONS = { ADMIN_LOGIN, CUSTOMER_LOGIN, EXIT };
        DisplayMenu(MENU_OPTIONS);

    }

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