using System.Text;

namespace WebWhisperer.Services
{
    public class WhisperService
    {
        private string _userInput = string.Empty;

        private string _querySoFar = string.Empty;

        private bool _whisperEnabled = false;
        private StringBuilder _whisperText = new StringBuilder();

        public List<string> ProcessInput(string querySoFar)
        {
            return new List<string> { 
                "První možnost", 
                "Druhá možnost", 
                "Třetí možnost" };
        }

        public void LoadUserInput(string userInput)
        {
            // load new user input
            _userInput = userInput;

            // clear the previous query
            _querySoFar = string.Empty;
        }

        public string GetInitQuery()
        {
            return "Init";
        }
    }

}
