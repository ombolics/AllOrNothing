using Microsoft.EntityFrameworkCore;
using AllOrNothing.Data;


namespace AllOrNothing.Repository
{
    public class AllOrNothingDbContext : DbContext
    {
        public AllOrNothingDbContext()
            : base()
        {

        }
        public DbSet<QuestionSerie> QuestionSeries { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AllOrNothingDb;Trusted_Connection=True;");
            }
        }
    }
}