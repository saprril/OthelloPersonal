using System.Security.Cryptography.X509Certificates;

namespace OthelloPersonal
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            List<IPlayer> players = new List<IPlayer>();
            Player player1 = new Player("bambang", Piece.Black);
            Player player2 = new Player("abigail", Piece.White);
            players.Add(player1);
            players.Add(player2);

            GameController othelloControl = new GameController(players);
            othelloControl.InitializeBoard();
            othelloControl.MakeMove(othelloControl.GetValidMoves(Piece.Black)[0], Piece.Black);
            othelloControl.DisplayBoard(0);
            */


            bool running = true;
            List<IPlayer> playerList = new List<IPlayer>();
            bool IsTwoPlayer = false;

            int currentPlayerIdx = 0;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Welcome To Othello game!");
                Thread.Sleep(500);
                (playerList, IsTwoPlayer) = AddPlayer();
                Console.Clear();
                // Console.WriteLine(IsTwoPlayer);
                GameController othelloControl = new GameController(playerList);
                othelloControl.InitializeBoard();
                if (IsTwoPlayer)
                {
                    while (!othelloControl.IsGameOver())
                    {

                        List<Position> currentPlayerValidMoves = othelloControl.GetValidMoves(playerList[currentPlayerIdx].PlayerPiece);
                        int moveIterator = 0;
                        ConsoleKey pressedKey;
                        if (currentPlayerValidMoves.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"Current Player: {playerList[currentPlayerIdx].Name} ({playerList[currentPlayerIdx].PlayerPiece})");
                            Console.WriteLine($"No valid moves, press any key to skip your turn");
                            othelloControl.DisplayBoard(0);
                            do
                            {
                                pressedKey = Console.ReadKey(true).Key;
                                othelloControl.MakeMove(new Position { Column = -1, Row = -1 }, playerList[currentPlayerIdx].PlayerPiece);

                            } while (false);
                        }
                        else
                        {
                            do
                            {
                                Console.Clear();
                                Console.WriteLine($"Current Player: {playerList[currentPlayerIdx].Name} ({playerList[currentPlayerIdx].PlayerPiece})");
                                Console.WriteLine($"Selected Position = {moveIterator}: {currentPlayerValidMoves[moveIterator]}");
                                Console.WriteLine("Use arrow key to navigate, Enter to select:");

                                othelloControl.DisplayBoard(moveIterator);
                                pressedKey = Console.ReadKey(true).Key;
                                if (pressedKey == ConsoleKey.RightArrow || pressedKey == ConsoleKey.DownArrow)
                                {
                                    moveIterator = (moveIterator == 0) ? currentPlayerValidMoves.Count - 1 : moveIterator - 1;
                                }
                                else if (pressedKey == ConsoleKey.LeftArrow || pressedKey == ConsoleKey.UpArrow)
                                {
                                    moveIterator = (moveIterator + 1) % currentPlayerValidMoves.Count;
                                }
                            } while (pressedKey != ConsoleKey.Enter);
                            othelloControl.MakeMove(currentPlayerValidMoves[moveIterator], playerList[currentPlayerIdx].PlayerPiece);
                        }
                        currentPlayerIdx = (currentPlayerIdx + 1) % playerList.Count;
                    }
                }
                else
                {
                    while (!othelloControl.IsGameOver())
                    {

                        List<Position> currentPlayerValidMoves = othelloControl.GetValidMoves(playerList[currentPlayerIdx].PlayerPiece);
                        int moveIterator = 0;
                        ConsoleKey pressedKey;
                        do
                        {
                            Console.Clear();
                            othelloControl.DisplayBoard(moveIterator);
                            Console.WriteLine($"Current Player: {playerList[currentPlayerIdx].Name} ({playerList[currentPlayerIdx].PlayerPiece})");
                            Console.WriteLine($"Selected Position (Main Loop) = {moveIterator}");
                            Console.WriteLine("Use ← → key to navigate, Enter to select:");
                            pressedKey = Console.ReadKey(true).Key;
                            if (pressedKey == ConsoleKey.RightArrow || pressedKey == ConsoleKey.DownArrow)
                            {
                                moveIterator = (moveIterator == 0) ? currentPlayerValidMoves.Count - 1 : moveIterator - 1;
                            }
                            else if (pressedKey == ConsoleKey.LeftArrow || pressedKey == ConsoleKey.DownArrow)
                            {
                                moveIterator = (moveIterator + 1) % currentPlayerValidMoves.Count;
                            }
                        } while (pressedKey != ConsoleKey.Enter);
                        othelloControl.MakeMove(currentPlayerValidMoves[moveIterator], playerList[currentPlayerIdx].PlayerPiece);
                        currentPlayerValidMoves = othelloControl.GetValidMoves(playerList[currentPlayerIdx + 1].PlayerPiece);
                        Random rand = new Random();
                        if (currentPlayerValidMoves.Count != 0)
                        {
                            othelloControl.MakeMove(currentPlayerValidMoves[rand.Next(currentPlayerValidMoves.Count)], playerList[currentPlayerIdx + 1].PlayerPiece);
                        }

                    }
                }
                running = ShowMenu(othelloControl);
            }
        }

        static (List<IPlayer>, bool) AddPlayer()
        {
            List<IPlayer> playingPlayer = new List<IPlayer>();
            string[] options = { "VS CPU", "VS Other Player" };
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Enter Number of Player");
                Console.WriteLine("Use ↑ ↓ to navigate, Enter to select:");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();

                }
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                    index = (index == 0) ? options.Length - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % options.Length;

            } while (key != ConsoleKey.Enter);
            Console.Clear();

            if (index == 0)
            {
                Console.WriteLine("You have choosen to play against computer");
                Console.WriteLine("Enter Player 1 (Black) Name:");
                string? player1Username = Console.ReadLine();
                playingPlayer.Add(new Player(player1Username, Piece.Black));
                playingPlayer.Add(new Player("CPU", Piece.White));
            }
            else
            {
                Console.WriteLine("You have choosen to play against computer");
                Console.WriteLine("Enter Player 1 (Black) Name:");
                string? player1Username = Console.ReadLine();
                playingPlayer.Add(new Player(player1Username, Piece.Black));

                Console.WriteLine("Enter Player 2 (White) Name:");
                string? player2Username = Console.ReadLine();
                playingPlayer.Add(new Player(player2Username, Piece.White));
            }

            return (playingPlayer, index != 0);
        }
        static bool ShowMenu(GameController controller)
        {
            string[] options = { "Restart", "Exit" };
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                controller.DisplayBoard(0);
                Console.WriteLine("Use ↑ ↓ to navigate, Enter to select:\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine($"{options[i]}\n");

                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                    index = (index == 0) ? options.Length - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % options.Length;

            } while (key != ConsoleKey.Enter);

            // If "Restart" is chosen, return true, otherwise false
            return index == 0;
        }
    }
}


/*
Black — C4

White — C3

Black — E6

White — B4

Black — A4

White — A5

Black — B2

White — A3 ← after this move Black has no legal moves

Shortest Black Win
e6 f4 e3 f6 g5 d6 e7 f5 c5
*/
