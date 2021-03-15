using CrossNull.Domain;
using CrossNull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public GameState State { get; private set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player PlayerActive { get; set; }
    }
}
