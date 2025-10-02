namespace OthelloPersonal.Backend.Models;

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
