﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UoW : IUoW
    {
        private readonly TournamentApiContext _context;
        public ITournamentRepository  TournamentRepository { get; }
        public IGameRepository GameRepository { get; }


        public UoW(TournamentApiContext context)
        {
           _context = context;
           TournamentRepository=new TournamentRepository(_context);
           GameRepository = new  GameRepository(_context);
        }
       
       

        public async  Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
