using System;

namespace NzzApp.Data
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message)
            : base(message)
        {
            
        }
    }
}