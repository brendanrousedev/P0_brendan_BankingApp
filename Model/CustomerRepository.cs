using System.Reflection.Metadata.Ecma335;
using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class CustomerRepository 
{
    P0BrendanBankingDbContext Context;
    const int MIN_USERNAME_LENGTH = 5;
    public CustomerRepository(P0BrendanBankingDbContext Context)
    {
        this.Context = Context;
    }

    // A method that will prompt the user for a username
    
}