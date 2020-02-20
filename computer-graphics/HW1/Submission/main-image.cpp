#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <cmath>

#define M_PI 3.141592654f

unsigned int g_windowWidth = 600;
unsigned int g_windowHeight = 600;
char* g_windowName = "HW1-Transform-Coding-Image";

#define IMAGE_FILE "data/cameraman.ppm"

GLFWwindow* g_window;

int g_image_width;
int g_image_height;

std::vector<float> g_luminance_data;
std::vector<float> g_compressed_luminance_data;

struct color
{
	unsigned char r, g, b;
};

bool g_draw_origin = true;

void DCT(const float* A, float* C, int blockSize)
{
	// TODO: Homework Task 2 (see the PDF description)
	// Use std::cos
}
void compress(float* C, int blockSize, int m)
{
	// TODO: Homework Task 2 (see the PDF description)
}
void inverseDCT(const float* C, float* B, int blockSize)
{
	// TODO: Homework Task 2 (see the PDF description)
	// Use std::cos
}

void processBlock(const float* A, float* B, int m)
{
	const int blockSize = 8;
	float *C = new float[blockSize * blockSize]; // Coefficients
	DCT(A, C, blockSize);
	compress(C, blockSize, m);
	inverseDCT(C, B, blockSize);
	delete[] C;
}

void processImage(const std::vector<float> I, std::vector<float>& O, int m)
{
	int H = g_image_height;
	int W = g_image_width;

	float *A = new float[8 * 8];
	float *B = new float[8 * 8];

	for (int u = 0; u < H; u +=8) {
		for (int v = 0; v < W; v +=8) {
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					A[i * 8 + j] = I[ (u+i)*W + (v+j) ];
				}
			}
			processBlock(A, B, m);
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					O[(u + i)*W + (v + j)] = B[i * 8 + j];
				}
			}
		}
	}
	delete[] A;
	delete[] B;
}

int ReadLine(FILE *fp, int size, char *buffer)
{
	int i;
	for (i = 0; i < size; i++) {
		buffer[i] = fgetc(fp);
		if (feof(fp) || buffer[i] == '\n' || buffer[i] == '\r') {
			buffer[i] = '\0';
			return i + 1;
		}
	}
	return i;
}

//-------------------------------------------------------------------------------

bool LoadPPM(FILE *fp, int &width, int &height, std::vector<color> &data)
{
	const int bufferSize = 1024;
	char buffer[bufferSize];
	ReadLine(fp, bufferSize, buffer);
	if (buffer[0] != 'P' && buffer[1] != '6') return false;

	ReadLine(fp, bufferSize, buffer);
	while (buffer[0] == '#') ReadLine(fp, bufferSize, buffer);  // skip comments

	sscanf(buffer, "%d %d", &width, &height);

	ReadLine(fp, bufferSize, buffer);
	while (buffer[0] == '#') ReadLine(fp, bufferSize, buffer);  // skip comments

	data.resize(width*height);
	fread(data.data(), sizeof(color), width*height, fp);

	return true;
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
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	if (g_draw_origin)
		glDrawPixels(g_image_width, g_image_height, GL_LUMINANCE, GL_FLOAT, &g_luminance_data[0]);
	else
		glDrawPixels(g_image_width, g_image_height, GL_LUMINANCE, GL_FLOAT, &g_compressed_luminance_data[0]);
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

bool loadImage()
{
	std::vector<color> g_image_data;
	g_image_data.clear();
	g_image_width = 0;
	g_image_height = 0;
	FILE *fp = fopen(IMAGE_FILE, "rb");
	if (!fp) return false;

	bool success = false;
	success = LoadPPM(fp, g_image_width, g_image_height, g_image_data);

	g_luminance_data.resize(g_image_width * g_image_height);
	g_compressed_luminance_data.resize(g_image_width * g_image_height);
	for (int i = 0; i < g_image_height; i++)
	{
		for (int j = 0; j < g_image_width; j++)
		{
			// the index are not matching because of the difference between image space and OpenGl screen space
			g_luminance_data[i* g_image_width + j] = g_image_data[(g_image_height - i - 1)* g_image_width + j].r / 255.0f;
		}
	}

	g_windowWidth = g_image_width;
	g_windowHeight = g_image_height;

	return success;
}

bool writeImage()
{
	std::vector<color> tmpData;
	tmpData.resize(g_image_width * g_image_height);

	for (int i = 0; i < g_image_height; i++)
	{
		for (int j = 0; j < g_image_width; j++)
		{
			// make sure the value will not be larger than 1 or smaller than 0, which might cause problem when converting to unsigned char
			float tmp = g_compressed_luminance_data[i* g_image_width + j];
			if (tmp < 0.0f)	tmp = 0.0f;
			if (tmp > 1.0f)	tmp = 1.0f;

			tmpData[(g_image_height - i - 1)* g_image_width + j].r = unsigned char(tmp * 255.0);
			tmpData[(g_image_height - i - 1)* g_image_width + j].g = unsigned char(tmp * 255.0);
			tmpData[(g_image_height - i - 1)* g_image_width + j].b = unsigned char(tmp * 255.0);
		}
	}

	FILE *fp = fopen("data/out.ppm", "wb");
	if (!fp) return false;

	fprintf(fp, "P6\r");
	fprintf(fp, "%d %d\r", g_image_width, g_image_height);
	fprintf(fp, "255\r");
	fwrite(tmpData.data(), sizeof(color), g_image_width * g_image_height, fp);

	return true;
}

int main()
{
	loadImage();

	int m = 1;	// TODO: change the parameter m to see different image quality
	processImage(g_luminance_data, g_compressed_luminance_data, m);	

	writeImage();

	// render loop
	initWindow();
	initGL();
	renderLoop();

	return 0;
}
