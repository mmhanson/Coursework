#include <dirent.h> 
#include <stdio.h> 
#include <iostream>

int main(void)
{
    DIR           *dirp;
    struct dirent *directory;

    dirp = opendir("spreadsheets");
    if (dirp)
    {
        while ((directory = readdir(dirp)) != NULL)
        {
	  std::cout << directory->d_name << std::endl;
          //printf("%s\n", directory->d_name);
        }

        closedir(dirp);
    }

    return(0);
}
