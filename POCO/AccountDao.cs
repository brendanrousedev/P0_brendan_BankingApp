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
        // THIS LINE IS CRUCIAL
        Context.Entry(account.Customer).State = EntityState.Unchanged;
        Context.Accounts.Add(account);
        Context.SaveChanges();

        return account.AccId;
    }

    public void DeleteAccountById(int id)
    {
        // TODO: Modify Database to use triggers
        Context.Database.ExecuteSqlRaw("DELETE FROM TransactionLog WHERE AccId = {0}", id);
        Context.Database.ExecuteSqlRaw("DELETE FROM Request WHERE AccId = {0}", id);
        Account account = GetAccountById(id);
        Context.Accounts.Remove(account);
        Context.SaveChanges();
    }

    public Account GetAccountById(int id)
    {
        return Context.Accounts.Find(id);
    }


}