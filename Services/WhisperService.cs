using System.Text;

namespace WebWhisperer.Services
{
    public class WhisperService
    {
        private string _userInput = string.Empty;

        private string _querySoFar = string.Empty;

        /// <summary>
        /// Every change in queryBuilder invokes new transformation build, allowing to build the query step by step, but also remove and edit the previous steps.
        /// </summary>
        /// <param name="querySoFar"></param>
        /// <returns></returns>
        public IEnumerable<string> ProcessInput(string querySoFar)
        {
            // load the new query
            _querySoFar = querySoFar;

            // process the query


            return new List<string> { 
                "První možnost", 
                "Druhá možnost", 
                "Třetí možnost" };
        }

        /// <summary>
        /// Loading the user input from the client side. Removes the old query and starts a new one.
        /// </summary>
        /// <param name="userInput"></param>
        public void LoadUserInput(string userInput)
        {
            // load new user input
            _userInput = userInput;

            // clear the previous query
            _querySoFar = string.Empty;
        }

        /// <summary>
        /// Gets the initial query to build the transformation.
        /// </summary>
        /// <returns></returns>
        public string GetInitQuery()
        {
            return "Init";
        }
    }

}
