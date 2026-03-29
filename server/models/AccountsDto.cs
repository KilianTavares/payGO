namespace server.models;

public class AccountDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string AccountType { get; set; }
    public decimal Balance { get; set; }
}