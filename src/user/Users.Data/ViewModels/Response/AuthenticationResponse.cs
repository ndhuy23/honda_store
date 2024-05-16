using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Data.ViewModels.Response
{
    public class AuthenticationResponse
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string JwtToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
