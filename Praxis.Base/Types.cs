using System;

namespace Praxis.Base
{
    public class Types
    {
        public enum PieceTypes { King, Queen, Rook, Bishop, Knight, Pawn }
        public enum Colors { White, Black }
        public enum Files { a = 1, b, c, d, e, f, g, h }
        public enum Ranks { one = 1, two, three, four, five, six, seven, eight }
        public enum Stages { Opening, Middle, End }
        public enum Styles { Offensive, Defensive, Tactical, Positional, Universal }
        public enum Ratings { J, I, H, G, F, E, D, C, B, A, Expert, NationalMaster, SeniorMaster } // Based on USCF rating categories.
    }
}
