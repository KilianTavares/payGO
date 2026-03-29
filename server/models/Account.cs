namespace server.models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AccountType { get; set; }
        public decimal Balance { get; set; }

        public string? Passkey { get; set; }
    }
}