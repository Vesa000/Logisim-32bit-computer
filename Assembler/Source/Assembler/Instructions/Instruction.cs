using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Instruction
    {
        public int lineNr;
        public string label;
        public string condition;
        public string opcode;

        public Instruction(string line, string insLabel,int linenumber)
        {
            lineNr = linenumber;
            label = insLabel;
            string[] operands = line.Split("\t".ToCharArray());

            //Condition
            if (operands[0] == "")
            {
                condition = "ALW";
            }
            else
            {
                condition = operands[0];
            }
        }
        public virtual int Output(Dictionary<string,int>conditionDict,Dictionary<string,int> opcodeDict)
        {
            Console.WriteLine("virtual int output was not overwritten");
            return 0;
        }
    }
}
