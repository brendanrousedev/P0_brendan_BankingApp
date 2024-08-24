using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class TransActionRepository 
{
    P0BrendanBankingDbContext? Context;

    public TransActionRepository(P0BrendanBankingDbContext context)
    {
        Context = context;

    }
}