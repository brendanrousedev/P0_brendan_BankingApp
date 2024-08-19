using Newtonsoft.Json.Bson;

public static class DisplayScreen
{
    public static void Greeting()
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

    public static void Goodbye()
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


    private static void MenuName(string menu)
    {
        Console.WriteLine("*************************");
        Console.WriteLine(menu);
        Console.WriteLine("*************************");
    }

    private static void WaitForKey()
    {
        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey();
    }
}