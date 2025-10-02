using OthelloPersonal.Backend.Enums;


namespace OthelloPersonal.Backend.EventArg;
public class GameOverEventArgs : EventArgs
{
    public Piece? PieceColor { get; set; }

    public GameOverEventArgs(Piece? winning)
    {
        PieceColor = winning;
    }
}