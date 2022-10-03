using System;

namespace Praxis.Engine.Win98.Application
{
    public class Types
    {
        public enum PieceTypes { King, Queen, Rook, Bishop, Knight, Pawn }
        public enum Colors { White, Black }
        internal enum Files { a = 1, b, c, d, e, f, g, h }
        internal enum Ranks { one = 1, two, three, four, five, six, seven, eight }
        internal enum Stages { Opening, Middle, End }
        internal enum Styles { Offensive, Defensive, Tactical, Positional, Universal }
        internal enum Ratings { J, I, H, G, F, E, D, C, B, A, Expert, NationalMaster, SeniorMaster } // Based on USCF rating categories.
    }
}
