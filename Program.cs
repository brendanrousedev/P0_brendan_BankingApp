using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Program
{

    public static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }

    public void Run()
    {
        Console.Clear(); // Clear the console to ready the program
        IOConsole.DisplayGreeting(); // Display greeting to the user

        // Start LoginController to get login credentials
        LoginController lc = new LoginController();
        lc.Run();


        
    }

    // TODO: Add comments explaining how the json menu is created
    
}
