using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Move:Instruction
    {
        int value;
        int address;
        int registerFrom;
        int registerTo;
        int optionalExtra;

        public Move(string line, string inslabel, int linenumber):base(line,inslabel,linenumber)
        {
            string[] operands = line.Split("\t".ToCharArray());
            opcode = operands[1];
            if(opcode=="LDV")
            {
                value = int.Parse(operands[2]);
                registerTo = int.Parse(operands[3]);
            }
            if (opcode == "LDR")
            {
                address = int.Parse(operands[2]);
                registerTo = int.Parse(operands[3]);
            }
            if (opcode == "STR")
            {
                address = int.Parse(operands[2]);
                registerFrom = int.Parse(operands[3]);
            }
            if (opcode == "MOV")
            {
                registerFrom = int.Parse(operands[2]);
                registerTo = int.Parse(operands[3]);
                if(operands[4]!="")
                    optionalExtra = int.Parse(operands[4]);
            }
        }

        public override int Output(Dictionary<string, int> conditionDict, Dictionary<string, int> opcodeDict)
        {
            int output = 0;

            output |= conditionDict[condition]<<28;
            output |= opcodeDict[opcode]<<23;

            if (opcode == "LDV")
            {
                output |= registerTo << 18;
                output |= value;
            }
            if (opcode == "LDR")
            {
                output |= registerTo << 18;
                output |= address;
            }
            if (opcode == "STR")
            {
                output |= registerFrom << 18;
                output |= address;
            }
            if (opcode == "MOV")
            {
                output |= registerFrom << 18;
                output |= registerTo << 13;
                output |= optionalExtra << 8;
            }
            return output;
        }
    }
}
