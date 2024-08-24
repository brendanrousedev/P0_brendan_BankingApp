using Azure.Core.Pipeline;
using Spectre.Console;

public static class IOConsole
{
    const int MIN_USERNAME_LENGTH = 5,
        MAX_USERNAME_LENGTH = 50;

    public static string GetUsername() 
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"\nEnter Username:")
            .PromptStyle("green")
            .Validate(input =>
            {
                return input.Length >= MIN_USERNAME_LENGTH && input.Length <= MAX_USERNAME_LENGTH
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]length of Username must be between x and y characters[/]");
            }));
    }


}