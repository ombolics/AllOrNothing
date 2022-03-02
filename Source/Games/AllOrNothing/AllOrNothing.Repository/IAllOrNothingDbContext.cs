
using AllOrNothing.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
