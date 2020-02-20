#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <cmath>

#define M_PI 3.141592654f

#define WAV_FILE "data/train.wav"

unsigned int g_windowWidth = 600;
unsigned int g_windowHeight = 600;
char* g_windowName = "HW1-Transform-Coding-Audio";

GLFWwindow* g_window;
bool g_draw_origin = true;

// WAVE PCM sound file format 
typedef struct header_file
{
	char chunk_id[4];
	int chunk_size;
	char format[4];
	char subchunk1_id[4];
	int subchunk1_size;
	short int audio_format;
	short int num_channels;
	int sample_rate;            // sample_rate denotes the sampling rate.
	int byte_rate;
	short int block_align;
	short int bits_per_sample;
	char subchunk2_id[4];
	int subchunk2_size;         // subchunk2_size denotes the number of samples.
} header;

header_file* g_wav_header; 

// wav data
float* g_wav_data;
float* g_compress_wav_data;
int g_wav_size;


/*
 * Compute the DCT coefficients (C) from a signal (A) both in R^size.
 * Completed by Max Hanson, August 2019.
*/
void DCT(const float* A, float* C, int size)
{
	// This algorithm is based directly from the DCT formula in the assignment pdf.
	// A: f, C: F
	for (int u = 0; u < size; u++)
	{
		float sum = 0;
		for (int i = 0; i < size; i++)
		{
			float cos_interior = ((2 * i + 1) * u * M_PI) / (2 * size);
			float cos = std::cos(cos_interior);
			float product = cos * A[i];

			sum += product;
		}

		C[u] = sum * 0.36;
	}
}

/*
 * Zero out elements of vector C with indices greater than m.
 * Completed by Max Hanson, August 2019.
*/
void compress(float* C, int size, int m)
{
	for (int C_idx = m; C_idx < size; C_idx++)
	{
		C[C_idx] = 0;
	}

}

/*
 * Decompress a vector (C) into a vector (B) using the inverse-DCT.
 * Completed by Max Hanson, August 2019.
*/
void inverseDCT(const float* C, float* B, int size)
{
	// This algorithm is based directly from the inverse-DCT formula in the assignment pdf.
	// C: F, B: f.
	for (int i = 0; i < size; i++)
	{
		float sum = 0; // Tracks the sum of each calc in loop below
		for (int u = 0; u < size; u++)
		{
			float cos_interior = ((2 * i + 1) * u * M_PI) / (2 * size);
			float cos = std::cos(cos_interior);
			float product = cos * C[u];

			sum += product;
		}

		B[i] = sum * 0.36;
	}
}
void processBlock(const float* A, float* B, int m)
{
	const int size = 8;
	float* C = new float[size];
	DCT(A, C, size);
	compress(C, size, m);
	inverseDCT(C, B, size);
	delete[] C;
}
void processWAVSignal()
{
	float *A, *B;

	const int m = 1; // TODO: change the parameter m as discussed in Homework Task 1
	
	// processing data by 8
	for (int i = 0; i < g_wav_size; i += 8)
	{
		A = g_wav_data + i;
		B = g_compress_wav_data + i;
		processBlock(A, B, m);
	}
}

void glfwErrorCallback(int error, const char* description)
{
	std::cerr << "GLFW Error " << error << ": " << description << std::endl;
	exit(1);
}

void glfwKeyCallback(GLFWwindow* p_window, int p_key, int p_scancode, int p_action, int p_mods)
{
	if (p_key == GLFW_KEY_ESCAPE && p_action == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(g_window, GL_TRUE);
	}
	else if (p_action == GLFW_PRESS)
	{
		switch (p_key)
		{
		case 49:	// press '1'
			g_draw_origin = true;
			break;
		case 50:	// press '2'
			g_draw_origin = false;
			break;
		default:
			break;
		}
	}
}

void initWindow()
{
	// initialize GLFW
	glfwSetErrorCallback(glfwErrorCallback);
	if (!glfwInit())
	{
		std::cerr << "GLFW Error: Could not initialize GLFW library" << std::endl;
		exit(1);
	}

	g_window = glfwCreateWindow(g_windowWidth, g_windowHeight, g_windowName, NULL, NULL);
	if (!g_window)
	{
		glfwTerminate();
		std::cerr << "GLFW Error: Could not initialize window" << std::endl;
		exit(1);
	}

	// callbacks
	glfwSetKeyCallback(g_window, glfwKeyCallback);

	// Make the window's context current
	glfwMakeContextCurrent(g_window);

	// turn on VSYNC
	glfwSwapInterval(1);
}

void initGL()
{
	glClearColor(1.f, 1.f, 1.f, 1.0f);
}

void render()
{
	glColor3f(1.0, 0.0, 0.0);
	glBegin(GL_LINE_STRIP);
	if (g_draw_origin)
	{
		for (int i = 0; i < g_wav_size; i++)
		{
			glVertex2f(-(0.5f - i / float(g_wav_size)) * 2.0f, float(g_wav_data[i]));
		}
	}
	else
	{
		for (int i = 0; i < g_wav_size; i++)
		{
			glVertex2f(-(0.5f - i / float(g_wav_size)) * 2.0f, float(g_compress_wav_data[i]));
		}
	}
	glEnd();	
}

void renderLoop()
{
	while (!glfwWindowShouldClose(g_window))
	{
		// clear buffers
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		render();

		// Swap front and back buffers
		glfwSwapBuffers(g_window);

		// Poll for and process events
		glfwPollEvents();
	}
}

void writeWAVFile()
{
	FILE * outfile = fopen("data/out.wav", "wb");	// Open wave file in read mode

	const int BUFSIZE = 256;						// BUFSIZE can be changed according to the frame size required (eg:512)
	short int buff16[BUFSIZE];						// short int used for 16 bit as input data format is 16 bit PCM audio

	int nb;											// variable storing number of bytes returned
	fwrite(g_wav_header, 1, sizeof(*g_wav_header), outfile);

	int dataSize = 0;
	int size = g_wav_size * 2;
	while (size > 0)
	{
		if (size > BUFSIZE)
		{
			nb = BUFSIZE;
			size -= BUFSIZE;
		}
		else
		{
			nb = size;
			size = 0;
		}
		// Incrementing > of frame	
		for (int i = 0; i < nb / 2; i++) // nb = 256 (frame size)			
		{
			buff16[i] = short int(g_compress_wav_data[dataSize] * 32768.0);
			dataSize++;
		}

		for (int i = nb / 2; i < nb; i++) buff16[i] = -13108;	// don't change this value

		fwrite(buff16, 1, nb, outfile);          // Writing read data into output file
	}
}

void loadWAVFile()
{
	FILE * infile = fopen(WAV_FILE, "rb");		// Open wave file in read mode

	const int BUFSIZE = 256;					// BUFSIZE can be changed according to the frame size required (eg:512)
	int count = 0;								// For counting number of frames in wave file.
	short int buff16[BUFSIZE];					// short int used for 16 bit as input data format is 16 bit PCM audio

	g_wav_header = (header_file*)malloc(sizeof(header));   // header_p points to a header struct that contains the wave file metadata fields

	int nb;										// variable storing number of bytes returned

	if (infile)
	{
		fread(g_wav_header, 1, sizeof(header), infile);

		std::cout << " Size of Header file is " << sizeof(*g_wav_header) << " bytes" << std::endl;
		std::cout << " Sampling rate of the input wave file is " << g_wav_header->sample_rate << " Hz" << std::endl;
		std::cout << " Number of samples in wave file are " << g_wav_header->subchunk2_size << " samples" << std::endl;
		std::cout << " The number of channels of the file is " << g_wav_header->num_channels << " channels" << std::endl;

		g_wav_size = 0;

		g_wav_data = new float[g_wav_header->subchunk2_size/2];
		g_compress_wav_data = new float[g_wav_header->subchunk2_size / 2];
		while ((nb = (int)fread(buff16, 1, BUFSIZE, infile)) > 0)
		{
			// Reading data in chunks of BUFSIZE
			count++;

			// Incrementing > of frame	
			for (int i = 0; i < nb / 2; i++) // nb = 256 (frame size)			
			{
				g_wav_data[g_wav_size] = buff16[i] / 32768.0f;
				g_compress_wav_data[g_wav_size] = g_wav_data[g_wav_size];
				g_wav_size++;
			}
		}

		std::cout << " Number of frames in the input wave file are " << count << std::endl;
		std::cout << " Size of data " << g_wav_size << std::endl;
	}
}

int main()
{
	loadWAVFile();

	processWAVSignal();

	writeWAVFile();

	// render loop
	initWindow();
	initGL();
	renderLoop();

	return 0;
}
