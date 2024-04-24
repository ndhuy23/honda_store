using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Data.Dto.Product
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }
    }
}
