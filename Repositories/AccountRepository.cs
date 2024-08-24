using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class AccountRepository 
{
    P0BrendanBankingDbContext? Context;

    public AccountRepository(P0BrendanBankingDbContext context)
    {
        Context = context;

    }
}