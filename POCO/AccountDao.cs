using P0_brendan_BankingApp.POCO;

public static class AccountDao
{
    public static Account CreateAccount(P0BrendanBankingDbContext Context, Account account)
    {
        Context.Accounts.Add(account);
        Context.SaveChanges();
        return Context.Accounts.Find(account.AccId);
    }

    public static Account GetAccountById(P0BrendanBankingDbContext Context, int id)
    {
        return Context.Accounts.Find(id);
    }

    public static void DeleteAccountById(P0BrendanBankingDbContext Context, int id)
    {
        Context.Remove(id);
        Context.SaveChanges();
    }

    public static bool CheckIfAccountExists(P0BrendanBankingDbContext Context, int id)
    {
        Account account = Context.Accounts.Find(id); 
        return account != null;
    }
}