using Praxis.Engine.Win98.Application.BoardRepresentation;
using Praxis.Engine.Win98.Application.BoardRepresentation.Pieces;
using System;
using System.Collections.Generic;
using static Praxis.Engine.Win98.Application.Types;

namespace Praxis.Engine.Win98.Application
{
    public class Player
    {
        public Colors Color { get; set; }
        internal bool IsHuman { get; set; }
        internal Styles Style { get; set; }
        internal Ratings Rating { get; set; }
        internal bool MovesNext { get; set; }
        internal bool CanCastleKingside { get; set; }
        internal bool CanCastleQueenside { get; set; }
        internal TimeSpan Clock { get; set; }
        internal Engine Engine { get; set; }
        internal King King
        {
            get
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Engine.Board.Squares[i, j] != null && Engine.Board.Squares[i, j].PieceType == PieceTypes.King && Engine.Board.Squares[i, j].Player == this)
                        {
                            return ((King)Engine.Board.Squares[i, j]);
                        }
                    }
                }

                return null;
            }
        }

        internal Player(Colors color, Engine engine, Styles style, Ratings rating = Ratings.SeniorMaster)
        {
            Style = style;
            Rating = rating;
            Color = color;
            Engine = engine;
        }

        internal string MakeMove()
        {
            Move bestMove = Engine.MoveEvaluator.GetBestMove();

            if (bestMove == null)
            {
                return null;
            }

            King.IsChecked = false; // If the player's king was in check prior to this move, it isn't now.
            Engine.Board.MovePiece(bestMove);

            return bestMove.ConvertToAlgebraic();
        }

        internal List<Move> GetValidMoves()
        {
            List<Move> potentialMoves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Engine.Board.Squares[i, j] != null && Engine.Board.Squares[i, j].Player == this)
                    {
                        potentialMoves.AddRange(Engine.Board.Squares[i, j].GetValidMoves());
                    }
                }
            }

            return potentialMoves;
        }

        internal List<Move> GetLegalMoves()
        {
            List<Move> potentialMoves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Engine.Board.Squares[i, j] != null && Engine.Board.Squares[i, j].Player == this)
                    {
                        potentialMoves.AddRange(Engine.Board.Squares[i, j].GetLegalMoves());
                    }
                }
            }

            return potentialMoves;
        }

        internal List<Move> GetLegalMovesByType(PieceTypes pieceType)
        {
            List<Move> potentialMoves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Engine.Board.Squares[i, j] != null && Engine.Board.Squares[i, j].PieceType == pieceType && Engine.Board.Squares[i, j].Player == this)
                    {
                        potentialMoves.AddRange(Engine.Board.Squares[i, j].GetLegalMoves());
                    }
                }
            }

            return potentialMoves;
        }

        internal static Styles GetRandomStyle(Random random)
        {
            int numberOfStyles = Enum.GetValues(typeof(Styles)).Length;
            int randomStyleValue = random.Next(0, numberOfStyles);

            return (Styles)randomStyleValue;
        }

        internal static Ratings GetRandomRating(Random random)
        {
            int numberOfRatings = Enum.GetValues(typeof(Ratings)).Length;
            int randomRatingValue = random.Next(0, numberOfRatings);

            return (Ratings)randomRatingValue;
        }

        // Personalities:
        //   Bobby Fischer - Aggresive
        //   Paul Morphy
        //   Samuel Reshevsky - Positional
        //   Reuben Fine
        //   Frank Marshall
        //       Some more: www.chess.com/forum/view/general/gm-playing-style

        // www.chess.com/forum/view/general/chess-styles | pathtochessmastery.blogspot.com/2012/06/playing-styles-deconstructed.html | www.reddit.com/r/chess/comments/2xr0fv/what_are_some_different_chess_playing_styles/ | www.chessfiles.com/chessfiles-blog/different-chess-playing-styles
    }
}
