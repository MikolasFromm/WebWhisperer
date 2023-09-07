using NaturalSQLParser.Parser;
using NaturalSQLParser.Query;
using NaturalSQLParser.Query.Secrets;
using NaturalSQLParser.Types;
using OpenAI_API;

namespace WebWhisperer.Services
{
    public class WhisperService
    {
        private string _querySoFar = string.Empty;
        private string _userInput = string.Empty;

        private char querySeparator = '.';

        private QueryAgent _queryAgent;

        public WhisperService()
        {

            _queryAgent = QueryAgent.CreateOpenAIServerQueryAgent(new OpenAIAPI(Credentials.PersonalApiKey), CsvParser.ParseCsvFile("C:\\Users\\mikol\\Documents\\SQLMock.csv"));
        }

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

            List<string> splittedQuerySoFar = querySoFar.Split(querySeparator, StringSplitOptions.RemoveEmptyEntries).ToList();

            var response = _queryAgent.ServerLikePerformQueryWithIndices(splittedQuerySoFar);

            // place the suggestion to the first place
            
            if (response.BotSuggestionIndex > 0 && response.BotSuggestionIndex <  response.NextMoves.Count())
            {
                var nextMovesList = response.NextMoves.ToList();
                nextMovesList.RemoveAt(response.BotSuggestionIndex);
                nextMovesList.Insert(0, response.BotSuggestion);

                return nextMovesList;
            }

            return response.NextMoves;
        }

        /// <summary>
        /// Loading the user input from the client side. Removes the old query and starts a new one.
        /// </summary>
        /// <param name="userInput"></param>
        public void LoadUserInput(string userInput)
        {
            // load new user input
            _userInput = userInput;

            _queryAgent.AddUserQuery(userInput);

            // clear the previous query
            _querySoFar = string.Empty;
        }
    }

}
