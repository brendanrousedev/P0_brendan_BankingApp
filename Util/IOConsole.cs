using P0_brendan_BankingApp.POCO;

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
        Console.Write("Enter a number for your selection: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    public int GetSelectionAsInt(string message)
    {
        Console.Write(message + ": ");
        try
        {
            int selection = Convert.ToInt32(Console.ReadLine());
            return selection;
        }
        catch (FormatException ex)
        {
            PrintInputException(ex);
            return -1;
        }
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
        Console.WriteLine(message);
        Console.Write("Enter 'y' for Yes, any other key for No): ");
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

    public string GetLine(string message)
    {
        NewLine();
        string? line = "";
        try
        {
            Console.Write($"{message}: ");
            line = Console.ReadLine();
        }
        catch (Exception ex)
        {
            PrintInputException(ex);
        }

        return line;

    }

    public void DisplayDoesNotExist(string message)
    {
        NewLine();
        Console.WriteLine($"{message} does not exist.");
    }

    public void DisplayNote(string note)
    {
        NewLine();
        Console.Write($"Note: {note}");
        NewLine();
    }

    public void DisplayMessageWithPauseOutput(string message)
    {
        NewLine();
        Console.WriteLine(message);
        PauseOutput();
    }

    public decimal GetAmount(string message)
    {
        DisplayMenuName(message);
        NewLine();
        Console.Write("Enter amount $");
        decimal amount = 0m;
        try
        {
            amount = Convert.ToDecimal(Console.ReadLine());
        }
        catch (FormatException ex)
        {
            PrintInputException(ex);
        }


        return amount;
    }

    public void DisplayAccountDetails(Account account)
    {
        NewLine();
        Console.WriteLine($"Account Id: {account.AccId}");
        Console.WriteLine($"Account Type: {account.AccType}");
        Console.WriteLine($"Account Balance: ${account.Balance}");
        Console.WriteLine($"Account is Active: {account.IsActive}");
        Console.WriteLine($"Number of Open Requests: {account.Requests}");
        NewLine();
    }

    public void DisplaySummary()
    {
        throw new NotImplementedException();
    }

    public void DisplayAccountCreated(Account account)
    {
        Clear();
        DisplayMenuName($"New Account Created for {account.Customer.CustomerUsername}");
        Console.WriteLine("Account ID: " + account.AccId);
        Console.WriteLine("Account Type: " + account.AccType);
        Console.WriteLine("Is Active: " + account.IsActive);
        Console.WriteLine("Balance: $" + account.Balance);
        NewLine();
        PauseOutput();
    }

    public void DisplayMenuSwitch(string message)
    {
        Clear();
        Console.WriteLine(message + "...");
        PauseOutput();
    }

    // GetCustomerBynames acts as a mini menu, so more paramters are required
    public Customer GetCustomerByName(P0BrendanBankingDbContext Context, string menuName, string note)
    {
        DisplayMenuName(menuName);
        DisplayNote(note);
        bool isRunning = true;
        Customer? customer = new Customer();
        NewLine();
        string username = GetLine("Enter customer username");
        customer = CustomerDao.GetCustomerByUsername(Context, username);
        if (customer == null)
        {
            DisplayDoesNotExist(username);
            if (!Confirm("Try reentering username?"))
            {
                isRunning = false;
            }
            else
            {
                GetCustomerByName(Context, menuName, note);
            }
        }

        Console.WriteLine(customer.CustomerId);
        Console.WriteLine(customer.CustomerUsername);
        Console.WriteLine(customer.PasswordHash);
        PauseOutput();

        return customer;
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void DisplayAllCustomerAccounts(Customer customer)
    {
        Console.WriteLine($"{customer.CustomerUsername} has {customer.Accounts.Count}");

        foreach (Account account in customer.Accounts)
        {
            DisplayAccountDetails(account);
        }
    }

    public void DisplayAccountDeletion(Account account)
    {
        Console.Clear();
        Console.WriteLine($"Successfully deleted Account #{account.AccId}");
        PauseOutput();
    }

    public decimal GetDecimalFromUser()
    {
        decimal value = 0m;
        try
        {
            Console.Write("Enter amount: $");
            value = Convert.ToDecimal(Console.ReadLine());
        }
        catch (FormatException ex)
        {
            PrintInputException(ex);
            if(Confirm("Enter another value?"))
            {
                return GetDecimalFromUser();
            }
        }

        return value;

    }

    public void DisplayTransactionLog(TransactionLog tl)
    {
        NewLine();
        Console.WriteLine($"Transaction ID: {tl.TransactionId}");
        Console.WriteLine($"{INDENT}Type: {tl.TransactionType}");
        Console.WriteLine($"{INDENT}Transaction Amount: ${tl.Amount}");
        NewLine();
    }
}