using System;
using System.ComponentModel.DataAnnotations;

namespace AccountMicroservice.Models
{
    public class Statement
    {
        [Key]
        public int TransactionID { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Deposit { get; set; }
        public double Withdrawal { get; set; }
        [Required]
        public double ClosingBalance { get; set; }
    }
}
