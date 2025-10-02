using OthelloPersonal.Backend.Interfaces;

namespace OthelloPersonal.Backend.Models;

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