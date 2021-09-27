using MockProject.Core.Interfaces;
using System;

namespace MockProject.Core.Services
{
    public class BankAccountManager
    {
        private IRepository<int, IBankAccount> accounts;

        public int Count 
        { 
            get
            {
                return accounts.Count;
            }
        }
        public BankAccountManager(IRepository<int, IBankAccount> repo)
        {
            this.accounts = repo ?? throw new ArgumentException("Missing BankAccount Repository");
        }

        public void AddBankAccount(IBankAccount acc)
        {
            if (acc == null)
            {
                throw new ArgumentException("Bank account cannot be null");
            }
            if (accounts.GetByID(acc.AccountNumber) != null)
            { 
                throw new ArgumentException("Bank Account already exist");
            }
            accounts.Add(acc);
        }
    }
}
