using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject.Classes
{
    class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } //Password system will be improved
    }
}
