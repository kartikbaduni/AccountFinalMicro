using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountMicroservice.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime AccountCreationDate { get; set; }
        [Required]
        public string AccountType { get; set; }
        [Required]
        public double CurrentBalance { get; set; }

    }
}
