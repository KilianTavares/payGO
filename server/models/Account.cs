namespace server.models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
    }
    public class BankAccount
    {
        public int Id { get; set; }
        public required string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string? PasswordHash { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}