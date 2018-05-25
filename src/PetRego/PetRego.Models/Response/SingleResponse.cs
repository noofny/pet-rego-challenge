using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class SingleResponse : Response
    {
        public IModel Value { get; private set; }

        public SingleResponse(IModel value, Result result) : base(result)
        { 
            Value = value;
        }
        public SingleResponse(string error, Result result) : base(error, result)
        {
        }
    }
}
