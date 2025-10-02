using OthelloPersonal.Backend.Enums;
using OthelloPersonal.Backend.Models;

namespace OthelloPersonal.Backend.Interfaces;


public interface IGameBoard
{
    public int Size { get; set; }
    public ISquare[,] Squares { get; set; }
}