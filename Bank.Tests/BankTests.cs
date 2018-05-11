using System;
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
            string result = _bank.GetBankName();

            Assert.True(result.Length > 0, "Bank name should have alphanumeric contents");
        }
    }
}
