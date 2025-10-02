using OthelloPersonal.Backend.Models;
using OthelloPersonal.Backend.Enums;

namespace OthelloPersonal.Backend.EventArg;

public class MoveMadeEventArgs : EventArgs
{
    public Dictionary<Directions, Position> positionDictionary;
    public Position targetPosition;
    public Piece color;

    public MoveMadeEventArgs(Dictionary<Directions, Position> posDict, Position targetPos, Piece color)
    {
        positionDictionary = posDict;
        targetPosition = targetPos;
        this.color = color;
    }
}
