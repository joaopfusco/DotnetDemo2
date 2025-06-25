using DotnetDemo2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetDemo2.Service.DTOs
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required User User { get; set; }
    }
}
