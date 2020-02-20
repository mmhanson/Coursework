/*
 * Author: Daniel Kopta
 * Updated by: Erin Parker
 * CS 4400, University of Utah
 *
 * Simulator handout
 * A simple x86-like processor simulator.
 * Read in a binary file that encodes instructions to execute.
 * Simulate a processor by executing instructions one at a time and appropriately 
 * updating register and memory contents.
 *
 * Some code and pseudo code has been provided as a starting point.
 *
 * Completed by: Max Hanson (u0985911)
*/

#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <unistd.h>
#include "instruction.h"

// Forward declarations for helper functions
unsigned int get_file_size(int file_descriptor);
unsigned int* load_file(int file_descriptor, unsigned int size);
instruction_t* decode_instructions(unsigned int* bytes, unsigned int num_instructions);
unsigned int execute_instruction(unsigned int program_counter, instruction_t* instructions, 
int* registers, unsigned char* memory);
void print_instructions(instruction_t* instructions, unsigned int num_instructions);
void error_exit(const char* message);
// added by Max Hanson
int int_from_memory(const unsigned char* memory, int addr);
void int_to_memory(unsigned char* memory, int addr, int num);

// 17 registers
#define NUM_REGS 17
// 1024-byte stack
#define STACK_SIZE 1024

int main(int argc, char** argv)
{
  // Make sure we have enough arguments
  if(argc < 2)
    error_exit("must provide an argument specifying a binary file to execute");

  // Open the binary file
  int file_descriptor = open(argv[1], O_RDONLY);
  if (file_descriptor == -1) 
    error_exit("unable to open input file");

  // Get the size of the file
  unsigned int file_size = get_file_size(file_descriptor);
  // Make sure the file size is a multiple of 4 bytes
  // since machine code instructions are 4 bytes each
  if(file_size % 4 != 0)
    error_exit("invalid input file");

  // Load the file into memory
  // We use an unsigned int array to represent the raw bytes
  // We could use any 4-byte integer type
  unsigned int* instruction_bytes = load_file(file_descriptor, file_size);
  close(file_descriptor);

  unsigned int num_instructions = file_size / 4;


  /****************************************/
  /**** Begin code to modify/implement ****/
  /****************************************/

  // Allocate and decode instructions (left for you to fill in)
  instruction_t* instructions = decode_instructions(instruction_bytes, num_instructions);

  // Optionally print the decoded instructions for debugging
  // Will not work until you implement decode_instructions
  // Do not call this function in your submitted final version
  //print_instructions(instructions, num_instructions);


  // Once you have completed Part 1 (decoding instructions), uncomment the below block

  // Allocate and initialize registers to zero (except esp = 1024)
  int* registers = (int*)malloc(sizeof(int) * NUM_REGS);
  int reg_idx = 0;
  for (reg_idx = 0; reg_idx < NUM_REGS; reg_idx++)
  {
    registers[reg_idx] = 0;
  }
  registers[6] = 1024; // set esp to 1024

  // Stack memory is byte-addressed, so it must be a 1-byte type
  unsigned char* memory = (unsigned char*)malloc(sizeof(char) * STACK_SIZE);

  // Run the simulation
  unsigned int program_counter = 0;

  // program_counter is a byte address, so we must multiply num_instructions by 4 
  // to get the address past the last instruction
  while(program_counter != num_instructions * 4)
  {
    program_counter = execute_instruction(program_counter, instructions, registers, memory);
  }
  
  return 0;
}

/*
 * Decodes the array of raw instruction bytes into an array of instruction_t
 * Each raw instruction is encoded as a 4-byte unsigned int
*/
instruction_t* decode_instructions(unsigned int* bytes, unsigned int num_instructions)
{
  instruction_t *insts;
  unsigned int raw_inst;
  instruction_t *inst_cursor;
  int idx;

  insts = malloc(sizeof(instruction_t) * num_instructions);

  for(idx = 0; idx < num_instructions; idx++)
  {
    inst_cursor = &insts[idx];
    raw_inst = *bytes;

    inst_cursor->opcode = (raw_inst >> 27) & 0x1F;
    inst_cursor->first_register = (raw_inst >> 22) & 0x1F;
    inst_cursor->second_register = (raw_inst >> 17) & 0x1F;
    inst_cursor->immediate = raw_inst & 0xFFFF;

    bytes++;
  }
    
  return insts;
}


/*
 * Executes a single instruction and returns the next program counter
*/
unsigned int execute_instruction(unsigned int program_counter, instruction_t* instructions, int* registers, unsigned char* memory)
{
  // program_counter is a byte address, but instructions are 4 bytes each
  // divide by 4 to get the index into the instructions array
  instruction_t instr = instructions[program_counter / 4];
  
  switch(instr.opcode)
  {
  case subl:
    registers[instr.first_register] = registers[instr.first_register] - instr.immediate;
    break;
  case addl_reg_reg:
    registers[instr.second_register] = registers[instr.first_register] + registers[instr.second_register];
    break;
  case addl_imm_reg:
    registers[instr.first_register] = registers[instr.first_register] + instr.immediate;
    break;
  case imull:
    registers[instr.second_register] = registers[instr.first_register] * registers[instr.second_register];
    break;
  case movl_reg_reg:
    registers[instr.second_register] = registers[instr.first_register];
    break;
  case movl_deref_reg:
    registers[instr.second_register] = int_from_memory(
        memory,
        instr.immediate + registers[instr.first_register]);
    break;
  case movl_reg_deref:
    int_to_memory(
        memory,
        instr.immediate + registers[instr.second_register],
        registers[instr.first_register]);
    break;
  case movl_imm_reg:
    registers[instr.first_register] = instr.immediate;
    break;
  case printr:
  printf("%d (0x%x)\n", registers[instr.first_register], registers[instr.first_register]);
  break;
  case readr:
  scanf("%d", &(registers[instr.first_register]));
  break;
  case jmp:
    // immeidate is offset from next instruction addr
    return program_counter + 4 + instr.immediate;
    break;
  case jbe:
    if ((registers[16] >> 6 & 0x1) || (registers[16] & 0x1))
    {
      return program_counter + 4 + instr.immediate;
    }
    break;
  case je:
    if (registers[16] >> 6 & 0x1)
    {
      return program_counter + 4 + instr.immediate;
    }
    break;
  case jge:
    // not(SF xor OF)
    if (!( (registers[16] >> 7 & 0x1) ^ (registers[16] >> 11 & 0x1)) )
    {
      return program_counter + 4 + instr.immediate;
    }
    break;
  case jle:
  {
    // (SF xor OF) or ZF
    int zf = registers[16] >> 6 & 0x1;
    int sf = registers[16] >> 7 & 0x1;
    int of = registers[16] >> 11 & 0x1;
    if ((sf ^ of) || zf)
    {
      return program_counter + 4 + instr.immediate;
    }
    break;
  }
  case jl:
    if ((registers[16] >> 7 & 0x1) ^ (registers[16] >> 11 & 0x1))
    {
      return program_counter + 4 + instr.immediate;
    }
    break;
  case shrl:
    // casts to unsigned to shift negative nums without sign extension
    registers[instr.first_register] = (unsigned int)registers[instr.first_register] >> 1;
    break;
  case call:
    // note %esp = register 6
    // make room on stack, save return addr, and jump to target
    registers[6] -= 4;
    int_to_memory(memory, registers[6], program_counter + 4);
    return program_counter + 4 + instr.immediate;
    break; // just in case...
  case ret:
    // note %esp = register 6
    if (registers[6] == 1024)
    {
      exit(0);
    }
    else
    {
      program_counter = int_from_memory(memory, registers[6]);
      registers[6] += 4;
      return program_counter;
    }
    break; // just in case...
  case pushl:
    registers[6] -= 4;
    int_to_memory(memory, registers[6], registers[instr.first_register]);
    break;
  case popl:
    registers[instr.first_register] = int_from_memory(memory, registers[6]);
    registers[6] += 4;
    break;
  case cmpl:
  {
    // note %eflags = register 16
    int r1 = registers[instr.first_register];
    int r2 = registers[instr.second_register];
    registers[16] = 0; // reset flags
    // if (r2 - r1) creates:
    // unsigned overflow (CF)
    if ( ((unsigned int)r2) < ((unsigned int)r1) )
    {
      registers[16] = registers[16] | (0x1 << 0); // set 0th bit
    }
    // zero (ZF)
    if (r1 == r2)
    {
      registers[16] = registers[16] | (0x1 << 6); // set 6th bit
    }
    // a (negatively) signed result (SF)
    if ((r2 - r1) < 0)
    {
      registers[16] = registers[16] | (0x1 << 7); // set 7th bit
    }
    // signed overflow (OF) ie the result is over the maximum value
    if (r2 > 0 && r1 < 0 && (-1 * r1) > (INT32_MAX - r2))
    {
      registers[16] = registers[16] | (0x1 << 11); // set 11th bit
    }
    // signed overflow (OF) ie the result is under the minimum value
    if (r2 < 0 && r1 > 0 && (-1 * r1) < (INT32_MIN - r2))
    {
      registers[16] = registers[16] | (0x1 << 11); // set 11th bit
    }
    break;
  }
  }

  // program_counter + 4 represents the subsequent instruction
  // instructions that return a different value do so in their switch cases
  return program_counter + 4;
}

/*
 * Helper function to get 32 bit integers from memory (array of bytes).
 * @addr: must satisfy: 0 <= @addr < STACK_SIZE
 */
int int_from_memory(const unsigned char* memory, int addr)
{
    int *int_addr = (int*)&memory[addr];
    return *int_addr;
}

/*
 * Helper function to store a 32 bit integer into memory (array of bytes).
 * @addr: must satisfy: 0 <= @addr < STACK_SIZE
 * @num: 32 bit int to store in memory.
 */
void int_to_memory(unsigned char* memory, int addr, int num)
{
    int *int_addr = (int*)&memory[addr];
    *int_addr = num;
}


/*********************************************/
/****  DO NOT MODIFY THE FUNCTIONS BELOW  ****/
/*********************************************/

/*
 * Returns the file size in bytes of the file referred to by the given descriptor
*/
unsigned int get_file_size(int file_descriptor)
{
  struct stat file_stat;
  fstat(file_descriptor, &file_stat);
  return file_stat.st_size;
}

/*
 * Loads the raw bytes of a file into an array of 4-byte units
*/
unsigned int* load_file(int file_descriptor, unsigned int size)
{
  unsigned int* raw_instruction_bytes = (unsigned int*)malloc(size);
  if(raw_instruction_bytes == NULL)
    error_exit("unable to allocate memory for instruction bytes (something went really wrong)");

  int num_read = read(file_descriptor, raw_instruction_bytes, size);

  if(num_read != size)
    error_exit("unable to read file (something went really wrong)");

  return raw_instruction_bytes;
}

/*
 * Prints the opcode, register IDs, and immediate of every instruction, 
 * assuming they have been decoded into the instructions array
*/
void print_instructions(instruction_t* instructions, unsigned int num_instructions)
{
  printf("instructions: \n");
  unsigned int i;
  for(i = 0; i < num_instructions; i++)
  {
    printf("op: %d, reg1: %d, reg2: %d, imm: %d\n", 
	   instructions[i].opcode,
	   instructions[i].first_register,
	   instructions[i].second_register,
	   instructions[i].immediate);
  }
  printf("--------------\n");
}

/*
 * Prints an error and then exits the program with status 1
*/
void error_exit(const char* message)
{
  printf("Error: %s\n", message);
  exit(1);
}
