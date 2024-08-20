using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Program
{

    public BasicConsole io = new IOConsole();

    public static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }

    public void Run()
    {
        Console.Clear(); // Clear the console to ready the program
        io.DisplayGreeting(); // Display greeting to the user
        io.PauseOutput();
        // Start LoginController to get login credentials
        LoginController lc = new LoginController();
        lc.Run();


        
    }

    // TODO: Add comments explaining how the json menu is created
    
}
