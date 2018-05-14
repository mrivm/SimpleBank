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

            Owner bankOwner = new Owner("SimpleBank");

            // Dummy bank-owned accounts
            AddAccount("1", AccountType.CHECKING, 5000.00, bankOwner);
            AddAccount("2", AccountType.CORPORATE_INVESTMENT, 5000.00, bankOwner);
            AddAccount("3", AccountType.INDIVIDUAL_INVESTMENT, 5000.00, bankOwner);
        }

        public string AddAccount(string id, AccountType type, Double balance, Owner owner) {
            if (id.Length == 0) {
                System.Console.WriteLine("ID cannot be empty");
            }
            else if (balance < 0.0) {
                System.Console.WriteLine("Balance cannot be negative");
            }
            else if (owner == null) {
                System.Console.WriteLine("An Owner must be specified for the account");
            } else if (Accounts.Contains(id)) {
                System.Console.WriteLine("The account already exists");
            } else {
                Account acct = new Account(id, type, balance, owner);
                Accounts.Add(id, acct);
                return id;
            }

            return null;
        }

        public bool AccountTransfer(string from, string to, Double amount) {
            return true;
        }
    }
}
