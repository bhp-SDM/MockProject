using MockProject.Core.Interfaces;
using System;

namespace MockProject.Core.Model
{
    public class BankAccount : IBankAccount
    {
        public const double DEFAULT_INTERESTRATE = 0.01;
        private double _interestRate; 
        public int AccountNumber { get; set; }
        public double Balance { get; set; }
        public double InterestRate { 
            get
            { 
                return _interestRate;
            } 
            set
            { 
                if (value < 0.00 || value > 0.10)
                    throw new ArgumentException("Interest Rate must be between [0.00 - 0.10]");
                _interestRate = value;
            } 
        }

        public BankAccount(int accNumber, double initialBalance, double interestRate)
        { 
            if (accNumber <= 0)
                throw new ArgumentException("Account number must be positive");
            if (initialBalance < 0.0)
                throw new ArgumentException("Initial balance must be a positive amount");
            AccountNumber = accNumber;
            Balance = initialBalance;
            InterestRate = interestRate;
        }

        public BankAccount(int accNumber, double initialBalance)
            : this(accNumber, initialBalance, DEFAULT_INTERESTRATE)
        {
        }

        public BankAccount(int accNumber)
            : this(accNumber, 0.0, DEFAULT_INTERESTRATE)
        {
        }

        public BankAccount()
        {
        }
    }
}
