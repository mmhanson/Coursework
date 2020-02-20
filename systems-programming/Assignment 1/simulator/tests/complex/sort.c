#include <stdio.h>

void swap(int* arr, int i, int j)
{
  int temp = arr[i];
  arr[i] = arr[j];
  arr[j] = temp;
}

void sort(int* arr, int size)
{
  if(size < 2)
    return;
  int i;
  int j;

  for(i = 0; i < size; i++)
  {
    int min_index = i;
    for(j = i + 1; j < size; j++)
    {
      if(arr[j] < arr[min_index])
	min_index = j;
    }
    swap(arr, i, min_index);
  }
}

int main()
{
  int i;
  int a[6];

  for(i = 0; i < 6; i++)
    scanf("%d", &(a[i]));

  sort(a, 6);
  
  for(i = 0; i < 6; i++)
    printf("%d\n", a[i]);
}

