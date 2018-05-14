using System;
using System.Collections.Generic;

namespace Bank
{
    public enum AccountType {
        CHECKING,
        CORPORATE_INVESTMENT,
        INDIVIDUAL_INVESTMENT
    }

    public class Account
    {
        public string Id { get; }
        public Owner Owner { get; }
        public AccountType Type { get; }
        public Double Balance { get; }

        public Account(string id, AccountType type, Double balance, Owner owner) {
            this.Id = id;
            this.Type = type;

            if (balance < 0.0) {
                this.Balance = 0.0;
            } else {
                this.Balance = balance;
            }

            this.Owner = owner;
        }
    }
}
