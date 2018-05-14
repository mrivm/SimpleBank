using System;
using System.Collections;
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
            Hashtable accounts = _bank.Accounts;

            Assert.True(accounts.Count > 0, "Bank should have an account list");
        }

        [Fact]
        public void AccountCreationRequiresAccountID() {
            string result = _bank.AddAccount("", AccountType.CHECKING, 500.00, new Owner("John Doe"));

            Assert.True(result == null, "Account requires an ID specified");
        }

        [Fact]
        public void AccountCreationDisallowsDuplicateID() {
            string result = _bank.AddAccount("1D1", AccountType.CHECKING, 500.00, new Owner("John Doe"));

            Assert.True(result != null, "Account is created successfully");

            result = _bank.AddAccount("1D1", AccountType.CHECKING, 500.00, new Owner("John Doe"));

            Assert.True(result == null, "Account already exists");
        }

        [Fact]
        public void AccountCreationRequiresPositiveBalance() {
            string result = _bank.AddAccount("1D1", AccountType.CHECKING, -500.00, new Owner("John Doe"));

            Assert.True(result == null, "Account balance must be zero or greater");
        }

        [Fact]
        public void AccountCreationRequiresAnOwner() {
            string result = _bank.AddAccount("1D1", AccountType.CHECKING, 500.00, null);

            Assert.True(result == null, "Account requires an owner specified");
        }

        [Fact]
        public void AccountTransferRequiresAccountIDs() {
            bool result = _bank.AccountTransfer("1D1", "", 0.0);
            Assert.False(result, "Two ID's were not specified");

            result = _bank.AccountTransfer("", "1D2", 0.0);
            Assert.False(result, "Two ID's were not specified");

            result = _bank.AccountTransfer("", "", 0.0);
            Assert.False(result, "Two ID's were not specified");
        }

        [Fact]
        public void AccountsOnTransferMustBeDifferent() {
            bool result = _bank.AccountTransfer("1D1", "1D1", 0.0);
            Assert.False(result, "Same ID specified on both parameters");
        }

        [Fact]
        public void AccountsOnTransferMustExist() {
            bool result = _bank.AccountTransfer("1D1", "1D2", 500.00);
            Assert.False(result, "Accounts don't exist");
        }

        [Fact]
        public void AccountTransferBalanceMustBePositive() {
            bool result = _bank.AccountTransfer("1D1", "1D2", -500.00);
            Assert.False(result, "Balance must be positive");
        }
    }
}
