namespace Server.DTOs
{
    public record CreateAccountDto(string AccountNumber, string Owner, decimal InitialBalance);
}
