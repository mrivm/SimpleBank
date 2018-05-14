using System;
using System.Collections.Generic;
using Xunit;

namespace Bank.Tests
{
    public class BankTests
    {
        public readonly Bank _bank;
        
        public BankTests() {
            _bank = new Bank();
        }

        [Fact]
        public void BankNameIsDefined()
        {
            string result = _bank.BankName;

            Assert.True(result.Length > 0, "Bank name should have alphanumeric contents");
        }

        [Fact]
        public void BankHasAccounts()
        {
            List<Account> accounts = _bank.Accounts;

            Assert.True(accounts.Count > 0, "Bank should have an account list");
        }
    }
}
