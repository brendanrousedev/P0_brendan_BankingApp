using P0_brendan_BankingApp.POCO;

public static class CustomerDao
{
    public static Customer GetCustomerByUsername(P0BrendanBankingDbContext Context, string username)
    {
        int id = Context.Customers
                .Where(c => c.CustomerUsername == username)
                .Select(c => c.CustomerId)
                .FirstOrDefault();
        Customer? customer;
        customer = Context.Customers.Find(id);

        return customer;
    }
}