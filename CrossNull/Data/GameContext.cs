using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace CrossNull.Data
{
    class GameContext : DbContext
    {
        public GameContext() : base("DbConnection22")
        {

        }
        public DbSet<GameScoreDb> GameScores { get; set; }
        public DbSet<GameStateDb> Games { get; set; }
    }

    class GameScoreDb
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountWin { get; set; }

    }

    class GameStateDb
    {
        public int Id { get; set; }
        public string Game { get; set; }
    }
}
