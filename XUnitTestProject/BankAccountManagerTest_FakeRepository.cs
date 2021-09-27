using MockProject.Core.Interfaces;
using MockProject.Core.Model;
using MockProject.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class BankAccountManagerTest_FakeRepository
    {
        // Fake store for repository
        private Dictionary<int, IBankAccount> dataStore;

        private Mock<IRepository<int, IBankAccount>> repoMock;

        public BankAccountManagerTest_FakeRepository()
        {
            //// Fake data store for the repository mock object
            dataStore = new Dictionary<int, IBankAccount>();

            //// setting up the mock
            repoMock = new Mock<IRepository<int, IBankAccount>>();

            // prepare properties to contain values
            repoMock.SetupAllProperties();

            // redirect methods of the mock to use the fake data store
            repoMock.SetupGet(x => x.Count).Returns(() => dataStore.Count);

            repoMock.Setup(x => x.Add(It.IsAny<IBankAccount>())).Callback<IBankAccount>((acc) =>
                dataStore.Add(acc.AccountNumber, acc));

            repoMock.Setup(x => x.Remove(It.IsAny<IBankAccount>())).Callback<IBankAccount>((acc) =>
                dataStore.Remove(acc.AccountNumber));

            repoMock.Setup(x => x.GetByID(It.IsAny<int>())).Returns<int>((accNum) =>
                dataStore.ContainsKey(accNum) ? dataStore[accNum] : null);

            repoMock.Setup(x => x.GetAll()).Returns(() => new List<IBankAccount>(dataStore.Values));
        }

        [Fact]
        public void CreateBankAccountManager()
        { 
            // arrange
            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = null;
            
            // act
            bam = new BankAccountManager(repo);

            // assert
            Assert.NotNull(bam);
            Assert.True(bam is BankAccountManager);
            Assert.Equal(0, bam.Count);
            Assert.Empty(dataStore);
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

            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = new BankAccountManager(repo);

            // act
            bam.AddBankAccount(acc);

            Assert.True(dataStore.Count == 1);
            Assert.True(dataStore.ContainsKey(acc.AccountNumber));

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

            IRepository<int, IBankAccount> repo = repoMock.Object;
            BankAccountManager bam = new BankAccountManager(repo);

            // dataStore contains acc
            dataStore.Add(acc.AccountNumber, acc);
            int oldCount = dataStore.Count;
            
            var ex = Assert.Throws<ArgumentException>(() => bam.AddBankAccount(acc));

            Assert.Equal("Bank Account already exist", ex.Message);
            Assert.Equal(oldCount, dataStore.Count);

            repoMock.Verify(repo => repo.Add(acc), Times.Never);
        }
    }
}
