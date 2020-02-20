/*******************************************
 * Solutions for the CS:APP Performance Lab
 ********************************************/

#include <stdio.h>
#include <stdlib.h>
#include "defs.h"

/* 
 * Please fill in the following student struct 
 */
student_t student = {
  "Max M. Hanson",     /* Full name */
  "u0985911@utah.edu",  /* Email address */
};

/***************
 * COMPLEX KERNEL
 ***************/

/******************************************************
 * Your different versions of the complex kernel go here
 ******************************************************/

/* 
 * naive_complex - The naive baseline version of complex 
 */
char naive_complex_descr[] = "naive_complex: Naive baseline implementation";
void naive_complex(int dim, pixel *src, pixel *dest)
{
  int i, j;

  for(i = 0; i < dim; i++)
    for(j = 0; j < dim; j++)
    {

      dest[RIDX(dim - j - 1, dim - i - 1, dim)].red = ((int)src[RIDX(i, j, dim)].red +
						      (int)src[RIDX(i, j, dim)].green +
						      (int)src[RIDX(i, j, dim)].blue) / 3;
      
      dest[RIDX(dim - j - 1, dim - i - 1, dim)].green = ((int)src[RIDX(i, j, dim)].red +
							(int)src[RIDX(i, j, dim)].green +
							(int)src[RIDX(i, j, dim)].blue) / 3;
      
      dest[RIDX(dim - j - 1, dim - i - 1, dim)].blue = ((int)src[RIDX(i, j, dim)].red +
						       (int)src[RIDX(i, j, dim)].green +
						       (int)src[RIDX(i, j, dim)].blue) / 3;

    }
}

/* 
 * Transforms the image with blocking.
 */
char complex_descr[] = "complex: Current working version";
void complex(int dim, pixel *src, pixel *dest)
{
    int i, j, k, l;
    int avg;
    const int blocksize = 16; /*  dim assumed to be multiple of 32  */

    for (i = 0; i < dim; i += blocksize)
    {
        for (j = 0; j < dim; j += blocksize)
        {
            /*  transpose the square block with top-left at (i, j)  */
            for (k = i; k < i + blocksize; k++)
            {
                for (l = j; l < j + blocksize; l++)
                {
                    /*  make each pixel grayscale by averaging their colors  */
                    avg = ((int)src[RIDX(k, l, dim)].red +
                           (int)src[RIDX(k, l, dim)].green +
                           (int)src[RIDX(k, l, dim)].blue) / 3;
                    dest[RIDX(dim - l - 1, dim - k - 1, dim)].red = avg;
                    dest[RIDX(dim - l - 1, dim - k - 1, dim)].green = avg;
                    dest[RIDX(dim - l - 1, dim - k - 1, dim)].blue = avg;
                }
            }
        }
    }
}

/*********************************************************************
 * register_complex_functions - Register all of your different versions
 *     of the complex kernel with the driver by calling the
 *     add_complex_function() for each test function. When you run the
 *     driver program, it will test and report the performance of each
 *     registered test function.  
 *********************************************************************/

void register_complex_functions() {
  add_complex_function(&complex, complex_descr);
  add_complex_function(&naive_complex, naive_complex_descr);
}


/***************
 * MOTION KERNEL
 **************/

/***************************************************************
 * Various helper functions for the motion kernel
 * You may modify these or add new ones any way you like.
 **************************************************************/


/* 
 * weighted_combo - Returns new pixel value at (i,j) 
 */
static pixel weighted_combo(int dim, int i, int j, pixel *src) 
{
  int ii, jj;
  pixel current_pixel;

  int red, green, blue;
  red = green = blue = 0;

  int num_neighbors = 0;
  for(ii=0; ii < 3; ii++)
    for(jj=0; jj < 3; jj++) 
      if ((i + ii < dim) && (j + jj < dim)) 
      {
    	  num_neighbors++;
	      red += (int) src[RIDX(i+ii,j+jj,dim)].red;
	      green += (int) src[RIDX(i+ii,j+jj,dim)].green;
	      blue += (int) src[RIDX(i+ii,j+jj,dim)].blue;
      }
  
  current_pixel.red = (unsigned short) (red / num_neighbors);
  current_pixel.green = (unsigned short) (green / num_neighbors);
  current_pixel.blue = (unsigned short) (blue / num_neighbors);
  
  return current_pixel;
}

/* 
 * weighted_combo_fast - like weighted combo... but fast
 *
 * Doesn't check bounds or use loops at all. Also writes result directly
 * out to the destination instead of returning it.
 *
 * @dst: pointer to pixel to write average out to
 */
__attribute__((always_inline)) static void weighted_combo_fast(int dim, int i, int j, pixel *src, pixel* dst) 
{
    int red;
    int green;
    int blue;
    const int num_neighbors = 9;

    red = 0;
    green = 0;
    blue = 0;

    /*  i offset = 0  */
    red += (int) src[RIDX(i + 0, j + 0, dim)].red;
    green += (int) src[RIDX(i + 0, j + 0, dim)].green;
    blue += (int) src[RIDX(i + 0, j + 0, dim)].blue;

    red += (int) src[RIDX(i + 0, j + 1, dim)].red;
    green += (int) src[RIDX(i + 0, j + 1, dim)].green;
    blue += (int) src[RIDX(i + 0, j + 1, dim)].blue;

    red += (int) src[RIDX(i + 0, j + 2, dim)].red;
    green += (int) src[RIDX(i + 0, j + 2, dim)].green;
    blue += (int) src[RIDX(i + 0, j + 2, dim)].blue;

    /*  i offset = 1  */
    red += (int) src[RIDX(i + 1, j + 0, dim)].red;
    green += (int) src[RIDX(i + 1, j + 0, dim)].green;
    blue += (int) src[RIDX(i + 1, j + 0, dim)].blue;

    red += (int) src[RIDX(i + 1, j + 1, dim)].red;
    green += (int) src[RIDX(i + 1, j + 1, dim)].green;
    blue += (int) src[RIDX(i + 1, j + 1, dim)].blue;

    red += (int) src[RIDX(i + 1, j + 2, dim)].red;
    green += (int) src[RIDX(i + 1, j + 2, dim)].green;
    blue += (int) src[RIDX(i + 1, j + 2, dim)].blue;

    /*  i offset = 2  */
    red += (int) src[RIDX(i + 2, j + 0, dim)].red;
    green += (int) src[RIDX(i + 2, j + 0, dim)].green;
    blue += (int) src[RIDX(i + 2, j + 0, dim)].blue;

    red += (int) src[RIDX(i + 2, j + 1, dim)].red;
    green += (int) src[RIDX(i + 2, j + 1, dim)].green;
    blue += (int) src[RIDX(i + 2, j + 1, dim)].blue;

    red += (int) src[RIDX(i + 2, j + 2, dim)].red;
    green += (int) src[RIDX(i + 2, j + 2, dim)].green;
    blue += (int) src[RIDX(i + 2, j + 2, dim)].blue;
  
    dst->red = (unsigned short) (red / num_neighbors);
    dst->green = (unsigned short) (green / num_neighbors);
    dst->blue = (unsigned short) (blue / num_neighbors);
  
    /*return current_pixel;*/
}


/******************************************************
 * Your different versions of the motion kernel go here
 ******************************************************/


/*
 * naive_motion - The naive baseline version of motion 
 */
char naive_motion_descr[] = "naive_motion: Naive baseline implementation";
void naive_motion(int dim, pixel *src, pixel *dst) 
{
  int i, j;
    
  for (i = 0; i < dim; i++)
    for (j = 0; j < dim; j++)
      dst[RIDX(i, j, dim)] = weighted_combo(dim, i, j, src);
}

/*
 * motion - Your current working version of motion. 
 * Has two version of the blur functions, one that checks bounds and one that doesn't.
 */
char motion_descr[] = "motion: Current working version. Separated blur fxns";
void motion(int dim, pixel *src, pixel *dst) 
{
    int i;
    int j;
    
    /*  use quicker fxn for non-edge cases  */
    for (i = 0; i < (dim - 2); i++)
    {
        for (j = 0; j < (dim - 2); j++)
        {
            weighted_combo_fast(dim, i, j, src, &dst[RIDX(i, j, dim)]);
        }
    }

    /*  cleanup loops use bound-checking fxn for the right and bottom edges  */
    /*  i needs checking  */
    for (i = dim - 2; i < dim; i++)
    {
        for (j = 0; j < dim; j++)
        {
            dst[RIDX(i, j, dim)] = weighted_combo(dim, i, j, src);
        }
    }

    /*  j needs checking  */
    for (i = 0; i < dim; i++)
    {
        for (j = dim - 2; j < dim; j++)
        {
            dst[RIDX(i, j, dim)] = weighted_combo(dim, i, j, src);
        }
    }
}

/********************************************************************* 
 * register_motion_functions - Register all of your different versions
 *     of the motion kernel with the driver by calling the
 *     add_motion_function() for each test function.  When you run the
 *     driver program, it will test and report the performance of each
 *     registered test function.  
 *********************************************************************/

void register_motion_functions() {
  add_motion_function(&motion, motion_descr);
  add_motion_function(&naive_motion, naive_motion_descr);
}
