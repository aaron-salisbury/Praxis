using System.Collections.Generic;
using System.Linq;
using static Praxis.Engine.Application.Types;

namespace Praxis.Engine.Application.BoardRepresentation.Pieces
{
    public abstract class Piece
    {
        public Player Player { get; set; }
        public PieceTypes PieceType { get; set; }
        /// <summary>
        /// Standard Algebraic Notation for current position.
        /// </summary>
        public string StandardNotation
        {
            get
            {
                return CurrentFile.ToString() + ((int)CurrentRank).ToString();
            }
        }

        internal Files? CurrentFile { get; set; }
        internal Ranks? CurrentRank { get; set; }
        internal string Figurine { get; set; }
        internal bool IsSliding { get; set; }
        //internal List<Dictionary<Files, Ranks>> AttackSquares { get; set; }

        public bool IsStandardAlgebraicMoveLegal(string standardAlgebraicMove)
        {
            return GetLegalMoves().Any(move => move.ConvertToAlgebraic().StartsWith(standardAlgebraicMove));
        }

        internal Piece(Player player, Files startingFile, Ranks startingRank)
        {
            Player = player;
            CurrentFile = startingFile;
            CurrentRank = startingRank;
            //AttackSquares = new List<Dictionary<Files, Ranks>>();

            Player.Engine.Board.Squares[(int)CurrentFile - 1, (int)CurrentRank - 1] = this;
        }

        internal abstract List<Move> GetValidMoves();

        internal abstract ICollection<Move> GetLegalMoves();

        internal abstract Piece ClonePiece();

        internal List<Move> RemoveIllegalMoves(List<Move> moves)
        {
            //return moves;

            List<Move> legalMoves = new List<Move>();

            foreach (Move move in moves)
            {
                Engine testEngine = Player.Engine.CloneEngine();

                Move testMove = new Move()
                {
                    FromFile = move.FromFile,
                    FromRank = move.FromRank,
                    ToFile = move.ToFile,
                    ToRank = move.ToRank
                };

                testEngine.Board.MovePiece(testMove);

                Player opposingPlayer = Player.Color == Colors.White ? testEngine.BlackPlayer : testEngine.WhitePlayer;

                if (opposingPlayer.GetValidMoves().Where(m => m.AttackedPiece != null && m.AttackedPiece.PieceType == PieceTypes.King).ToList().Count == 0)
                {
                    legalMoves.Add(move);
                }
            }

            return legalMoves;
        }
    }
}