/*
 * Author: Daniel Kopta
 * Updated by: Erin Parker
 * CS 4400, University of Utah
 *
 * Contains definitions useful for representing instructions.
 *
 * STUDENTS MAY NOT MODIFY THIS FILE
*/

#pragma once

#include <stdint.h>

/*
 * A sequence of values in ascending order representing opcodes, starting at 0
 */
enum opcodes{
  subl             = 0,
  addl_reg_reg,   // 1
  addl_imm_reg,   // 2
  imull,          // 3
  shrl,           // 4
  movl_reg_reg,   // 5
  movl_deref_reg, // 6
  movl_reg_deref, // 7
  movl_imm_reg,   // 8
  cmpl,           // 9
  je,             // 10
  jl,             // 11
  jle,            // 12
  jge,            // 13
  jbe,            // 14
  jmp,            // 15
  call,           // 16
  ret,            // 17
  pushl,          // 18
  popl,           // 19
  printr,         // 20
  readr           // 21
};



/*
 * A struct to represent an instruction for the simulator.
 * The fields in this struct should correspond to the values encoded in the binary 
 * instruction representation.
 */
typedef struct 
{
  unsigned char opcode;
  unsigned char first_register;
  unsigned char second_register;
  int16_t immediate; // 16-bit signed integer
} instruction_t;
