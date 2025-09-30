using System.Numerics;

namespace OthelloPersonal
{
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
    public interface ISquare
    {
        public Position SquarePosition { get; set; }
        public Piece SquarePiece { get; set; }
        public int Value { get; set; }

    }

    public struct Position
    {
        public int Row;
        public int Column;
        public static Position operator +(Position pos1, Position pos2)
        {
            return new Position { Row = pos1.Row + pos2.Row, Column = pos1.Column + pos2.Column };
        }
        public static Position operator -(Position pos1, Position pos2)
        {
            return new Position { Row = pos1.Row - pos2.Row, Column = pos1.Column - pos2.Column };
        }
        public override string ToString()
        {
            return $"({Column}, {Row})";
        }
    }

    public enum Piece
    {
        Empty = 0,
        Black = 1,
        White = 2
    }
}