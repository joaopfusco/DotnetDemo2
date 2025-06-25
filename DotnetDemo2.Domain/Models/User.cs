using System.Text.Json.Serialization;

namespace DotnetDemo2.Domain.Models
{
    public class User : BaseModel
    {
        private string _keycloakId;

        [JsonIgnore]
        public string KeycloakId
        {
            get => _keycloakId;
            private set { /* ignore external sets */ }
        }

        public string Username { get; set; }
        public string Email { get; set; }

        public void SetKeycloakId(string keycloakId)
        {
            _keycloakId = keycloakId;
        }
    }
}
