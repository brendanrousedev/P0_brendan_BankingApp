using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class AccountDao 
{
    P0BrendanBankingDbContext? Context;
    BasicConsole io; 

    public AccountDao(P0BrendanBankingDbContext context)
    {
        Context = context;
        io = new IOConsole();
    }

    public int CreateNewAccount(Account account)
    {
        Context.Entry(account.Customer).State = EntityState.Unchanged;
        Context.Accounts.Add(account);
        Context.SaveChanges();

        return account.AccId;
    }


}