using System.ComponentModel.DataAnnotations;

namespace CrossNull.Data
{
    class GameStateDb
    {
        [Key]
        public int Id { get; set; }
        public string Game { get; set; }
    }
}
