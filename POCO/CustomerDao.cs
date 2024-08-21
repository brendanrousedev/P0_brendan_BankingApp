using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class CustomerDao 
{
    P0BrendanBankingDbContext Context;
    BasicConsole io;
    public CustomerDao(P0BrendanBankingDbContext context)
    {
        Context = context;
        io = new IOConsole();
    }
    public Customer? GetCustomerByUsername(string customerUsername)
    {
        return Context.Customers.SingleOrDefault(c => c.CustomerUsername == customerUsername);
    }

    public Customer GetCustomerById(int id)
    {
        return Context.Customers.Find(id);
    }

    public int CreateNewCustomer(Customer customer)
    {
        if (CustomerExists(customer.CustomerUsername))
        {
            io.PrintMessage("${customer.CustomerUsername} already exists.");
            io.PauseOutput();
            return GetCustomerByUsername(customer.CustomerUsername).CustomerId;
        }
        Context.Customers.Add(customer);
        Context.SaveChanges();
        io.PrintMessage($"{customer.CustomerUsername} was added to the data base");
        io.PauseOutput();
        return customer.CustomerId;
    }

    public bool CustomerExists(string username)
    {
        return Context.Customers.Any(c => c.CustomerUsername == username);
    }

    public void DeleteCustomerByUsername(string username)
    {
        var customer = GetCustomerByUsername(username);
        if (customer != null)
        {
            Context.Customers.Remove(customer);
            Context.SaveChanges();
            io.PrintMessage("\n" + username + " was successfully deleted.");
        }
        else
        {
            io.DisplayDoesNotExist(username);
            io.PauseOutput();
        }

    }

}