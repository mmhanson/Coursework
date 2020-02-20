#include <stdio.h>

// Recursively computes log base 2
// of the input, by counting how many
// times we can divide it by 2
unsigned log2(unsigned input)
{

  // Integer math only, approximation
  if(input <= 2)
  {
    return 1;
  }

  return 1 + log2(input / 2);
}

int main()
{
  int input;
  scanf("%d", &input);
  return log2(input);
}
