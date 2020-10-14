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

        public Account()
        {
        }

        public Account(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
