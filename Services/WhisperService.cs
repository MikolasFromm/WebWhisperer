using System.Text;

namespace WebWhisperer.Services
{
    public class WhisperService
    {
        private string _userInput = string.Empty;

        private string _querySoFar = string.Empty;

        private bool _whisperEnabled = false;
        private StringBuilder _whisperText = new StringBuilder();

        public List<string> ProcessInput(string inputChar)
        {
            return new List<string> { 
                "První možnost", 
                "Druhá možnost", 
                "Třetí možnost" };
            //if (inputChar == '.')
            //{
            //    _whisperEnabled = !_whisperEnabled; // Toggle whisper mode
            //}
            //else if (_whisperEnabled)
            //{
            //    _whisperText.Append(char.ToLower(inputChar));
            //}

            //return _whisperText.ToString();
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
