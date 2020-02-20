#!/bin/bash

# Autor: Daniel Kopta
# Updated by: Erin Parker
# CS 4400, University of Utah
# Simulator handout
# This script runs a simulator on the provided tests

NUM_PASSED=0

if [ ! -f simulator ]
then
    echo "Please compile the simulator first"
    exit 1
fi

# put the tests in increasing order of difficulty and roughly in the order
# that they build on each other

BINARIES="tests/simple/subl.o tests/simple/addl_imm_reg.o tests/simple/movl_imm.o tests/simple/movl_reg_reg.o tests/simple/addl_reg_reg.o tests/simple/imull.o tests/simple/simple_return.o tests/simple/jmp.o tests/simple/shrl.o tests/moderate/movl_deref.o tests/moderate/movl_deref2.o tests/moderate/unaligned1.o tests/moderate/unaligned2.o tests/moderate/pushpop.o tests/moderate/callret.o tests/moderate/callret2.o tests/moderate/stack_multibyte.o tests/moderate/cmpl.o tests/moderate/je.o tests/moderate/jl.o tests/moderate/jle.o tests/moderate/jge.o tests/moderate/jbe.o tests/complex/factorial.o tests/complex/log2.o tests/complex/sort.o"

for BINARY in $BINARIES
do
    pathname=${BINARY%.*}
    testname=${BINARY##*/}
    testname=${testname%.*}
    echo "Testing $testname"

    if [ -f $pathname.in ]
    then
	./simulator $BINARY < $pathname.in > temp_output.txt
    else
	./simulator $BINARY > temp_output.txt
    fi

    if [ $? -ne 0 ]
    then
	echo "FAIL"
	echo "simulator returned non-zero exit status"
	continue
    fi

    if [ ! -f $pathname.expected ]
    then
	echo "Unable to find expected output file. Download a fresh copy of the \"tests\" directory and don't tamper with it."
	exit 1
    fi

    diff temp_output.txt $pathname.expected > /dev/null
    if [ $? -eq 0 ]
    then
	echo "PASS"
	let NUM_PASSED=NUM_PASSED+1
    else
	echo "FAIL"
	echo "Expected:"
	echo -n "\""
	cat $pathname.expected
	echo "\""
	echo "Got:"
	echo -n "\""
	cat temp_output.txt
	echo "\""
    fi
done

if [ -f temp_output.txt ]
then
    rm temp_output.txt
fi

echo "Passed $NUM_PASSED / 26 tests"
