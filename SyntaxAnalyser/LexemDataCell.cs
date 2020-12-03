using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    /// <summary>
    /// Данный класс представляет собой группировку значения лексемы с расширенной информацией о ней: строка, в которой она написана,
    /// тип, к которому принадлежит лексема. 
    /// Представляет собой строку из таблицы лексем.
    /// </summary>
    public class LexemDataCell
    {
        /// <summary>
        /// Номер строки в файле, в котором записана лексема
        /// </summary>
        public int NumOfString { get; private set; }

        /// <summary>
        /// Значение лексемы. Типа просто текст.
        /// </summary>
        public string Lexem { get; private set; }

        /// <summary>
        /// Тип лексемы. Они заранее были классифицированы, т.е. разделены на типы.
        /// </summary>
        public LexemType LexType { get; private set; }

        public LexemDataCell (int numOfStr, string lexem, LexemType lexType)
        {
            this.NumOfString = numOfStr;
            this.Lexem = lexem;
            this.LexType = lexType;
        }
    }
}
