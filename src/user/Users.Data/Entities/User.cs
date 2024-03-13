﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Data.Entities
{
    public class User
    {
        public Guid id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public int Active { get; set; }

        public Role Role { get; set; }
    }
}
