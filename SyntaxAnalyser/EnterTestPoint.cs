using System;
using System.Collections.Generic;
using System.Diagnostics;
using SyntaxAnalyser.CoreStaff;
using System.Reflection;

namespace SyntaxAnalyser
{
    class EnterTestPoint
    {
        static void Main(string[] args)
        {
            EnterTestPoint.MovingMatrixTest();
            Console.WriteLine("Test#1 - Success");

            EnterTestPoint.RulesMatrixTest();
            Console.WriteLine("Test#2 - Success");

            EnterTestPoint.SymbolsStackTest();
            Console.WriteLine("Test#3 - Success");

            EnterTestPoint.SyntaxScannerFullTest();
            Console.WriteLine("Test#4 - Success");

        }

        static void MovingMatrixTest() 
        {
            MovingMatrix m = new MovingMatrix();
            
            char?[] trueRez = { null, '=', null, '>', '<'};
            Debug.Assert(trueRez[0].Equals(m.GetMove(SpecialSymbs.START_SYMB, SpecialSymbs.END_SYMB)));
            Debug.Assert(trueRez[1].Equals(m.GetMove("for", "(")));
            Debug.Assert(trueRez[2].Equals(m.GetMove("a", "a")));
            Debug.Assert(trueRez[3].Equals(m.GetMove(";", ")")));
            Debug.Assert(trueRez[4].Equals(m.GetMove(";", "for")));
        }

        static void RulesMatrixTest() 
        {
            RulesMatrix r = new RulesMatrix();

            string[][] inputData = new string[6][];
            inputData[0] = new string[] {";", "!N", ";"};
            inputData[1] = new string[] {";", ";", "!N", ";"};
            inputData[2] = new string[] {"!N"};
            inputData[3] = new string[] {"!N", ";"};
            inputData[4] = new string[] {";", "!N"};
            inputData[5] = new string[] { "a", ">", "a"};
           
            bool[] trueRez1 = { true, false, false, true, false, true};
            Debug.Assert(r.IsContains(inputData[0]) == true);
            Debug.Assert(r.IsContains(inputData[1]) == false);
            Debug.Assert(r.IsContains(inputData[2]) == false);
            Debug.Assert(r.IsContains(inputData[3]) == true);
            Debug.Assert(r.IsContains(inputData[4]) == false);
            Debug.Assert(r.IsContains(inputData[5]) == true);

            int [] trueRez2 = { 7, -1, -1, 1, -1, 9};
            Debug.Assert(r.GetNumberOfRule(inputData[0]) == 7);
            Debug.Assert(r.GetNumberOfRule(inputData[1]) == -1);
            Debug.Assert(r.GetNumberOfRule(inputData[2]) == -1);
            Debug.Assert(r.GetNumberOfRule(inputData[3]) == 1);
            Debug.Assert(r.GetNumberOfRule(inputData[4]) == -1);
            Debug.Assert(r.GetNumberOfRule(inputData[5]) == 9);
        }

        static void SymbolsStackTest()
        {
            SymbolsStack s = new SymbolsStack();
            string[] strs = {"1", "2", "3", "4", "5" };

            s.Push(strs[0]);
            s.Push(strs[1]);
            Debug.Assert(s.Lenght == 2);

            string poped = s.Pop();
            Debug.Assert(poped.Equals(strs[1]));
            Debug.Assert(s.Lenght == 1);

            s.Push(strs[1]);
            s.Push(strs[2]);
            Debug.Assert(s.Lenght == 3);
            string getted = s.GetFirstTerminalSymb();
            Debug.Assert(s.Lenght == 3);
            Debug.Assert(getted.Equals(strs[2]));

            s.Push(SpecialSymbs.NOT_TERMINAL_SYMB);
            Debug.Assert(s.Lenght == 4);
            getted = s.GetFirstTerminalSymb();
            Debug.Assert(s.Lenght == 4);
            Debug.Assert(getted.Equals(strs[2]));

            s = new SymbolsStack();
            for (int i = 0; i < 100; i++)
                s.Push(Convert.ToString(i));
            Debug.Assert(s.Lenght == 100);
            for (int i = 99; i >= 0; i--)
                Debug.Assert(Convert.ToString(i).Equals(s.Pop()));
            Debug.Assert(s.Lenght == 0);
        }

        static void SyntaxScannerFullTest() 
        {
            List<LexemDataCell> table = EnterTestPoint.LexemTableGeneration();
            List<OutputTreeCell> tree = null;
            try 
            {
                SyntaxScanner scanner = new SyntaxScanner(table);
                tree = scanner.DoAnalysis();
            } catch (Exception ex) { Console.WriteLine("---ExceptionStafff---\n{0}\n", ex); }
           
           DrawOutputTree(tree);
        }

        static List<LexemDataCell> LexemTableGeneration() 
        {
            List<LexemDataCell> table = new List<LexemDataCell>();
            table.Add(new LexemDataCell(1, "for", LexemType.Key_Word));
            table.Add(new LexemDataCell(1, "(", LexemType.Splitter));
            table.Add(new LexemDataCell(1, ";", LexemType.Splitter));
            table.Add(new LexemDataCell(1, "k", LexemType.Identificator));
            table.Add(new LexemDataCell(1, "=", LexemType.Condition_Symbol));
            table.Add(new LexemDataCell(1, "k2", LexemType.Identificator));
            table.Add(new LexemDataCell(1, ";", LexemType.Splitter));
            table.Add(new LexemDataCell(1, ")", LexemType.Splitter));
            table.Add(new LexemDataCell(1, "do", LexemType.Key_Word));
            table.Add(new LexemDataCell(1, "index1", LexemType.Identificator));
            table.Add(new LexemDataCell(1, ":=", LexemType.Assignment_Symbol));
            table.Add(new LexemDataCell(1, "ident_2", LexemType.Identificator));
            table.Add(new LexemDataCell(1, ";", LexemType.Splitter));

            return table;
        }

        static void DrawOutputTree(List<OutputTreeCell> tree) 
        {
            OutputTreeDrawer drawer = new OutputTreeDrawer(tree);
            drawer.AmountOfSlashesBeforeSymbol = 2;
            drawer.DrawToConsole();
        }
    }
}
