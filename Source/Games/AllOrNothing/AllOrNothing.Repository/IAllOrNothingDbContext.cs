
using AllOrNothing.Data;
using Microsoft.EntityFrameworkCore;

namespace AllOrNothing.Repository
{
    public interface IAllOrNothingDbContext
    {
        DbSet<QuestionSerie> QuestionSeries { get; set; }
        DbSet<Competence> Competences { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Team> Teams { get; set; }
    }
}
