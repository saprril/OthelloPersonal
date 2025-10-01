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

    
    public enum Directions
    {
        NorthWest,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West
    }


    public enum GameState
    {
        Initializing = 0,
        PlayerTurn = 1,
        MoveMade = 2,
        GameOver = 3
    }
}