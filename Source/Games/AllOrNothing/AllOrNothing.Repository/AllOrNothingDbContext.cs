using AllOrNothing.Data;
using Microsoft.EntityFrameworkCore;


namespace AllOrNothing.Repository
{
    public class AllOrNothingDbContext : DbContext, IAllOrNothingDbContext
    {
        //The parameterless contstructor ALWAYS should be the first among the constructors
        //This is to avoid the following error from EF Core at runtime:
        //
        // Unable to create an object of type 'AllOrNothingDbContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
        //
        //***************************************************************
        public AllOrNothingDbContext()
            : base()
        {
        }

        public AllOrNothingDbContext(DbContextOptions<AllOrNothingDbContext> options)
            : base(options)
        {
        }
        //*************************************************************

        public DbSet<QuestionSerie> QuestionSeries { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);

            if (!optionsBuilder.IsConfigured)
            {
                var dir = System.AppDomain.CurrentDomain.BaseDirectory;
                optionsBuilder.UseSqlite($@"Data source={dir}\AllOrNothingDb.db");
            }
        }
    }
}