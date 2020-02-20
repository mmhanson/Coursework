#!/bin/bash
clear
echo -e "\033[0;31mTESTER STARTED\033[0m"
for i in {1..9}
  do
     make rtest0$i > exp.out
     make test0$i > act.out
     echo -e "\033[0;31mNext test ready. Press enter to continue...\033[0m"
     read
     clear
     echo -e "\033[0;31mTEST$i EXPECTED:ACTUAL\033[0m"
     diff exp.out act.out
  done

for i in {0..7}
  do
     make rtest1$i > exp.out
     make test1$i > act.out
     echo -e "\033[0;31mNext test ready. Press enter to continue...\033[0m"
     read
     clear
     echo -e "\033[0;31mTEST1$i EXPECTED:ACTUAL\033[0m"
     diff exp.out act.out
  done
