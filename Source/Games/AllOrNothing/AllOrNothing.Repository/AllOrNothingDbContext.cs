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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

                //TODO kivenni az eszköznév specifikus dolgokat a connection stringből DESKTOP-B5C457P\SQLEXPRESS
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLOCALDB;Integrated Security=true;Database=AllOrNothingDb;");
            }

        }
    }
}