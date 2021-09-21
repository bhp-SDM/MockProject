using MockProject.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockProject.Core.Services
{
    public class BankAccountManager
    {
        private IRepository<int, IBankAccount> accounts;

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

            accounts.Add(acc);
        }
    }
}
