namespace server.models.responses
{
    public class GetAccountResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}