public class IOConsole : BasicConsole
{

    const String INDENT = "    ";
    const int EXIT = 0;
    public void Clear()
    {
        Console.Clear();
    }

    public void DisplayGoodbye()
    {
        Clear();
        Console.WriteLine(@"
        
 _____                 _ _                
|  __ \               | | |               
| |  \/ ___   ___   __| | |__  _   _  ___ 
| | __ / _ \ / _ \ / _` | '_ \| | | |/ _ \
| |_\ \ (_) | (_) | (_| | |_) | |_| |  __/
 \____/\___/ \___/ \__,_|_.__/ \__, |\___|
                                __/ |     
                               |___/      

        ");
        Console.WriteLine("\n\nThank you for using the Bank of Arstotzka application");
        Console.WriteLine("The application is now closing.");
        NewLine();
    }

    public void DisplayGreeting()
    {
        this.Clear();
        Console.WriteLine(@"
        
______             _             __    ___           _        _       _         
| ___ \           | |           / _|  / _ \         | |      | |     | |        
| |_/ / __ _ _ __ | | __   ___ | |_  / /_\ \_ __ ___| |_ ___ | |_ ___| | ____ _ 
| ___ \/ _` | '_ \| |/ /  / _ \|  _| |  _  | '__/ __| __/ _ \| __|_  / |/ / _` |
| |_/ / (_| | | | |   <  | (_) | |   | | | | |  \__ \ || (_) | |_ / /|   < (_| |
\____/ \__,_|_| |_|_|\_\  \___/|_|   \_| |_/_|  |___/\__\___/ \__/___|_|\_\__,_|
                                                                                
                                                                                

        ");
        Console.WriteLine("Welcome to The Bank of Arstotzka");

    }

    public void DisplayMenuName(string menu)
    {
        Clear();
        Console.WriteLine("*************************");
        Console.WriteLine(menu);
        Console.WriteLine("*************************");
    }

    public int GetMenuSelection(string menuName, string[] options)
    {
        int[] validOptions = GetValidOptions(options);
        int selection = -1;

        Clear();
        DisplayMenuName(menuName);
        NewLine();

        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{INDENT}{validOptions[i]}. {options[i]}");
        }
        NewLine();
        Console.Write("Enter a number for you selection: ");

        // enclose options in try / catch
        try
        {
            selection = Convert.ToInt32(Console.ReadLine());
            if (!validOptions.Contains(selection))
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            PrintInputException(ex, validOptions);
            PauseOutput();
        }
        catch (FormatException ex)
        {
            PrintInputException(ex, validOptions);
            PauseOutput();
        }
        catch (OverflowException ex)
        {
            PrintInputException(ex, validOptions);
            PauseOutput();
        }

        return selection;
    }

    public string[] GetCredentials(string message)
    {
        string? username = "";
        string? password = "";

        DisplayMenuName($"Enter {message} Credentials");
        NewLine();
        try
        {
            Console.Write("Username: ");
            username = Console.ReadLine();
        }
        catch (ArgumentNullException ex)
        {
            PrintInputException(ex);
        }
        catch (InvalidOperationException ex)
        {
            PrintInputException(ex);
        }

        try
        {
            Console.Write("Password: ");
            password = Console.ReadLine();
        }
        catch (ArgumentNullException ex)
        {
            PrintInputException(ex);
        }
        catch (InvalidOperationException ex)
        {
            PrintInputException(ex);
        }

        // C# Really does not like it if it is possible to pass a null reference to a variable.
        // ?? will provide a default value if either username or password is null
        return new string[] { username ?? string.Empty, password ?? string.Empty };
    }

    public int GetSelectionAsInt()
    {
        Console.Write("Enter a number for you selection: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    public int[] GetValidOptions(string[] options)
    {
        // The last element in the array should be zero
        // So, the for loop will only go to the second-to-last index
        int[] validOptions = new int[options.Length];
        for (int i = 0; i < options.Length - 1; i++)
        {
            validOptions[i] = i + 1;
        }
        validOptions[options.Length - 1] = EXIT;

        return validOptions;
    }

    public void NewLine()
    {
        Console.WriteLine();
    }

    public void PauseOutput()
    {
        NewLine();
        Console.Write("Enter any key to continue...");
        Console.ReadKey();
    }

    public void PrintInputException(Exception ex, int[] validOptions)
    {
        NewLine();
        Console.WriteLine($"Error: {ex.Message}", ex);
        Console.WriteLine($"Valid selections are: {string.Join(", ", validOptions)}");
    }

    public void PrintInputException(Exception ex)
    {
        NewLine();
        Console.WriteLine($"Error: {ex.Message}", ex);
    }

    public void PrintInvalidCredentials()
    {
        NewLine();
        Console.WriteLine("Please check your password and account name and try again.");
    }

    public bool Confirm(string message)
    {
        NewLine();
        Console.Write($"{message} (Enter 'y' for Yes): ");
        string? option = Console.ReadLine()?.Trim().ToLower();
        if (string.IsNullOrEmpty(option))
        {
            option = "n";
        }
        return option == "y";
    }

    public void PrintLoginSuccess()
    {
        Clear();
        DisplayMenuName("Successfully Logged in!");
    }

    public void DisplayReturnToMainMenu()
    {
        Clear();
        DisplayMenuName("Logging out and returning to the main menu...");
    }

    public string GetLineFromUser(string message)
    {
        NewLine();
        try
        {
            Console.Write($"{message}: ");
            string? line = Console.ReadLine();
        }
        catch (Exception ex)
        {
            PrintInputException(ex);
        }

        return message;

    }

    public void DisplayDoesNotExist(string username)
    {
        Console.WriteLine($"{username} does not exist.");
    }

}