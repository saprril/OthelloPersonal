using OthelloPersonal.Backend.Enums;
using OthelloPersonal.Backend.Models;

namespace OthelloPersonal.Backend.Interfaces;

public interface ISquare
{
    public Position SquarePosition { get; set; }
    public Piece SquarePiece { get; set; }
    public int Value { get; set; }

}

