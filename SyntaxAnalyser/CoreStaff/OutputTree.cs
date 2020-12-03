using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyser.CoreStaff 
{
    /// <summary>
    /// Дерево Вывода для Синтаксического анализа.
    /// </summary>
    public class OutputTree 
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public OutputTree () { }

        public List<OutputTreeCell> GetOutputTree(RulesMatrix rulesMatrix, List<int> usedRulesList)
        { 
            List<OutputTreeCell> treeContainer = new List<OutputTreeCell>();
            
            OutputTreeCell root = new OutputTreeCell(0, SpecialSymbs.NOT_TERMINAL_SYMB);
            treeContainer.Add(root);

            Logger.Info("Iteration 0, tree: {0}", string.Join(" || ", treeContainer.ToArray()));

            for (int ruleIdx = usedRulesList.Count - 1; ruleIdx >= 0; ruleIdx--)
            {
                int iteration = usedRulesList.Count - ruleIdx;
                Logger.Info("Iteration: {0} || Rule #{1} || size of tree: {2}", iteration, usedRulesList[ruleIdx], treeContainer.Count);
                Logger.Info("- Tree at start: {0}", string.Join(" ", treeContainer.ToArray()));

                string[] ruleSymbs = rulesMatrix.GetRuleByNumber(usedRulesList[ruleIdx]);

                int insertIdx = lastIndexOfVnWithoutChilds(treeContainer) + 1;
                if ((insertIdx - 1) < 0) break;

                Logger.Info("-- Last index of VN without childs: {0}", insertIdx - 1);
                int childsLevel = treeContainer[insertIdx - 1].Level + 1;

                // Закидывание символов из текущего правила рядом с нетерминалом, который они раскрывают
                foreach (string symb in Reverse(ruleSymbs)) { treeContainer.Insert(insertIdx, new OutputTreeCell(childsLevel, symb)); }

                Logger.Info("- Tree at end: {0}", string.Join(" ", treeContainer.ToArray()));
            }

            Logger.Info("\nFull tree: {0}\n", string.Join(" || ", treeContainer.ToArray()));

            return treeContainer;
        }

        public string[] Reverse(string[] array)
        {
            string[] arrayR = new string[array.Length];
            for (int idxFromEnd = array.Length - 1; idxFromEnd >= 0; idxFromEnd--) 
            {
                int idxFromStart = (array.Length - 1) - idxFromEnd;
                arrayR[idxFromStart] = array[idxFromEnd];
            }
            return arrayR;
        }
        private int lastIndexOfVnWithoutChilds(List<OutputTreeCell> container) 
        {
            if (container.Count == 0) 
            {
                Logger.Info("-- OutputTree is empty");
                return -1;
            }
            if (container.Count == 1 & container[0].Value.Equals(SpecialSymbs.NOT_TERMINAL_SYMB)) return 0;
            if (container[container.Count - 1].Equals(SpecialSymbs.NOT_TERMINAL_SYMB)) return container.Count - 1;

            for (int i = container.Count - 2; i >= 0; i--)
            {   
                OutputTreeCell pre = container[i];
                OutputTreeCell post = container[i + 1];
                
                if (!pre.Value.Equals(SpecialSymbs.NOT_TERMINAL_SYMB)) continue;
                if (pre.Level < post.Level) continue;
                return i;     
            }

            Logger.Info("-- All VN with child elems");
            return -1;
        }

    }

    public struct OutputTreeCell 
    {
        public int Level {get; set;}
        public string Value {get; set;}

        public OutputTreeCell(int level, string value) 
        {
            this.Level = level;
            this.Value = value;
        }

        public override string ToString() 
        {
            return ("[ " + Level + " -> " + Value + " ]");
        }
    }

    internal class TreeCell
    {
        public TreeCell ParentCell {get; set;} = null;

        public int DeepLevel {get; private set;}
        
        private List<TreeCell> childCells = null;

        string lexemValue;

        internal TreeCell (string lexemValue, int level) 
        {
            this.lexemValue = lexemValue;
            DeepLevel = level;
        }

        public void AddChildCell(TreeCell childCell) 
        {
            this.childCells.Add(childCell);
        }

        public void SetChildCells(List<TreeCell> childCells) 
        {
            this.childCells = new List<TreeCell>(childCells);
        }
    }
}

