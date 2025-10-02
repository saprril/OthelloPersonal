using OthelloPersonal.Backend.Enums;

namespace OthelloPersonal.Backend.Interfaces;

public interface IPlayer
{
    string Name { get; }
    Piece PlayerPiece { get; }
}
