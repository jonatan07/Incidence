using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncidenceDionny.Models
{
    public class Incidence
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
