using System;
using System.Collections;

namespace Bank
{
    public class Bank
    {
        public string BankName { get; }
        public Hashtable Accounts { get; }

        public Bank() {
             BankName = "Simple Bank";
             Accounts = new Hashtable();
        }

    }
}
