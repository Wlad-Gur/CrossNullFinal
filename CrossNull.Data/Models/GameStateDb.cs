using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossNull.Data
{
    class GameStateDb
    {
        [Key]
        public int Id { get; set; }
        public string Game { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
