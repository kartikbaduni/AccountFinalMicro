using AccountMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroservice
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> context) : base(context)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Statement> Statements { get; set; }
    }
}
