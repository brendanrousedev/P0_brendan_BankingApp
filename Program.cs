using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Program
{
    private Dictionary<string, Dictionary<string, MenuOption>>? Menu;
    private const string menuJsonFilePath = "Resources\\menu.json";
    private const string INDENT = "    ";

    public static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }

    public void Run()
    {
        Console.Clear();
        DisplayScreen.Greeting();
        Console.WriteLine("Welcome to The Bank of Arstotzka");
        DisplayScreen.WaitForKey();

        // Loop used for the program
        while (true)
        {
            DisplayScreen.MenuName("Select User Type");
            Console.WriteLine($"{INDENT}1. Administrator");
            Console.WriteLine($"{INDENT}2. Customer");
            Console.WriteLine($"{INDENT}0. Exit Program");
            
        }
    }

    // TODO: Add comments explaining how the json menu is created
    public Program()
    {
        try
        {
            // Initialize Menu in the constructor
            string json = File.ReadAllText(menuJsonFilePath);
            Menu = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, MenuOption>>>(json);

            if (Menu == null)
            {
                Console.WriteLine("Failed to load the menu. Exiting the program.\n\n");
                DisplayScreen.Goodbye();
                Environment.Exit(0); // Exit the program
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
            DisplayScreen.Goodbye();
            Environment.Exit(0); // Exit the program
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            DisplayScreen.Goodbye();
            Environment.Exit(0); // Exit the program
        }
    }
}
