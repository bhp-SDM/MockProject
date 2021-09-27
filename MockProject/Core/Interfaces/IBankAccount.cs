namespace MockProject.Core.Interfaces
{
    public interface IBankAccount
    {
        int AccountNumber { get; set; }
        double Balance { get; set; }
        double InterestRate { get; set; }
    }
}
