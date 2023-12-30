using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EdgeDB.Examples.ExampleTODOApi
{
    [EdgeDBType]
    public class TODOModel
    {
        [JsonPropertyName("title")]
        [EdgeDBProperty("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        [EdgeDBProperty("description")]
        public string? Description { get; set; }

        [JsonPropertyName("date_created")]
        [EdgeDBProperty("date_created")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonPropertyName("state")]
        [EdgeDBProperty("state")]
        public TODOState State { get; set; }
    }

    public enum TODOState
    {
        // [EdgeDBValue("NotStarted")]
        NotStarted,

        // [EdgeDBValue("InProgress")]
        InProgress,

        // [EdgeDBValue("Complete")]
        Complete
    }
}
