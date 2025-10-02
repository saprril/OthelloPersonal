using OthelloPersonal.Backend.Interfaces;
using OthelloPersonal.Backend.Enums;

namespace OthelloPersonal.Backend.Models;

public class Player : IPlayer
{
    public string Name { get; set; }
    public Piece PlayerPiece { get; set; }

    public Player(string name, Piece piece)
    {
        Name = name;
        PlayerPiece = piece;
    }
}