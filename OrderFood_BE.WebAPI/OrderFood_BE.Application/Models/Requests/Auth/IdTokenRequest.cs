using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Auth
{
    public class IdTokenRequest
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }
    }
}
