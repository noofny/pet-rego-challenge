using System;
using System.Collections.Generic;

namespace PetRego.Models
{
    public class MultiResponse : Response
    {
        public List<IModel> Values { get; set; }

        public MultiResponse(List<IModel> values, Result result) : base(result)
        {
            Values = values;
        }
        public MultiResponse(string error, Result result) : base(error, result)
        {
        }
    }
}
