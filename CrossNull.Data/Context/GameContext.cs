using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CrossNull.Data
{
    class GameContext : IdentityDbContext
    {
        public GameContext() : this("DbConnection22")
        {
#if DEBUG
            Database.Log = l => Debug.WriteLine(l);
#endif
        }
        public GameContext(string connectionString) : base(connectionString)
        {

        }

        public virtual DbSet<GameScoreDb> GameScores { get; set; }
        public virtual DbSet<GameStateDb> Games { get; set; }
    }
}
