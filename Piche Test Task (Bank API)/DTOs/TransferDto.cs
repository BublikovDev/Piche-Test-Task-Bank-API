namespace Server.DTOs
{
    public record TransferDto(string FromAccount, string ToAccount, decimal Amount);
}
