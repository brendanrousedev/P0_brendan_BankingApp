using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
            LoginController lc = new LoginController();
            lc.Run();

            
        }


    }

    // TODO: Add comments explaining how the json menu is created

}
