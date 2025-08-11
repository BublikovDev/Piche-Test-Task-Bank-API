using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = null!;
        public Account Account { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Required]
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
