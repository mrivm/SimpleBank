using System;
using System.Collections;
using Xunit;

namespace Bank.Tests
{
    public class BankTests
    {
        public readonly Bank _bank;
        public readonly Owner _bankOwner;
        
        public BankTests() {
            _bank = new Bank();
            _bankOwner = new Owner("SimpleBank");

            _bank.AddAccount("1", AccountType.CHECKING, 5000.00, _bankOwner);
            _bank.AddAccount("2", AccountType.CORPORATE_INVESTMENT, 5000.00, _bankOwner);
            _bank.AddAccount("3", AccountType.INDIVIDUAL_INVESTMENT, 5000.00, _bankOwner);
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
            bool result = _bank.AccountTransfer("1D1", "1D1", 500.0);
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

        [Fact]
        public void AccountTransferMustNotOverdraftAccount() {
            bool result = _bank.AccountTransfer("1", "2", 50000.00);
            Assert.False(result, "Amount must not overdraft account");
        }

        [Fact]
        public void IndividualInvestmentAccountTransferMustNotBeGreaterThanOneThousand() {
            bool result = _bank.AccountTransfer("3", "2", 2000.00);
            Assert.False(result, "Cannot transfer more than $1,000 at a time");
        }

        [Fact]
        public void AccountWithdrawalAmountMustBePositive() {
            bool result = _bank.Withdraw("1", -500.00);
            Assert.False(result, "Amount must be positive");
        }

        [Fact]
        public void AccountWithdrawalAmountMustNotOverdraftAccount() {
            bool result = _bank.Withdraw("1", 50000.00);
            Assert.False(result, "Amount must not overdraft account");
        }

        [Fact]
        public void AccountDepositAmountMustBePositive() {
            bool result = _bank.Withdraw("1", -500.00);
            Assert.False(result, "Amount must be positive");
        }

        [Fact]
        public void IndividualInvestmentWithdrawalMustNotBeGreaterThanOneThousand() {
            bool result = _bank.Withdraw("3", 2000.00);
            Assert.False(result, "Cannot withdraw more than $1,000 at a time");
        }

        [Fact]
        public void GetAccountBalanceValidatesAccountID() {
            Double result = _bank.GetAccountBalance("");
            Assert.False(result > 0.0, "An Account ID must be specified");
        }

        [Fact]
        public void GetAccountBalanceValidatesAccountExists() {
            Double result = _bank.GetAccountBalance("NOTEXISTS123");
            Assert.False(result > 0.0, "Cannot report balance of inexsisting account");
        }

        [Theory]
        [InlineData("1D31", 3000.00, 50.00, 3050.00)]
        [InlineData("1D32", 2000.00, 1000.00, 3000.00)]
        [InlineData("1D33", 1000.00, 500.00, 1500.00)]
        public void TestBalanceUpdateAfterDeposit(string id, Double destAcctBalance, Double depositAmount, Double updatedBalance) {
            _bank.AddAccount(id, AccountType.CHECKING, destAcctBalance, _bankOwner);
            Double reportedBalance = _bank.GetAccountBalance(id);
            Assert.True(reportedBalance == destAcctBalance, "Reported amount must be the initial amount");

            _bank.Deposit(id, depositAmount);
            reportedBalance = _bank.GetAccountBalance(id);
            Assert.True(reportedBalance == updatedBalance, "Reported amount must be the initial amount plus the deposit amount");
        }

        [Theory]
        [InlineData("1D41", 3000.00, 50.00, 2950.00)]
        [InlineData("1D42", 2000.00, 1000.00, 1000.00)]
        [InlineData("1D43", 1000.00, 500.00, 500.00)]
        public void TestBalanceUpdateAfterWithdrawal(string id, Double destAcctBalance, Double withdrawalAmount, Double updatedBalance) {
            _bank.AddAccount(id, AccountType.CHECKING, destAcctBalance, _bankOwner);
            Double reportedBalance = _bank.GetAccountBalance(id);
            Assert.True(reportedBalance == destAcctBalance, "Reported amount must be the initial amount");

            _bank.Withdraw(id, withdrawalAmount);
            reportedBalance = _bank.GetAccountBalance(id);
            Assert.True(reportedBalance == updatedBalance, "Reported amount must be the initial amount minus the deposit amount");
        }

        [Theory]
        [InlineData("1D51", "1D61", 3000.00, 3000.00, 50.00, 2950.00, 3050.00)]
        [InlineData("1D52", "1D62", 2000.00, 2000.00, 1000.00, 1000.00, 3000.00)]
        [InlineData("1D53", "1D63", 1000.00, 1000.00, 500.00, 500.00, 1500.00)]
        public void TestBalanceUpdateAfterTransfer(string srcId, string destId, Double srcAcctBalance, Double destAcctBalance, Double transferAmount, Double srcUpdatedBalance, Double destUpdatedBalance) {
            _bank.AddAccount(srcId, AccountType.CHECKING, srcAcctBalance, _bankOwner);
            Double reportedBalance = _bank.GetAccountBalance(srcId);
            Assert.True(reportedBalance == srcAcctBalance, "Reported amount must be the initial amount");

            _bank.AddAccount(destId, AccountType.CHECKING, destAcctBalance, _bankOwner);
            reportedBalance = _bank.GetAccountBalance(destId);
            Assert.True(reportedBalance == destAcctBalance, "Reported amount must be the initial amount");

            _bank.AccountTransfer(srcId, destId, transferAmount);
            
            Double srcReportedBalance = _bank.GetAccountBalance(srcId);
            Double destReportedBalance = _bank.GetAccountBalance(destId);

            Assert.True(srcReportedBalance == srcUpdatedBalance, "Reported amount must be the initial amount minus the transferred amount");
            Assert.True(destReportedBalance == destUpdatedBalance, "Reported amount must be the initial amount plus the transferred amount");
        }
    }
}
