using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using P0_brendan_BankingApp.POCO;

/******************************************************************************
* Author: Brendan Rouse
* 
* 1. Initially, there was one problem that had to be solved before the application
* could be written. In order to add new customers, and new admins, at least one
* admin must already exist in the DB. While it is considered poor practice to
* explicitly show a username and password in the source code, for the purposes of 
* this project, I decided it was necessary to list the username and password below
* for reference, so a user can sign in as a Admin and begin performing necessary 
* operations to perform DB operations. 
*
* username: owner
* password: password123
*******************************************************************************/

public class Program
{

    public BasicConsole io = new IOConsole();
    P0BrendanBankingDbContext Context = new P0BrendanBankingDbContext();

    public static void Main(string[] args)
    {
        Program program = new Program();
        program.Run();
    }

    public void Run()
    {
        io.Clear(); // Clear the console to ready the program
        io.DisplayGreeting(); // Display greeting to the user
        io.PauseOutput();
        // Start LoginController to get login credentials
        bool isRunning = true;
        // Main while loop for program
        while (isRunning)
        {
            LoginController lc = new LoginController(Context);
            lc.Run();

            // Once the Run method ends for lc, then the user must have selected the option to exit the program
            if (io.Confirm("Close the program?"))
            {
                isRunning = false;
            }
        }

        io.DisplayGoodbye();

        // Are there any connections that need to be closed when the program ends? 
        Environment.Exit(0); // Is this line even necessary? 


    }

    // TODO: Add comments explaining how the json menu is created

}
