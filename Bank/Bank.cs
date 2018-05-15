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
            if (from.Length == 0 || to.Length == 0) {
                System.Console.WriteLine("Account ID's are required");
            } else if (amount <= 0.0) {
                System.Console.WriteLine("Amount must be positive and greater than zero");
            } else if (from == to) {
                System.Console.WriteLine("Accounts must be different");
            } else if (!Accounts.ContainsKey(from) || !Accounts.ContainsKey(to)) {
                System.Console.WriteLine("Accounts must exist");
            } else {
                Account srcAccount = (Account) this.Accounts[from];
                if (!srcAccount.Withdraw(amount)) {
                    System.Console.WriteLine("Withdrawal amount greater than account balance");
                } else {
                    Account destAccount = (Account) this.Accounts[to];
                    destAccount.Deposit(amount);
                    Accounts[from] = srcAccount;
                    Accounts[to] = destAccount;
                    return true;
                }
            }

            return false;
        }

        public bool Deposit(string id, Double amount) {
            if (id.Length == 0) {
                System.Console.WriteLine("Account ID is required");
            } else if (amount <= 0.0) {
                System.Console.WriteLine("Amount must be positive and greater than zero");
            } else if (!Accounts.ContainsKey(id)) {
                System.Console.WriteLine("Account must exist");
            } else {
                Account destAccount = (Account) this.Accounts[id];
                destAccount.Deposit(amount);
                Accounts[id] = destAccount;
                return true;
            }

            return false;
        }

        public bool Withdraw(string id, Double amount) {
            if (id.Length == 0) {
                System.Console.WriteLine("Account ID is required");
            } else if (amount <= 0.0) {
                System.Console.WriteLine("Amount must be positive and greater than zero");
            } else if (!Accounts.ContainsKey(id)) {
                System.Console.WriteLine("Account must exist");
            } else {
                Account destAccount = (Account) this.Accounts[id];
                if (!destAccount.Withdraw(amount)) {
                    System.Console.WriteLine("Withdrawal amount greater than account balance");
                } else {
                    Accounts[id] = destAccount;
                    return true;
                }
            }

            return false;
        }
        
        public Double GetAccountBalance(string id) {
            if (id.Length == 0) {
                System.Console.WriteLine("Account ID is required");
            } else if (!Accounts.ContainsKey(id)) {
                System.Console.WriteLine("Account must exist");
            } else {
                Account acct = (Account) Accounts[id];
                return acct.Balance;
            }

            return -1.0;
        }
    }
}
