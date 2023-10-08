﻿using NaturalSQLParser.Parser;
using NaturalSQLParser.Query;
using NaturalSQLParser.Query.Secrets;
using NaturalSQLParser.Types;
using NaturalSQLParser.Types.Tranformations;
using OpenAI_API;

namespace WebWhisperer.Services
{
    public class WhisperService
    {
        private string _querySoFar = string.Empty;
        private string _userInput = string.Empty;

        private char querySeparator = '.';

        private QueryAgent _queryAgent;
        private List<Field> _inputFields;
        private IEnumerable<ITransformation> _transformations;

        private bool _isInputFieldLoaded = false;

        public WhisperService()
        {
            _queryAgent = QueryAgent.CreateOpenAIServerQueryAgent(new OpenAIAPI(Credentials.PersonalApiKey), true);
        }

        public bool IsIntputFieldLoaded { get { return _isInputFieldLoaded; } }

        public void LoadInputFields(List<Field> inputFields)
        {
            _inputFields = inputFields;
            _isInputFieldLoaded = true;
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

            var response = _queryAgent.ServerLikePerformQueryWithIndices(splittedQuerySoFar, _inputFields);

            // place the suggestion to the first place
            
            if (response.BotSuggestionIndex > 0 && response.BotSuggestionIndex <  response.NextMoves.Count())
            {
                var nextMovesList = response.NextMoves.ToList();
                nextMovesList.RemoveAt(response.BotSuggestionIndex);
                nextMovesList.Insert(0, response.BotSuggestion);

                return nextMovesList;
            }

            _transformations = response.Transformations;
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

        /// <summary>
        /// Returns the current table in a CSV format.
        /// When the transformation is not done yet, it returns the latest table.
        /// </summary>
        /// <returns>CSV table</returns>
        public string GetCurrentTable()
        {
            if (_transformations is not null)
            {
                var fields = Transformator.TransformFields(_inputFields, _transformations);
                var result = CsvParser.ParseFieldsIntoCsv(fields);
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
