using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using P0_brendan_BankingApp.POCO;
using Spectre.Console;
public class Program
{
    P0BrendanBankingDbContext Context;

    public Program()
    {
        Context = new P0BrendanBankingDbContext();
    }
    public static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }

    public void Run()
    {
        AnsiConsole.Clear();
        DisplayGreeting();
        AnsiConsole.MarkupLine("[red]Press Enter to continue...[/]");
        Console.ReadKey();

        const string ADMIN = "Administrator", CUSTOMER = "Customer", EXIT = "Exit";
        var menu = new SelectionPrompt<string>()
            .Title("What type of user are you?")
            .PageSize(10)
            .HighlightStyle(new Style(foreground: Color.Green, background: Color.Black))
            .AddChoices(new[] {
                ADMIN,
                CUSTOMER,
                EXIT
            });

        const int USERNAME_INDEX = 0, PASSWORD_INDEX = 1;
        bool isRunning = true;
        while (isRunning)
        {
            AnsiConsole.Clear();
            string[] credentials = { "", "" };
            var choice = AnsiConsole.Prompt(menu);
            switch (choice)
            {
                case ADMIN:
                case CUSTOMER:
                    credentials = GetCredentials(choice);
                    break;
                case EXIT:
                    if (AnsiConsole.Confirm("Exit the application?"))
                    {
                        isRunning = false;
                    }
                    break;
            }

            // Check to see if either credentials are empty
            string username = credentials[USERNAME_INDEX];
            string password = credentials[PASSWORD_INDEX];
            if (choice == ADMIN && VerifyAdmin(username, password))
            {
                AnsiConsole.Clear();
                Admin? admin = Context.Admins.SingleOrDefault(a => a.AdminUsername == username);
                if (admin == null)
                {
                    AnsiConsoleHelper.WriteCouldNotFindInDb(username);
                }
                else
                {
                    AdminController ac = new AdminController(Context, admin);
                    ac.RunMainMenu();
                }
            }
            else if (choice == CUSTOMER && VerifyCustomer(username, password))
            {
                AnsiConsole.Clear();
                Customer? customer = Context.Customers.SingleOrDefault(c => c.CustomerUsername == username);
                if (customer == null)
                {
                    AnsiConsoleHelper.WriteCouldNotFindInDb(username);
                }
                else
                {
                    CustomerController cc = new CustomerController(Context, customer);
                    cc.RunMainMenu();

                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please check your password and username and try again.[/]");
                Console.ReadKey();
            }

            
            
        }

        DisplayGoodbye();


    }

    public bool VerifyAdmin(string username, string password)
    {
        return PasswordUtils.VerifyAdmin(username, password);
    }

    public bool VerifyCustomer(string username, string password)
    {
        return PasswordUtils.VerifyCustomer(username, password);
    }
    public string[] GetCredentials(string userType)
    {
        
        var username = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter {userType} username:")
                .PromptStyle("green")
                .Validate(input =>
                {
                    return input.Length > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Username cannot be empty[/]");
                }));
        
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter {userType} password:")
                .PromptStyle("green")
                .Validate(input =>
                {
                    return input.Length > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Password cannot be empty[/]");
                }));

        return new string[] {username, password};
    }

    public void DisplayGreeting()
    {
        AnsiConsole.Clear();
        AnsiConsole.Markup(@"[bold red]
        
______             _             __    ___           _        _       _         
| ___ \           | |           / _|  / _ \         | |      | |     | |        
| |_/ / __ _ _ __ | | __   ___ | |_  / /_\ \_ __ ___| |_ ___ | |_ ___| | ____ _ 
| ___ \/ _` | '_ \| |/ /  / _ \|  _| |  _  | '__/ __| __/ _ \| __|_  / |/ / _` |
| |_/ / (_| | | | |   <  | (_) | |   | | | | |  \__ \ || (_) | |_ / /|   < (_| |
\____/ \__,_|_| |_|_|\_\  \___/|_|   \_| |_/_|  |___/\__\___/ \__/___|_|\_\__,_|
                                                                                
                                                                                

        [/]");
        Console.WriteLine("Welcome to The Bank of Arstotzka");

    }

    public void DisplayGoodbye()
    {
        AnsiConsole.Clear();
        AnsiConsole.Markup(@"[bold red]
        
 _____                 _ _                
|  __ \               | | |               
| |  \/ ___   ___   __| | |__  _   _  ___ 
| | __ / _ \ / _ \ / _` | '_ \| | | |/ _ \
| |_\ \ (_) | (_) | (_| | |_) | |_| |  __/
 \____/\___/ \___/ \__,_|_.__/ \__, |\___|
                                __/ |     
                               |___/      

        [/]");
        Console.WriteLine("\n\nThank you for using the Bank of Arstotzka application");
        Console.WriteLine("The application is now closing.");
        AnsiConsole.WriteLine();
    }
}
