using JwtAuthenticationManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "awdkKAaSDsfaDFsdfSawrDKASDsuEgFKAsdaDoAsdFWwAkd129fASDKasfASL";

        private const int JWT_TOKEN_VALIDITY_MINS = 20;
        private readonly List<UserAccount> _userAccount;

        public JwtTokenHandler() 
        {
            _userAccount = new List<UserAccount> {
                new UserAccount{UserName = "admin", Password = "admin", Role = "Administrator" },
                new UserAccount{UserName = "user", Password = "user", Role = "User" }
            };
        }
    }
}
