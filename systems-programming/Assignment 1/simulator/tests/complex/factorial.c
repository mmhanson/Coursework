int fact(int f)
{
  if(f == 2)
    return 2;
  return f * fact(f-1);
}

int main()
{
  int input;
  scanf(&input);
  printf("%d\n", fact(input0));
}
