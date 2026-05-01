namespace server.models;

public class AccountRegistrationDTO
{
    public int Id { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}

public class AccountLoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class BankAccountDTO
{
    public int Id { get; set; }
    public required string AccountType { get; set; }
    public decimal Balance { get; set; }
}

public class AccountUpdateDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required string AccountType { get; set; }
}

public class AccountPasswordUpdateDTO
{
    public int Id { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}

