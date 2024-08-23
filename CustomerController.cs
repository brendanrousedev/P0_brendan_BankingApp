using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class CustomerController
{
    P0BrendanBankingDbContext Context;
    Customer customer;
    string username;

    public CustomerController(P0BrendanBankingDbContext Context, Customer customer)
    {
        this.Context = Context;
        this.customer = customer;
        this.username = customer.CustomerUsername;
    }

    public void RunMainMenu()
    {

    }
}