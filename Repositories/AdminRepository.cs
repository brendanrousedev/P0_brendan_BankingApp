using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class AdminRepository 
{
    P0BrendanBankingDbContext? Context;

    public AdminRepository(P0BrendanBankingDbContext context)
    {
        Context = context;

    }
}