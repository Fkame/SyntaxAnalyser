using System;
using System.Collections.Generic;
using SyntaxAnalyser.CoreStaff;

using NLog;

namespace SyntaxAnalyser 
{
    public class SyntaxScanner 
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Таблица лексем, полученная после Лексического анализа.
        /// </summary>
        private List<LexemDataCell> lexemTable;

        /// <summary>
        /// Матрица предшествования. Очень необходимый элемент, без которого не получится проверить семантику на правильность.
        /// </summary>
        private MovingMatrix movingMatrix;

        /// <summary>
        /// Стек для сдвига-свёртки - магазин МП-автомата.
        /// </summary>
        private SymbolsStack stack;

        private RulesMatrix rules;

        /// <summary>
        /// Список применённых правил.
        /// </summary>
        private List<int>usedRulesList;

        public SyntaxScanner(List<LexemDataCell> table) 
        {
            lexemTable = new List<LexemDataCell>(table);
            stack = new SymbolsStack(SpecialSymbs.START_SYMB);
            movingMatrix = new MovingMatrix();
            rules = new RulesMatrix();
            usedRulesList = new List<int>();

            Logger.Debug("SyntaxScanner created!");
        }

        public List<OutputTreeCell> DoAnalysis() 
        {
            Logger.Debug("Analysis started!");
            string inputLex = null;
            string stackLex = null;
            lexemTable.Add(new LexemDataCell(0, SpecialSymbs.END_SYMB, LexemType.Splitter));
            LexemDataCell[] lexemArray = lexemTable.ToArray();
            for (int i = 0; i < lexemArray.Length; i++)
            {
                LexemDataCell cell = lexemArray[i];

                string inputInner = string.Join(" ", this.InnerOfLexemArrayToStringArray(lexemArray, i));
                string stackInner = string.Join(" ", stack.StackToArray());
                Logger.Info("Input = {0} || Stack = {1}\n", inputInner, stackInner);

                inputLex = cell.Lexem;
                if (cell.LexType.Equals(LexemType.Identificator)) inputLex = "a";

                stackLex = stack.GetFirstTerminalSymb();

                if (stackLex.Equals(SpecialSymbs.START_SYMB) & inputLex.Equals(SpecialSymbs.END_SYMB)) break;

                char? move = movingMatrix.GetMove(stackLex, inputLex);

                Logger.Info("{0} {1} {2}\n", stackLex, move, inputLex);

                try 
                {    
                    switch (move) 
                    {
                        case '=': 
                            this.MakeShifting(inputLex);
                            break;
                        case '<': 
                            this.MakeShifting(inputLex);
                            break;
                        case '>': 
                            this.MakeRollingUp();
                            i -= 1;
                            break;
                        default: 
                            if (inputLex.Equals(SpecialSymbs.END_SYMB)) 
                            {
                                MakeRollingUp(); 
                                break;
                            }
                            throw new Exception(
                                "Нарушен синтаксис входного языка: связки слов " + stackLex + " + " + inputLex + " не предусмотрено!");
                    }
                } catch (Exception ex) { 
                    Logger.Error(ex, 
                        "Во время очередного сдвига-свёртки произошла ошибка. Работа программы прекращена!\nMessage = {0}\nStack-trace:\n{1}\n", ex.Message, ex.StackTrace);
                    return null;
                }     
            }

            stackLex = stack.GetFirstTerminalSymb();

            if (!stackLex.Equals(SpecialSymbs.START_SYMB)) 
            {
                throw new Exception("Нарушен синтаксис входного языка: непредусмотренный обрыв конструкции!");
            }
            else 
            {
                Logger.Info("Разбор успешно окончен!\n");
            }

            return this.BuildOutputTree();
        }

        public string[] InnerOfLexemTableToStringArray(List<LexemDataCell> table, int startFrom) 
        {
            if (startFrom + 1 >= table.Count) throw new ArgumentOutOfRangeException("Индекс начала превосходит размер массива!");
            string[] content = new string[table.Count - startFrom];
            int count = 0;
            int index = 0;
            foreach(LexemDataCell cell in table) 
            {
                if (count++ < startFrom) continue;
                content[index++] = cell.Lexem;
            }

            return content;
        }

        public string[] InnerOfLexemArrayToStringArray(LexemDataCell[] lexemArray, int startFrom) 
        {
            Logger.Info("----Converter InnerOfLexemArrayToStringArray. ArrayLen = {0}, start index = {1}\n", lexemArray.Length, startFrom);
            if (startFrom >= lexemArray.Length) throw new ArgumentOutOfRangeException("Индекс начала превосходит размер массива!");
            string[] content = new string[lexemArray.Length - startFrom];
            for (int i = 0; i < content.Length; i++) 
            {
                content[i] = lexemArray[i + startFrom].Lexem;
            }

            return content;
        }

        /// <summary>
        /// Операция сдвига - перекидывание символа из входной цепочки на вершину стека
        /// и сдвиг головки МП-автомата вправо - т.е. переход к след. символу входной цепочки
        /// </summary>
        private void MakeShifting(string inputLex)  
        {
            this.stack.Push(inputLex);
        }

        private void MakeRollingUp() 
        {
            Logger.Debug("--Rolling up started!");
            List<string> rule = new List<string>();

            string stackLexPost = SkipToNextTermSymb(rule);
            rule.Add(stackLexPost);
            Logger.Info("--Added first VT to rule-set: {0}\n", stackLexPost);

            string stackLexPre = SkipToNextTermSymb(rule);
           
            char? move = movingMatrix.GetMove(stackLexPre, stackLexPost);

            if (move.Equals('=')) 
            {
                while (move.Equals('=')) 
                {
                    rule.Add(stackLexPre);
                    stackLexPost = stackLexPre;
                    stackLexPre = SkipToNextTermSymb(rule);
                    move = movingMatrix.GetMove(stackLexPre, stackLexPost);
                }
            }

            Logger.Info("--Unused symb backed to stack: {0}\n", stackLexPre);
            stack.Push(stackLexPre);

            rule.Reverse();

            Logger.Info("--Builded rule: {0}\n", string.Join(" ", rule.ToArray()));

            int numOfRule = rules.GetNumberOfRule(rule.ToArray());
            if (numOfRule == -1) 
                throw new Exception(
                    "Нарушение синтаксиса языка: [ " + string.Join(" ", rule.ToArray()) + " ] - подобной конструкции не существует в языке!");

            usedRulesList.Add(numOfRule);
            stack.Push(SpecialSymbs.NOT_TERMINAL_SYMB);
        }

        private string SkipToNextTermSymb(List<string> ruleListToAddSymbolsFromStack) 
        {
            string stackLex = stack.Pop();
            while (stackLex.Equals(SpecialSymbs.NOT_TERMINAL_SYMB)) 
            {
                ruleListToAddSymbolsFromStack.Add(stackLex);
                stackLex = stack.Pop();
            }

            return stackLex;
        }  

        private List<OutputTreeCell> BuildOutputTree() 
        {
            Logger.Info("Building rule-list: {0}\n", string.Join(" ", usedRulesList.ToArray()));
            OutputTree outputTree = new OutputTree();
            List<OutputTreeCell> tree = outputTree.GetOutputTree(this.rules, this.usedRulesList);      
            return tree;
        }

    }
}
