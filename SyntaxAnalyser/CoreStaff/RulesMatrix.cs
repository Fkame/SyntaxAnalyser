using System;

namespace SyntaxAnalyser.CoreStaff 
{
    public class RulesMatrix 
    {
        private string[][] rules = new string[10][];

        public RulesMatrix() 
        {
            rules[0] = new string[] { "!N", ";" };
            rules[1] = new string[] { "for", "(", "!N", ")", "do", "!N" };
            rules[2] = new string[] { "a", ":=", "a" };
            rules[3] = new string[] { "!N", ";", "!N", ";", "!N"};
            rules[4] = new string[] { ";", "!N", ";", "!N"};
            rules[5] = new string[] { "!N", ";", "!N", ";"};
            rules[6] = new string[] { ";", "!N", ";" };
            rules[7] = new string[] { "a", "<", "a"};
            rules[8] = new string[] { "a", ">", "a"};
            rules[9] = new string[] { "a", "=", "a"};
        }

        public int Count { get {return rules.Length;} }

        public RulesMatrix(string[][] rules) 
        {
            this.rules = rules;
        }

        public bool IsContains(string[] rule) 
        {
            for (int rulesIdx = 0; rulesIdx < rules.Length; rulesIdx++) 
            {
                string[] checkingRule = rules[rulesIdx];
                if (rule.Length != checkingRule.Length) continue;

                int symbIdx = 0;
                while (checkingRule[symbIdx].Equals(rule[symbIdx])) 
                {
                    symbIdx += 1;
                    if (symbIdx == checkingRule.Length) break;
                }

                if (symbIdx == rule.Length) return true;
            }

            return false;
        }

        public int GetNumberOfRule(string[] rule) 
        {
            for (int rulesIdx = 0; rulesIdx < rules.Length; rulesIdx++) 
            {
                string[] checkingRule = rules[rulesIdx];
                if (rule.Length != checkingRule.Length) continue;

                int symbIdx = 0;
                while (checkingRule[symbIdx].Equals(rule[symbIdx])) 
                {
                    symbIdx += 1;
                    if (symbIdx == checkingRule.Length) break;
                }

                if (symbIdx == rule.Length) return rulesIdx + 1;
            }

            return -1;
        }

        public string[] GetRuleByNumber(int number) 
        {
            return GetRuleByIndex(number - 1);
        }

        public string[] GetRuleByIndex(int index) 
        {
            string[] rule = new string[this.rules[index].Length];
            Array.Copy(rules[index], rule, rule.Length);
            return rule;
        }
    }
}