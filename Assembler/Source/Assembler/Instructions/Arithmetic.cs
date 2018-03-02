using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Arithmetic : Instruction
    {
        public string operation;
        public int aluInstruction;
        public int RegisterA;
        public int RegisterB;
        public int RegisterQ;

        public Arithmetic(string line,string inslabel,int linenumber, Dictionary<string, int> aluDict) :base(line,inslabel,linenumber)
        {
            string[] operands = line.Split("\t".ToCharArray());

            aluInstruction = aluDict[operands[1]];
            operation = operands[1];
            opcode = "ALU";
            RegisterA = int.Parse(operands[2]);
            if (operation == "ADD" || operation == "SUB" || operation == "MUL" || operation == "DIV")
            {
                RegisterB = int.Parse(operands[3]);
                RegisterQ = int.Parse(operands[4]);
            }
            if (operation == "CMP" || operation == "CMPU")
            {
                RegisterB = int.Parse(operands[3]);
            }
            if (operation == "SHL" || operation == "SHR" || operation == "AND" || operation == "OR" || operation == "INV" || operation == "XOR" || operation == "NAND" || operation == "NOR" || operation == "XNOR")
            {
                RegisterQ = int.Parse(operands[3]);
            }
        }
        public override int Output(Dictionary<string, int> conditionDict, Dictionary<string, int> opcodeDict)
        {
            int output = 0;

            output |= conditionDict[condition] << 28;
            output |= opcodeDict[opcode] << 23;
            output |= aluInstruction << 0;

            //3 Register instructions
            if (operation == "ADD"|| operation == "SUB"|| operation == "MUL"|| operation == "DIV" || operation == "AND" || operation == "OR" || operation == "NOT" || operation == "XOR" || operation == "NAND" || operation == "NOR" || operation == "XNOR")
            {
                output |= RegisterA << 18;
                output |= RegisterB << 13;
                output |= RegisterQ << 8;
            }

            //2 Register instructions
            if (operation == "SHL" || operation == "SHR" || operation == "INV")
            {
                output |= RegisterA << 18;
                output |= RegisterQ << 8;
            }

            //comparisons
            if (operation == "CMP"|| operation == "CMPU")
            {
                output |= RegisterA << 18;
                output |= RegisterB << 13;
            }
            return output;
        }
    }
}
