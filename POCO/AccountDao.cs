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

    public int CreateNewAccount(Account account, Customer customer)
    {
        account.CustomerId = customer.CustomerId;
        Context.Accounts.Add(account);
        Context.SaveChanges();
        io.PrintMessage($"Created {account.AccType} account for {customer.CustomerUsername}");
        io.PauseOutput();

        return account.AccId;
    }


}