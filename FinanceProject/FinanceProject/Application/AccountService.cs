using FinanceProject.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceProject.Application
{
    class AccountService : DatabaseConfig
    {
        public Account Login(string username, string password)
        {
            Connection.Open();
            Command.CommandText = $"SELECT * FROM account WHERE username = '{username}' AND password = '{password}'";
            DataReader = Command.ExecuteReader();
            DataReader.Read();
            if (DataReader.HasRows)
            {
                Account account = new Account();
                account.Id = DataReader.GetGuid(0);
                account.Username = DataReader.GetString(1);
                account.Password = DataReader.GetString(2);
                Connection.Close();
                return account;
            }
            else
            {
                Connection.Close();
                return null;
            }
        }
    }
}
