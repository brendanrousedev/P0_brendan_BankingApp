public static class IOConsole 
{
    private static void PauseOutput()
    {
        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void DisplayMenu(string menuName, string[] options)
    {

    }

    public static void DisplayGreeting()
    {
        Console.WriteLine(@"
        
______             _             __    ___           _        _       _         
| ___ \           | |           / _|  / _ \         | |      | |     | |        
| |_/ / __ _ _ __ | | __   ___ | |_  / /_\ \_ __ ___| |_ ___ | |_ ___| | ____ _ 
| ___ \/ _` | '_ \| |/ /  / _ \|  _| |  _  | '__/ __| __/ _ \| __|_  / |/ / _` |
| |_/ / (_| | | | |   <  | (_) | |   | | | | |  \__ \ || (_) | |_ / /|   < (_| |
\____/ \__,_|_| |_|_|\_\  \___/|_|   \_| |_/_|  |___/\__\___/ \__/___|_|\_\__,_|
                                                                                
                                                                                

        ");
    }

    public static void DisplayGoodbye()
    {
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


    public static void DisplayMenuName(string menu)
    {
        Console.WriteLine("*************************");
        Console.WriteLine(menu);
        Console.WriteLine("*************************");
    }

}