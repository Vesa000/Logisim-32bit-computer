# Logisim 32bit computer
This is a 32bit RISC CPU with 3 stage pipeline.
<p align="center">
<img align="center" src="https://i.imgur.com/KsZ9LXr.png" alt="msw overview">
</p>

### Architecture

The CPU is divided into 5 parts:
- Fetch - reads the next instruction and passes it to Execute
- Execute - according to the instruction reads registers/ram and handles jumps
- ALU - Arithmetic Logic Unit manages arithmetic calculations.
- Store - Stores the values given to it from Execute Or ALU to the register/ram address given by Execute
- Registers - 32 registers that hold 32 bit values

### Machine instructions
Instructions follow the format below:
<p align="center">
<img align="center" src="https://i.imgur.com/tQzJ19d.png">
</p>
 - Bits 28-31 tell the Execute Under witch condition the instruction should be executed
 - Bits 23-27 tell witch instruction should be executed
 - Bits 0-22 Hold instruction specific data for the execute step.

#### Conditions

Conditions tell under witch condition the instruction should be executed.

|Number|Instruction|Behavior|
|------|-----------|--------|
|0|ALW|Always execute|
|1|EQ|Execute when comparison returned equals|
|2|NEQ|Execute when comparison returned not equals|
|3|GT|Execute when comparison returned greater than|
|4|GEQ|Execute when comparison returned greater or equals|
|5|LT|Execute when comparison returned less than|
|6|LEQ|Execute when comparison returned less or equals|
|7|TRU|Execute when REB returned 1|
|8|FAL|Execute when REB returned 0|
|15|NOP|Do not execute|

### Special Registers
- Status register holds values from comparisons and peripheral devices
- Keyboard register holds next character from keyboard and advances when red
- TTY register is used to write to TTY screen 

### Assembly Instructions

|Instruction|Usage|Behaviour|
|-----------|-----|---------|
|LDV|LDV 1 2|Load value 1 to register 2|
|LDR|LDR 1 2|Load ram address 1 to register 2|
|STR|STR 1 2|Store register 1 to ram address 2|
|MOV|MOV 1 2|Move value from register 1 to register 2|
|JMP|JMP LABEL|Jump to label|
|BRA|BRA LABEL|Jump to label and store Program Counter to stack|
|RET|RET| Load value from stack to PC|
|ADD|ADD 1 2 3|Reg 3 = reg1 + reg 2|
|SUB|SUB 1 2 3|Reg 3 = reg1 - reg 2|
|MUL|MUL 1 2 3|Reg 3 = reg1 * reg 2|
|DIV|DIV 1 2 3|Reg 3 = reg1 / reg 2|
|CMP|CMP 1 2|Compare reg 1 to reg 2 signed|
|CMPU|CMPU 1 2|Compare reg 1 to reg 2 unsigned|
|SHL|SHL 1 2 3|reg 3 = reg1 << 2|
|SHR|SHR 1 2 3|reg 3 = reg1 >> 2|
|AND|AND 1 2 3|reg 3 = reg1 & 2|
|OR|OR 1 2 3|reg 3 = reg1 | 2|
|INV|INV 1 2|reg 2 = !reg 1|
|XOR|XOR 1 2 3|reg 3 = reg1 XOR 2|
|NAND|NAND 1 2 3|reg 3 = reg1 NAND 2|
|NOR|NOR 1 2 3|reg 3 = reg1 NOR 2|
|XNOR|XNOR 1 2 3|reg 3 = reg1 XNOR 2|
|REB|REB 1 2|Read bit 2 from register 1|
|NOP|NOP|Do nothing|
|HLT|HLT|Stop|

### Example code
Following code writes Fibonacci sequence to the TTY

```
	LDV	0	1		//i
	LDV	1	2		//j
	LDV	10	10		//ENTER to reg 10
FIBONACCI:
	ADD	1	2	3	//k=i+j
	MOV	2	1		//i=j
	MOV	3	2		//j=k
	MOV	2	30	2	//Write j to screen as number
WAIT:
	REB	31	5		
TRU	JMP	WAIT		//Wait until TTY is ready
	MOV	10	30	0	//Enter
	JMP	FIBONACCI
```