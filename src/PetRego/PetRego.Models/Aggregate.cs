using System.Collections.Generic;

namespace PetRego.Models
{
    public class EntityCount
    {
        public string GroupedBy { get; set; }
        public Dictionary<string, long> Values { get; set; }

        
        public EntityCount(long count)
        {
            GroupedBy = string.Empty;
            Values = new Dictionary<string, long>
            {
                { string.Empty, count }
            };
        }
        public EntityCount(string groupedBy, Dictionary<string, long> values)
        {
            GroupedBy = groupedBy;
            Values = values;
        }

    }
}
