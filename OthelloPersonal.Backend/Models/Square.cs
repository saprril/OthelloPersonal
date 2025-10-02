using OthelloPersonal.Backend.Interfaces;
using OthelloPersonal.Backend.Enums;

namespace OthelloPersonal.Backend.Models;

public class Square : ISquare
{
    public Position SquarePosition { get; set; }
    public Piece SquarePiece { get; set; }
    public int Value { get; set; }
    public Square(Position squarePos, Piece squarePiece, int val)
    {
        SquarePosition = squarePos;
        SquarePiece = squarePiece;
        Value = val;
    }
}