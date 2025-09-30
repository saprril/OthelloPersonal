namespace OthelloPersonal
{
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
    public interface IPlayer
    {
        public string Name { get; set; }
        public Piece PlayerPiece { get; set; }
    }
}