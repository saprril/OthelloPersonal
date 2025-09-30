using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace OthelloPersonal
{
    public class GameBoard : IGameBoard
    {
        public int Size { get; set; }
        public ISquare[,] Squares { get; set; }

        public GameBoard(int size)
        {
            Squares = new ISquare[size, size];
            Size = size;
        }
    }


    public interface IGameBoard
    {
        public int Size { get; set; }
        public ISquare[,] Squares { get; set; }
    }
}