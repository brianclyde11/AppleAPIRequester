using System;

namespace AppleAPIConsoleApp
{
    /// <summary>
    /// Class for Handling API Request
    /// </summary>
    public class APINoResultException : Exception
    {
        public APINoResultException(string message)
            :base(message)
        {

        }

    }

}
