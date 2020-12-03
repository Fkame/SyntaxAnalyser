using System;
using System.Collections.Generic;

namespace SyntaxAnalyser.CoreStaff 
{
    /// <summary>
    /// Правосторонний стек, хранящий символы для сдвига-свёртки.
    /// </summary>
    public class SymbolsStack 
    {
        private string[] stack = new string[30];
        public int Lenght { get; private set; } = 0;

        private const float COEFF_OF_ARRAY_GROWING = 1.5f;

        public SymbolsStack() {  }

        public SymbolsStack(string firstValue) 
        { 
            this.Push(firstValue);
        }

        public void Push(string value) 
        {
            if (Lenght == stack.Length) this.GrowUpArray();
            stack[Lenght] = value;
            Lenght += 1;
        }

        public string Pop() 
        {
            if (Lenght == 0) throw new InvalidOperationException("Stack is empty!");
            string value = stack[Lenght - 1];
            stack[Lenght - 1] = null;
            Lenght -= 1;
            return value;
        }

        public List<string> Pop(int amount) 
        {
            List<string> poped = new List<string>();
            while (amount > 0) 
            {
                poped.Add(this.Pop());
                amount--;
            }
            
            return poped;
        }

        public string GetFirstTerminalSymb() 
        {
            int idx = Lenght - 1;
            while (stack[idx] == SpecialSymbs.NOT_TERMINAL_SYMB && idx >= 0)
            {
                idx -= 1;
            }

            if (idx < 0) return String.Empty;
            return stack[idx];
        }

        private void GrowUpArray() 
        {
            int newSize = (int)(Lenght * COEFF_OF_ARRAY_GROWING);
            string[] newArray = new string[newSize];
            Array.Copy(stack, newArray, Lenght);
            stack = newArray;
        }

        public string[] StackToArray() 
        {
            string[] array = new string[Lenght];
            Array.Copy(stack, array, this.Lenght);
            return array;
        }
    }
}