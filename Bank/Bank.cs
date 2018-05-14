using System;
using System.Collections.Generic;

namespace Bank
{
    public class Bank
    {
        public string BankName { get; }
        public List<Account> Accounts { get; }

        public Bank() {
             BankName = "Simple Bank";
        }
    }
}
