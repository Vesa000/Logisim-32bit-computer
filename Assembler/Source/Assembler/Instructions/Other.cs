using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Other : Instruction
    {
        int register;
        int bit;

        public Other(string line, string inslabel, int linenumber):base(line,inslabel,linenumber)
        {
            string[] operands = line.Split("\t".ToCharArray());

            opcode = operands[1];
            if (opcode == "REB")
            {
                register = int.Parse(operands[2]);
                bit = int.Parse(operands[3]);
            }
        }
        public override int Output(Dictionary<string, int> conditionDict, Dictionary<string, int> opcodeDict)
        {
            int output = 0;

            output |= conditionDict[condition] << 28;
            output |= opcodeDict[opcode] << 23;

            if (opcode == "REB")
            {
                output |= register << 18;
                output |= bit << 13;
            }
            return output;
        }
    }
}
