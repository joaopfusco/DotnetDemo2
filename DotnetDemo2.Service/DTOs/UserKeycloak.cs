using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetDemo2.Service.DTOs
{
    public class UserKeycloak
    {
        public required string KeycloakId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}
