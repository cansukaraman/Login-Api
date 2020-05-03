using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoginApp.Models
{
    public class User : BaseModel
    {
        public string phone { get; set; }
        [JsonIgnore]
        public string password { get; set; }
        public string token { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        [JsonIgnore]
        public string activationCode { get; set; }
        public DateTime created { get; set; }

        public new string title { get { return firstname + " " + lastname; } }
    }

}
