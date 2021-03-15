using System.ComponentModel.DataAnnotations;

namespace CrossNull.Data
{
    class GameScoreDb
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountWin { get; set; }

    }
}
