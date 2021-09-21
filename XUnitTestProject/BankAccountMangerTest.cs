using MockProject.Core.Interfaces;
using MockProject.Core.Model;
using MockProject.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class BankAccountMangerTest
    {
        // Fake store for repository
        private Dictionary<int, IBankAccount> dataStore;

        private Mock<IRepository<int, IBankAccount>> repoMock;

        public BankAccountMangerTest()
        {
            // Fake data store for the repository mock object
            dataStore = new Dictionary<int, IBankAccount>();

            // setting up the mock
            repoMock = new Mock<IRepository<int, IBankAccount>>();
            
            // prepare properties to contain values
            repoMock.SetupAllProperties();

            // redirect methods of the mock to use the fake data store
            repoMock.SetupGet(x => x.Count).Returns(dataStore.Count);
            
            repoMock.Setup(x => x.Add(It.IsAny<IBankAccount>())).Callback<IBankAccount>((acc) =>
                dataStore.Add(acc.AccountNumber, acc));
            
            repoMock.Setup(x => x.Remove(It.IsAny<IBankAccount>())).Callback<IBankAccount>((acc) =>
                dataStore.Remove(acc.AccountNumber));
            
            repoMock.Setup(x => x.GetByID(It.IsAny<int>())).Returns<int>((accNum) => 
                dataStore.ContainsKey(accNum) ? dataStore[accNum] : null);
            
            // there was a bug here - fixed now
            repoMock.Setup(x => x.GetAll()).Returns(() => new List<IBankAccount>(dataStore.Values));
        }

        [Fact]
        public void CreateBankAccountManager()
        {
            IRepository<int, IBankAccount> repo = repoMock.Object;

            BankAccountManager bam = new BankAccountManager(repo);

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
            Assert.Equal(acc, dataStore[1]);

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
    }
}
