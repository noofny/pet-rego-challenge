using System;
using PetRego.Models;

namespace PetRego.Data
{
    public class DataException<T> : Exception where T : IEntity
    {
        public string Type { get; private set; }    // not used yet, but would be useful for logging
        public string Action { get; private set; }  // not used yet, but would be useful for logging
        public Result Result { get; private set; }

        public DataException(string action, Result result, string message) : this(action, result, message, null) 
        { 
        }
        public DataException(string action, Result result, string message, Exception innerException) : base(message, innerException)
        {
            Type = typeof(T).Name;
            Action = action;
            Result = result;
        }

    }

}
