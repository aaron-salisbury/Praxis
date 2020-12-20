using Praxis.Engine.Tiers.Application.BoardRepresentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Praxis.Engine.Tiers.Application.Evaluation
{
    internal class OpeningMoveEvaluator : MoveEvaluator
    {
        internal OpeningMoveEvaluator(Engine engine) : base(engine) { }

        internal override Move GetBestMove()
        {
            if (Engine.FullmoveCounter == 1 && Engine.WhitePlayer.MovesNext)
            {
                //TODO: For now just always starting with e2e4.
                return new Move()
                {
                    Piece = Engine.Board.Squares[(int)Types.Files.e - 1, (int)Types.Ranks.two - 1],
                    FromFile = Types.Files.e,
                    FromRank = Types.Ranks.two,
                    ToFile = Types.Files.e,
                    ToRank = Types.Ranks.four
                };
            }
            else
            {
                List<Opening> potentialOpenings = Opening.GetPotentialOpenings(Engine);

                //TODO: For now, just picking random opening.
                int openingIndex = Engine.RandomGenerator.Next(0, potentialOpenings.Count);
                Opening opening = potentialOpenings[openingIndex];

                string openingValue = opening.Value.Substring(Engine.SAN.Length).TrimStart();
                string[] moves = openingValue.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                string move = moves[0];

                // A move that places the opponent's king in check usually has the symbol "+" appended.
                move = move.TrimEnd('+');

                // Remove turn number and period.
                if (Engine.WhitePlayer.MovesNext)
                {
                    move = move.Remove(0, 2);

                    // If the turn number is two digits, still have to remove the period.
                    if (move[0].Equals('.'))
                    {
                        move = move.Remove(0, 1);
                    }
                }

                if (move.StartsWith("O"))
                {
                    return Move.GetCastlingMove(move, Engine);
                }
                else
                {
                    Player player = Engine.WhitePlayer.MovesNext ? Engine.WhitePlayer : Engine.BlackPlayer;
                    Types.PieceTypes movingPieceType = !char.IsUpper(move[0]) ? Types.PieceTypes.Pawn : Move.PieceTypesByLetters[move[0].ToString().ToLower()];
                    Types.Files toFile = (Types.Files)Enum.Parse(typeof(Types.Files), move[move.Length - 2].ToString());
                    Types.Ranks toRank = (Types.Ranks)Convert.ToInt32(move[move.Length - 1].ToString());

                    List<Move> potentialMoves = player.GetLegalMovesByType(movingPieceType)
                        .Where(m => m.ToFile == toFile && m.ToRank == toRank).ToList();

                    if (potentialMoves.Count == 1)
                    {
                        return potentialMoves.First();
                    }
                    else if (potentialMoves.Count > 1)
                    {
                        //Types.Files fromFile;
                        //TODO: Find file moving from to narrow it down. Examples: dxe4 Nbd7
                        return potentialMoves.First();
                    }
                    else
                    {
                        throw new Exception("Error: Could not extrapolate move from ECO opening: '" + move + "' Piece: '" + movingPieceType.ToString() + "' File: '" + toFile.ToString() + "' Rank: '" + toRank.ToString());
                    }
                }
            }
        }
    }
}
