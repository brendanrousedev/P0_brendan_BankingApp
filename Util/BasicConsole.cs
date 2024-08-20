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
    public string GetLineFromUser(string message);
    public void DisplayDoesNotExist(string username);
    public void DisplayNote(string note);
    
}