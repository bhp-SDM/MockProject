using MockProject.Core.Interfaces;
using MockProject.Core.Model;
using MockProject.Core.Services;
using Moq;
using System;
using Xunit;

namespace XUnitTestProject
{
    public class BankAccountManagerTest_BehaviorBased
    {
        private Mock<IRepository<int, IBankAccount>> repoMock;

        public BankAccountManagerTest_BehaviorBased()
        {
            // setting up the mock
            repoMock = new Mock<IRepository<int, IBankAccount>>();            
        }

        [Fact]
        public void CreateBankAccountManager()
        {
            IRepository<int, IBankAccount> repo = repoMock.Object;

            BankAccountManager bam = null;
            
            bam = new BankAccountManager(repo);

            Assert.NotNull(bam);
            Assert.True(bam is BankAccountManager);
            Assert.Equal(0, bam.Count);
        }

        [Fact]
        public void CreateBankAccountManagerMissingRepositoryExpectArgumentException()
        {
            BankAccountManager bam = null;

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() => bam = new BankAccountManager(null));

            Assert.Null(bam);
            Assert.Equal("Missing BankAccount Repository", ex.Message);
        }

        [Fact]
        public void AddNonExistingBankAccount()
        {

            IBankAccount acc = new BankAccount(1);

            // account with account number = 1 does not exist in the repository
            repoMock.Setup(x => x.GetByID(1)).Returns(() => null);
            
            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = new BankAccountManager(repo);

            // act
            bam.AddBankAccount(acc);

            repoMock.Verify(repo => repo.Add(acc), Times.Once);
        }

        [Fact]
        public void AddBankAccountIsNullExpectArgumentException()
        {
            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = new BankAccountManager(repo);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() => bam.AddBankAccount(null));

            Assert.Equal("Bank account cannot be null", ex.Message);
            repoMock.Verify(repo => repo.Add(null), Times.Never);
        }

        [Fact]
        public void AddBankAccountAlreadyExistsExpectArgumentException()
        {
            IBankAccount acc = new BankAccount(1);
            
            // Bank account with account number = 1 already exists in the repository
            repoMock.Setup(x => x.GetByID(1)).Returns(() => acc);
            
            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = new BankAccountManager(repo);

            var ex = Assert.Throws<ArgumentException>(() => bam.AddBankAccount(acc));

            Assert.Equal("Bank Account already exist", ex.Message);            
            repoMock.Verify(repo => repo.Add(acc), Times.Never);
        }
    }
}
