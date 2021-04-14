using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Models
{
    public enum PlayerTypes
    {
        X,
        O
    }
    public class Player
    {
        public Player()
        {

        }

        public Player(PlayerTypes playerType, string name)
        {
            PlayerType = playerType;
            Name = name;
        }
        public string Name { get; set; }
        public PlayerTypes PlayerType { get; }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (Object.ReferenceEquals(obj, this))
            {
                return true;
            }

            Player playerAct = obj as Player;
            if (playerAct == null)
            {
                return false;
            }
            return (playerAct.Name == this.Name && playerAct.PlayerType == this.PlayerType);
        }
    }
}
