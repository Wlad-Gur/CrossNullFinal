using CrossNull.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Models
{
    internal static class GameFild
    {
        static Dictionary<ConsoleKey, Func<PlayerTypes, Cell>> _fild =
            new Dictionary<ConsoleKey, Func<PlayerTypes, Cell>>() {
            { ConsoleKey.NumPad1, (pl) => new Cell((CellStates) (int) pl, 0, 0) },
            { ConsoleKey.NumPad2, (pl) => new Cell((CellStates) (int) pl, 0, 1) },
            { ConsoleKey.NumPad3, (pl) => new Cell((CellStates) (int) pl, 0, 2) },
            { ConsoleKey.NumPad4, (pl) => new Cell((CellStates) (int) pl, 1, 0) },
            { ConsoleKey.NumPad5, (pl) => new Cell((CellStates) (int) pl, 1, 1) },
            { ConsoleKey.NumPad6, (pl) => new Cell((CellStates) (int) pl, 1, 2) },
            { ConsoleKey.NumPad7, (pl) => new Cell((CellStates) (int) pl, 2, 0) },
            { ConsoleKey.NumPad8, (pl) => new Cell((CellStates) (int) pl, 2, 1) },
            { ConsoleKey.NumPad9, (pl) => new Cell((CellStates) (int) pl, 2, 2) },
            };

public static Dictionary<ConsoleKey, Func<PlayerTypes, Cell>> State => _fild;
public static string ToStringStatic()
{
    return (" 1 | 2 | 3 " +
         $"{Environment.NewLine} ------------ {Environment.NewLine}" +
         " 4 | 5 | 6" +
         $" {Environment.NewLine} ------------{Environment.NewLine}" +
         $" 7 | 8 | 9 {Environment.NewLine}");
}
    }
}
