using System;
using System.Collections.Generic;

namespace Praxis.Engine.Presentation
{
    /// <summary>
    /// UCI Specification: http://wbec-ridderkerk.nl/html/UCIProtocol.html
    /// </summary>
    internal class UCI : CommunicationProtocol
    {
        // Further documentation:
        // python-chess.readthedocs.io/en/latest/uci.html
        // andchess2006.narod.ru/logic_work/UCI.htm
        // www.lokasoft.nl/uci-standard.aspx

        override internal Engine Engine { get; set; }

        private bool _recievedFirstPositionCommand;

        internal UCI(Engine engine) : base(engine)
        {
            _recievedFirstPositionCommand = false;
        }

        override internal List<string> ProcessInput(string input)
        {
            List<string> output;

            if (input.StartsWith("debug"))
            {
                output = new List<string>();
            }
            else if (input.StartsWith("setoption"))
            {
                output = new List<string>();
            }
            else if (input.StartsWith("position"))
            {
                output = HandlePosition(input);
            }
            else if (input.StartsWith("go"))
            {
                output = GetNextMove(input);
            }
            else
            {
                switch (input)
                {
                    case "uci":
                        output = Startup();
                        break;
                    case "isready":
                        output = HandleReady();
                        break;
                    // register
                    case "ucinewgame":
                        output = HandleUCINewGame();
                        break;
                    // stop
                    // ponderhit
                    case "quit":
                        output = HandleQuit();
                        break;
                    default:
                        output = null;
                        break;
                }
            }

            return output;
        }

        private List<string> Startup()
        {
            return new List<string>()
            {
                "id name " + Engine.NAME,
                "id author " + Engine.AUTHOR,
                //TODO: Options.
                "uciok"
            };
        }

        private List<string> HandleReady()
        {
            Engine.CostlySetup();

            return new List<string>() { "readyok" };
        }

        private List<string> HandleUCINewGame()
        {
            _recievedFirstPositionCommand = false;

            return null;
        }

        private List<string> HandlePosition(string input)
        {
            if (!_recievedFirstPositionCommand)
            {
                // New game starting.
                _recievedFirstPositionCommand = true;
            }

            string posistion = input.Remove(0, "position ".Length);

            if (posistion.StartsWith("startpos"))
            {
                Engine.LoadGame();
                posistion = posistion.Remove(0, "startpos".Length).TrimStart();
            }
            else if (posistion.StartsWith("fen "))
            {
                string fenRecord;

                if (posistion.Contains("moves "))
                {
                    int from = posistion.IndexOf("fen ") + "fen ".Length;
                    int to = posistion.LastIndexOf(" moves ");
                    fenRecord = posistion.Substring(from, to - from);
                }
                else
                {
                    fenRecord = posistion.Remove(0, "fen ".Length).TrimEnd();
                }

                Engine.LoadGame(fenRecord);
                posistion = posistion.Remove(0, string.Format("fen {0}", fenRecord).Length).TrimEnd();
            }

            if (posistion.StartsWith("moves "))
            {
                posistion = posistion.Remove(0, "moves ".Length);
                string[] moveNotations = posistion.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < moveNotations.Length; i++)
                {
                    string moveNotation = moveNotations[i];

                    if (!moveNotation.Equals("0000"))
                    {
                        Engine.Board.MovePiece(Application.BoardRepresentation.Move.ConvertAlgebraicToMove(moveNotation, Engine));
                    }
                }
            }

            return null;
        }

        private List<string> HandleQuit()
        {
            Engine.IsActive = false;

            return null;
        }

        private List<string> GetNextMove(string input)
        {
            //TODO: New thread in case we have to prematurely return a move?

            input = input.Remove(0, "go".Length).TrimStart();
            string[] goParameters = input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < goParameters.Length; i++)
            {
                HandleGoParameter(goParameters[i], goParameters);
            }

            string move;
            if (Engine.WhitePlayer.MovesNext)
            {
                move = Engine.WhitePlayer.MakeMove();
            }
            else
            {
                move = Engine.BlackPlayer.MakeMove();
            }

            return new List<string>() { "bestmove " + move ?? "ERROR" };
        }

        private void HandleGoParameter(string parameter, string[] goParameters)
        {
            //TODO: Handle GO parameters. Probably correspond to the options we set.
            //searchmoves
            //ponder
            //wtime
            //btime
            //winc
            //binc
            //movestogo
            //depth
            //nodes
            //mate
            //movetime
            //infinite
        }
    }
}
