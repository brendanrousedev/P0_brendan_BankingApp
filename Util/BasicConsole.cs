using Microsoft.VisualBasic;
using P0_brendan_BankingApp.POCO;

public interface BasicConsole {

    public void PauseOutput();

    public int GetMenuSelection(string menuName, string[] options);
    public int GetSelectionAsInt();
    public int[] GetValidOptions(string[] options);
    public void DisplayGreeting();
    public void DisplayGoodbye();
    public void DisplayMenuName(string menu);
    public void Clear();
    public void NewLine();
    public void PrintInputException(Exception ex, int[] validOptions);
    public void PrintInputException(Exception ex);
    public void PrintInvalidCredentials();
    public string[] GetCredentials(string message);
    public bool Confirm(string message);
    public void PrintLoginSuccess();
    public void DisplayReturnToMainMenu();
    public string GetLine(string message);
    public void DisplayDoesNotExist(string username);
    public void DisplayNote(string note);
    public void DisplayMessageWithPauseOutput(string message);
    public decimal GetAmount(string message);
    public void DisplayAccountDetails(Account account);
    public void DisplaySummary();
    public void DisplayAccountCreated(Account account);
    public void DisplayMenuSwitch(string message);
    public Customer GetCustomerByName(P0BrendanBankingDbContext Context, string menuname, string note);
    public void DisplayMessage(string message);
    public void DisplayAllCustomerAccounts(Customer customer);
    public int GetSelectionAsInt(string message);
    public void DisplayAccountDeletion(Account account);
    public decimal GetDecimalFromUser();
    public void DisplayTransactionLog(TransactionLog tl);


    
}