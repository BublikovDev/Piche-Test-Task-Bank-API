using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class Account
    {
        [Key]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = null!;
        [Required]
        public string Owner { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
