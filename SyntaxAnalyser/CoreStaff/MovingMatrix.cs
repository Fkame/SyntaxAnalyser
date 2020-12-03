using System;

namespace SyntaxAnalyser.CoreStaff
{
    public class MovingMatrix 
    {
        char?[,] movings =
        {
            { null,     '=',    null,   null,   null,   null,   null,   null,   null,   null,   null },
            { '<',      null,   '=',    null,   '<',    null,   null,   null,   null,   '<',    null },
            { null,     null,   null,   '=',    null,   null,   null,   null,   null,   null,   null },
            { '<',      null,   '>',    null,   '<',    null,   null,   null,   null,   '>',    null },
            { null,     null,   '>',    null,   null,   '=',    '=',    '=',    '=',    '>',    null },
            { null,     null,   null,   null,   '=',    null,   null,   null,   null,   null,   null },
            { null,     null,   null,   null,   '=',    null,   null,   null,   null,   null,   null },
            { null,     null,   null,   null,   '=',    null,   null,   null,   null,   null,   null },
            { null,     null,   null,   null,   '=',    null,   null,   null,   null,   null,   null },
            { '<',      null,   '>',    null,   '<',    null,   null,   null,   null,   '=',    '>'  },
            { '<',      null,   null,   null,   '<',    null,   null,   null,   null,   '<',    null }
        };
        public readonly string[] keysOfColumns = {"for", "(", ")", "do", "a", ":=", "<", ">", "=", ";", SpecialSymbs.END_SYMB};
        public readonly string[] keysOfRows = {"for", "(", ")", "do", "a", ":=", "<", ">", "=", ";", SpecialSymbs.START_SYMB};

        public MovingMatrix() { }
       
        public MovingMatrix(char?[,]movings, string[]keysOfRows, string[]keysOfColumns) 
        {
            this.movings = new char?[movings.GetLength(0), movings.GetLength(1)];
            Array.Copy(movings, this.movings, movings.Length);

            this.keysOfRows = new string[keysOfRows.Length + 1];
            Array.Copy(keysOfRows, this.keysOfRows, keysOfRows.Length);
            this.keysOfRows[keysOfRows.Length] = SpecialSymbs.START_SYMB;

            this.keysOfColumns = new string[keysOfColumns.Length + 1];
            Array.Copy(keysOfColumns, this.keysOfColumns, keysOfColumns.Length);
            this.keysOfColumns[keysOfColumns.Length] = SpecialSymbs.END_SYMB;
        }

        public char? GetMove(string rowKey, string columnKey) 
        {
            int indexRow = Array.IndexOf(keysOfRows, rowKey);
            int indexCol = Array.IndexOf(keysOfColumns, columnKey);
            if (indexRow == -1 || indexCol == -1) 
                throw new ArgumentException("No such key words in Moving Matrix!");
            return movings[indexRow, indexCol];
        }
    }
}