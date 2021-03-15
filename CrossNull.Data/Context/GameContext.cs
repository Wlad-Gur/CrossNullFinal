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
        public GameContext() : this("DbConnection22")
        {

        }
        public GameContext(string connectionString) : base(connectionString)
        {

        }

        public virtual DbSet<GameScoreDb> GameScores { get; set; }
        public virtual DbSet<GameStateDb> Games { get; set; }
    }
}
