using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class RequestRepository 
{
    P0BrendanBankingDbContext? Context;

    public RequestRepository(P0BrendanBankingDbContext context)
    {
        Context = context;

    }
}