﻿using AllOrNothing.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        IQuestionSerieRepository QuestionSeries { get; }
        int Complete();
    }
}
