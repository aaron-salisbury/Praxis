using Praxis.Base;
using Praxis.Base.Models;
using Praxis.Business.BoardRepresentation;
using Praxis.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Praxis.Business.Evaluation
{
    internal class OpeningMoveEvaluator : MoveEvaluator
    {
        // theboardgameguide.com/best-chess-openings/
        private readonly Dictionary<int, string> _rankedWhiteOpenings = new Dictionary<int, string>()
        {
            { 1, "1.d4 d5 2.c4" },              // Queen's Gambit,          ECO: D06–D69
            { 2, "1.e4 e5 2.Nf3 Nc6 3.Bb5"},    // Ruy Lopez,               ECO: C60–C99
            { 3, "1.e4 e5 2.Nf3 Nc6 3.Bc4" },   // Italian Game,            ECO: C50–C59
            { 4, "1.c4" },                      // English Opening,         ECO: A10–A39
            //{ 5, "1.d4" },                      // London System,           ECO: D02, A46, A48
            { 6, "1.Nf3 d5 2.g3" },             // King’s Indian Attack,    ECO: A07-A08
            { 7, "1.d4 Nf6 2.c4 c5 3.d5 b5" }   // Benko Gambit,            ECO: A57–A59
        };

        private readonly Dictionary<int, string> _rankedBlackOpenings = new Dictionary<int, string>()
        {
            { 1, "1.e4 c5" },                       // Sicilian Defense,    ECO: B20–B99
            { 2, "1.d4 Nf6 2.c4 e6 3.Nc3 Bb4" },    // Nimzo-Indian,        ECO: E20–E59
            { 3, "1.d4 Nf6 2.c4 e6 3.Nf3 b6" },     // Queen's Indian,      ECO: E12–E19
            { 4, "1.e4 d6" }                        // Pirc Defence,        ECO: B07–B09
        };

        internal OpeningMoveEvaluator(Engine engine) : base(engine) { }

        internal override Move GetBestMove()
        {
            Opening bestOpening = null;
            List<Opening> potentialOpenings = OpeningEvaluator.GetPotentialOpenings(Engine);

            if (Engine.WhitePlayer.MovesNext)
            {
                foreach (KeyValuePair<int, string> rankedWhiteOpening in _rankedWhiteOpenings)
                {
                    bestOpening = potentialOpenings.Where(po => po.Value.StartsWith(rankedWhiteOpening.Value)).FirstOrDefault();
                    if (bestOpening != null)
                    {
                        break;
                    }
                }
            }
            else if (Engine.BlackPlayer.MovesNext)
            {
                foreach (KeyValuePair<int, string> rankedBlackOpening in _rankedBlackOpenings)
                {
                    bestOpening = potentialOpenings.Where(po => po.Value.StartsWith(rankedBlackOpening.Value)).FirstOrDefault();
                    if (bestOpening != null)
                    {
                        break;
                    }
                }
            }

            if (bestOpening == null)
            {
                //TODO: If no ranked opening is possible, just picking random opening.
                int openingIndex = Engine.RandomGenerator.Next(0, potentialOpenings.Count);
                bestOpening = potentialOpenings[openingIndex];
            }

            return ConvertOpeningToNextMove(bestOpening);
        }

        private Move ConvertOpeningToNextMove(Opening opening)
        {
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
