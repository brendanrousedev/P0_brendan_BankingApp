using System.Reflection.Metadata.Ecma335;
using P0_brendan_BankingApp.POCO;
using Spectre.Console;

public class CustomerHelper 
{
    P0BrendanBankingDbContext Context;
    public CustomerHelper(P0BrendanBankingDbContext Context)
    {
        this.Context = Context;
    }

    public string GetUsername()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"\nEnter Customer username:")
            .PromptStyle("green")
            .Validate(input =>
            {
                return input.Length > 6
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Username cannot be less than 6 characters");
            }));
        
    }
}