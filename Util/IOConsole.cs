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
        this.NewLine();
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
        Console.WriteLine();
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
    }

    public void DisplayMenuName(string menu)
    {
        Console.WriteLine("*************************");
        Console.WriteLine(menu);
        Console.WriteLine("*************************");
        this.NewLine();
    }

    public int GetMenuSelection(string menuName, string[] options)
    {
        int[] validOptions = GetValidOptions(options);
        int selection = -1;

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
                throw new ArgumentOutOfRangeException($"Error: Your option must be {string.Join(", ", validOptions)}");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Error: Your option must be {string.Join(", ", validOptions)}");
        }
        
        return selection;
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
        validOptions[options.Length -1] = EXIT;

        return validOptions;
    }

    public void NewLine()
    {
        Console.WriteLine();
    }

    public void PauseOutput()
    {
        Console.Write("Enter any key to continue...");
        Console.ReadKey();
    }
}