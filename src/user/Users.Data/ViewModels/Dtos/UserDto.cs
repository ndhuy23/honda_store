using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Data.ViewModels.Dtos
{
    public class RegisterUserDto
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
