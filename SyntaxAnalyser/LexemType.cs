using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyser
{
    /// <summary>
    /// Перечисление содержит все типы (категории) лексем, на которые были разделены все допустимые лексемы языка. 
    /// Это список значений, которые могут содержаться в таблице лексем.
    /// Такая вот унификация.
    /// </summary>
    public enum LexemType
    {
        Key_Word, 
        Identificator,
        Char_Constant,
        Number_Constant,
        Splitter,
        Condition_Symbol,
        Assignment_Symbol
    }
}
