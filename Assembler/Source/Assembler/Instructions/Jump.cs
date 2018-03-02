using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Jump:Instruction
    {
        string labelTo;

        public Jump(string line, string inslabel, int linenumber):base(line,inslabel,linenumber)
        {
            string[] operands = line.Split("\t".ToCharArray());

            opcode = operands[1];
            if (opcode == "JMP"||opcode=="BRA")
            {
                labelTo = operands[2];
            }
        }
        public int Output(Dictionary<string, int> conditionDict, Dictionary<string, int> opcodeDict,List<Instruction> instructions)
        {
            int output = 0;

            output |= conditionDict[condition] << 28;
            output |= opcodeDict[opcode] << 23;

            if (opcode == "JMP"|| opcode == "BRA")
            {
                int labeldest = 0;
                for (int i= 0;i<instructions.Count;i++)
                {
                    if (labelTo == instructions[i].label)
                        labeldest = i;
                }
                output |= labeldest << 0;
            }
            return output;
        }
    }
}
