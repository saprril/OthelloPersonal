using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace OthelloPersonal
{
    public class GameController(List<IPlayer> playerList)
    {
        // Column = X, Row = Y
        private IGameBoard _board;
        private HashSet<Position> _moveCandidate;
        private GameState _currentState;
        public List<IPlayer> IplayerList = playerList;

        public event EventHandler<GameOverEventArgs> OnGameOverEvent;

        public event EventHandler<MoveMadeEventArgs> OnMoveMade;

        public Dictionary<Directions, Position> DirectionsDict = new Dictionary<Directions, Position>()
        {
            { Directions.NorthWest, new Position() { Column = -1, Row = -1 } },
            { Directions.North,     new Position() { Column = 0,  Row = -1 } },
            { Directions.NorthEast, new Position() { Column = 1,  Row = -1 } },
            { Directions.East,      new Position() { Column = 1,  Row = 0 } },
            { Directions.SouthEast, new Position() { Column = 1,  Row = 1 } },
            { Directions.South,     new Position() { Column = 0,  Row = 1 } },
            { Directions.SouthWest, new Position() { Column = -1, Row = 1 } },
            { Directions.West,      new Position() { Column = -1, Row = 0 } },
        };


        // public Position ToPositionOffset()

        public Directions GetDirectionsTo(Position referencePosition, Position targetPosition)
        {
            Position distance = new Position { Column = targetPosition.Column - referencePosition.Column, Row = targetPosition.Row - referencePosition.Row };
            Directions key = DirectionsDict.FirstOrDefault(x => x.Value.Equals(distance)).Key;
            return key;
        }
        public List<Directions> ToDirection(Position positionX)
        {
            List<Directions> validDirections = new List<Directions>();
            foreach (var item in DirectionsDict)
            {
                Position adjacentPosition = positionX + item.Value;
                if (IsInsideBound(adjacentPosition) && IsInsideBound(positionX))
                {
                    if (_board.Squares[adjacentPosition.Column, adjacentPosition.Row].SquarePiece != _board.Squares[positionX.Column, positionX.Row].SquarePiece &&
                        _board.Squares[adjacentPosition.Column, adjacentPosition.Row].SquarePiece != Piece.Empty)
                    {
                        validDirections.Add(item.Key);
                    }
                }
            }

            return validDirections;
        }

        public void InitializeBoard()
        {
            _currentState = GameState.Initializing;
            _board = new GameBoard(8);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position tilePosition = new Position();
                    tilePosition.Column = i;
                    tilePosition.Row = j;
                    if (i == 3)
                    {
                        if (j == 3)
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.White, 1);
                        }
                        else if (j == 4)
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.Black, 1);
                        }
                        else
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.Empty, 0);
                        }
                    }
                    else if (i == 4)
                    {
                        if (j == 3)
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.Black, 1);
                        }
                        else if (j == 4)
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.White, 1);
                        }
                        else
                        {
                            _board.Squares[i, j] = new Square(tilePosition, Piece.Empty, 0);
                        }
                    }
                    else
                    {
                        _board.Squares[i, j] = new Square(tilePosition, Piece.Empty, 0);
                    }
                }
            }
            _moveCandidate = new HashSet<Position>(GetValidMoves(Piece.Black));
            _currentState = GameState.PlayerTurn;

            OnGameOverEvent += (sender, e) =>
            {
                Console.WriteLine($"Game Finished, {e.PieceColor} ({IplayerList.Where(x => x.PlayerPiece == e.PieceColor).First().Name}) Is Winning");
                Console.WriteLine("---------------------Final Score-------------------");
            };

            OnMoveMade += (sender, e) =>
            {
                _board.Squares[e.targetPosition.Column, e.targetPosition.Row] = new Square(e.targetPosition, e.color, 1);
                foreach (var item in e.positionDictionary)
                {
                    Position iter = e.targetPosition;
                    while (!iter.Equals(item.Value))
                    {
                        iter += DirectionsDict[item.Key];
                        _board.Squares[iter.Column, iter.Row] = new Square(iter, e.color, 1);
                    }
                }
            };
        }
        public bool IsInsideBound(Position positionX)
        {
            return (positionX.Column < 8 && positionX.Row < 8) && (positionX.Column >= 0 && positionX.Row >= 0);
        }
        public void PrintBoard()
        {
            Func<Piece, string> boardProcessor = (x) =>
            {
                if (x == Piece.Black)
                {
                    return "O";
                }
                else if (x == Piece.White)
                {
                    return "@";
                }
                else
                {
                    return "▢";
                }
            };
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(string.Join(" ",
                                        Enumerable.Range(0, _board.Squares.GetLength(1))
                                                    .Select(j => boardProcessor(_board.Squares[j, i].SquarePiece))));
            }
        }

        public void DisplayBoard(int indexOfMoveCandidate)
        {
            // Console.Clear();
            if (IsGameOver())
            {
                Piece? winningPiece = GetWinner();
                HandleGameOver(new GameOverEventArgs(winningPiece));
            }
            Console.WriteLine($"Black = {CountPieces(Piece.Black)}              White={CountPieces(Piece.White)}");
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            List<Position> indexedMoveCandidate = new List<Position>(_moveCandidate);
            if (indexedMoveCandidate.Count != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write("|");
                        if (_board.Squares[j, i].SquarePiece == Piece.Black)
                        {
                            Console.Write(" B ");
                        }
                        else if (_board.Squares[j, i].SquarePiece == Piece.White)
                        {
                            Console.Write(" W ");
                        }
                        else
                        {
                            if (_moveCandidate.Contains(_board.Squares[j, i].SquarePosition))
                            {
                                Console.BackgroundColor = (_board.Squares[j, i].SquarePosition.Equals(indexedMoveCandidate[indexOfMoveCandidate])) ? ConsoleColor.DarkCyan : ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("   ");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write("   ");
                            }
                        }
                    }
                    Console.Write("|\n");
                    Console.WriteLine("+---+---+---+---+---+---+---+---+");
                }
                // Console.WriteLine($"Current Position (GameController)={indexedMoveCandidate[indexOfMoveCandidate]}");
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write("|");
                        if (_board.Squares[j, i].SquarePiece == Piece.Black)
                        {
                            Console.Write(" B ");
                        }
                        else if (_board.Squares[j, i].SquarePiece == Piece.White)
                        {
                            Console.Write(" W ");
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    Console.Write("|\n");
                    Console.WriteLine("+---+---+---+---+---+---+---+---+");
                }
            }

        }

        public List<Position> GetValidMoves(Piece pieceColor)
        {
            // Console.WriteLine("Function call +============================================");
            List<Position> validMoveList = new List<Position>();
            ISquare[,] arr = _board.Squares;

            var piecePositions = arr.Cast<ISquare>()
                                .Where(x => x.SquarePiece == pieceColor)
                                .Select(x => x.SquarePosition);
            // Console.WriteLine(string.Join(", ", filtered));
            foreach (var item in piecePositions)
            {
                List<Directions> keyList = ToDirection(item);
                foreach (var item1 in keyList)
                {
                    Position nextTile = item + DirectionsDict[item1];
                    // Console.WriteLine($"{pieceColor} Position {item.ToString()} - Directions {item1}, Traversed Adjacent Tile {nextTile.ToString()}");
                    while (IsInsideBound(nextTile) &&
                            arr[nextTile.Column, nextTile.Row].SquarePiece != Piece.Empty &&
                            arr[nextTile.Column, nextTile.Row].SquarePiece != pieceColor)
                    {
                        nextTile = nextTile + DirectionsDict[item1];
                        // Console.WriteLine($"Starting loop,{nextTile.ToString()}");
                    }
                    if (IsInsideBound(nextTile) &&
                        arr[nextTile.Column, nextTile.Row].SquarePiece != pieceColor)
                    {
                        // Console.WriteLine($"Tile added - {nextTile.ToString()}");
                        validMoveList.Add(nextTile);
                    }
                }

            }
            HashSet<Position> uniquePosition = new HashSet<Position>(validMoveList);
            List<Position> uniqueValidMoveList = new List<Position>(uniquePosition);
            return uniqueValidMoveList;
        }

        public bool IsValidMoves(Position startingPosition, Piece pieceColor)
        {
            return GetValidMoves(pieceColor).Contains(startingPosition);
        }

        public int CountPieces(Piece pieceColor)
        {
            ISquare[,] arr = _board.Squares;
            var piecePositions = arr.Cast<ISquare>().Where(x => x.SquarePiece == pieceColor).
                                    Select(x => x.Value);
            return (int)piecePositions.Sum();
        }

        public void MakeMove(Position piecePosition, Piece pieceColor)
        {
            _currentState = GameState.PlayerTurn;
            // Console.WriteLine("Move started ===========================================");
            if (IsValidMoves(piecePosition, pieceColor) && IsInsideBound(piecePosition))
            {
                List<Directions> originDirections = new List<Directions>();
                foreach (var item in DirectionsDict)
                {
                    Position adjIter = piecePosition + item.Value;
                    if (adjIter.Column + item.Value.Column >= 0 &&
                        adjIter.Column + item.Value.Column < _board.Squares.GetLength(0) &&
                        adjIter.Row + item.Value.Row >= 0 &&
                        adjIter.Row + item.Value.Row < _board.Squares.GetLength(1))
                    {
                        Piece adjColor = _board.Squares[adjIter.Column, adjIter.Row].SquarePiece;
                        if (adjColor != pieceColor && adjColor != Piece.Empty)
                        {
                            originDirections.Add(item.Key);
                        }
                    }
                    // Console.WriteLine(string.Join(", ", originDirections.Select(x => x.ToString())));
                }
                Dictionary<Directions, Position> filteredOrigins = new Dictionary<Directions, Position>();
                // Console.WriteLine($"Available Direction: {string.Join(", ", originDirections)}");
                foreach (var item in originDirections)
                {
                    Position adjacentCoordinate = piecePosition + DirectionsDict[item];
                    Console.WriteLine($"Position {piecePosition.ToString()} Direction {item}, adjacentCoordinate {adjacentCoordinate}, Current Piece {_board.Squares[adjacentCoordinate.Column, adjacentCoordinate.Row].SquarePiece}");

                    while (IsInsideBound(adjacentCoordinate) &&
                            pieceColor != _board.Squares[adjacentCoordinate.Column, adjacentCoordinate.Row].SquarePiece &&
                            _board.Squares[adjacentCoordinate.Column, adjacentCoordinate.Row].SquarePiece != Piece.Empty)
                    {
                        adjacentCoordinate += DirectionsDict[item];
                        filteredOrigins[item] = adjacentCoordinate;
                        Console.WriteLine($"Starting loop Position {piecePosition.ToString()} Direction {item}, adjacentCoordinate {adjacentCoordinate}");
                    }

                }
                Console.WriteLine(string.Join(", ", filteredOrigins));
                filteredOrigins = filteredOrigins
                                    .Where(kv => IsInsideBound(kv.Value))
                                    .Where(kv => _board.Squares[kv.Value.Column, kv.Value.Row].SquarePiece == pieceColor)
                                    .ToDictionary(kv => kv.Key, kv => kv.Value);


                Console.WriteLine($"Filtered dictionaries \n {string.Join(", ", filteredOrigins.Select(kv => $"{kv.Key}: {kv.Value}"))}");

                HandleMoveMad(new MoveMadeEventArgs(filteredOrigins, piecePosition, pieceColor));
                /*
                _board.Squares[piecePosition.Column, piecePosition.Row] = new Square(piecePosition, pieceColor, 1);
                foreach (var item in filteredOrigins)
                {
                    Position iter = piecePosition;
                    while (!iter.Equals(item.Value))
                    {
                        iter += DirectionsDict[item.Key];
                        _board.Squares[iter.Column, iter.Row] = new Square(iter, pieceColor, 1);
                    }
                }*/

            }
            if (IsGameOver())
            {
                _currentState = GameState.GameOver;
            }
            else
            {
                _currentState = GameState.MoveMade;
            }
            _moveCandidate = new HashSet<Position>(GetValidMoves((pieceColor == Piece.Black) ? Piece.White : Piece.Black));
        }

        public Piece? GetWinner()
        {
            bool isDraw = (CountPieces(Piece.Black) == CountPieces(Piece.White));
            Piece? mostPiece = (CountPieces(Piece.Black) > CountPieces(Piece.White)) ? Piece.Black : Piece.White;
            mostPiece = (isDraw) ? null : mostPiece;
            return mostPiece;
        }

        public bool IsGameOver()
        {
            return GetValidMoves(Piece.Black).Count == 0 && GetValidMoves(Piece.White).Count == 0;
        }

        protected virtual void HandleGameOver(GameOverEventArgs e)
        {
            OnGameOverEvent?.Invoke(this, e);
        }

        protected virtual void HandleMoveMad(MoveMadeEventArgs e) {
            OnMoveMade?.Invoke(this, e);
        }


    }

    public class GameOverEventArgs : EventArgs
    {
        public Piece? PieceColor { get; set; }

        public GameOverEventArgs(Piece? winning)
        {
            PieceColor = winning;
        }
    }

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
}