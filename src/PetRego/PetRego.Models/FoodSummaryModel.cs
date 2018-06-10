using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetRego.Models
{

    public class FoodSummaryModel : IModel
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<FoodCount> FoodCounts { get; set; }

        public FoodSummaryModel()
        {
            FoodCounts = new List<FoodCount>();
        }
    }
}
