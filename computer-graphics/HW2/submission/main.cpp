#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <string>
#include <vector>
#include <iostream>
#include <fstream>

#define M_PI 3.141592654f

unsigned int g_windowWidth = 600;
unsigned int g_windowHeight = 600;
char* g_windowName = "HW2-Rasterization";

GLFWwindow* g_window;

const int g_image_width = g_windowWidth;
const int g_image_height = g_windowHeight;

std::vector<float> g_image;

// Added for homework 2 by Max Hanson
void bresenham_small_pos_slope(int x1, int y1, int x2, int y2); // for 0 <= m <= 1
void bresenham_large_pos_slope(int x1, int y1, int x2, int y2); // for 1 < m
void bresenham_small_neg_slope(int x1, int y1, int x2, int y2); // for 0 < m <= -1
void bresenham_large_neg_slope(int x1, int y1, int x2, int y2); // for -1 < m
void draw_vertical_line(int x, int y1, int y2);
void draw_horizontal_line(int y, int x1, int x2);
void symmetrically_fill_pixel(int x0, int y0, int x, int y);

int mode; // 0 for pixels, 1 for line, 2 for circle
bool first_click_circle_flag; // set when the first click of a line or circle is completed. Only in line or circle mode.
bool first_click_line_flag;

void putPixel(int x, int y)
{
	// clamp
	if (x >= g_image_width || x < 0 || y >= g_image_height || y < 0) return;

	// write
	g_image[y* g_image_width + x] = 1.0f;
}

//-------------------------------------------------------------------------------

/*
 * Draw a line of arbitrary slope.
 * Uses Bresenham's line rasterization algorithm.
 * Determines slope of the line, may swap points, then hands to 'bresenham_x_x_slope' helper functions.
 */
void drawLine(int x1, int y1, int x2, int y2)
{
	int delta_x;
	int delta_y;
	int tmp;
	float m;

	delta_x = x2 - x1;
	delta_y = y2 - y1;

	// === handle vertical/horizontal lines ===
	if (delta_x == 0)
	{
		draw_vertical_line(x1, y1, y2);
		return;
	}
	if (delta_y == 0)
	{
		draw_horizontal_line(y1, x1, x2);
		return;
	}

	// === handle sloped lines ===

	// Swap points in certain cases. This makes lines (vectors) pointing backwards point forwards so
	// we can use the same algorithm to rasterize them.
	if (delta_x < 0)
	{
		tmp = x1;
		x1 = x2;
		x2 = tmp;
		tmp = y1;
		y1 = y2;
		y2 = tmp;
	}

	m = (float) delta_y / (float) delta_x;
	if (m >= 0 && m <= 1)
	{
		bresenham_small_pos_slope(x1, y1, x2, y2);
		return;
	}
	if (m > 1)
	{
		bresenham_large_pos_slope(x1, y1, x2, y2);
		return;
	}
	if (m <= 0 && m >= -1)
	{
		bresenham_small_neg_slope(x1, y1, x2, y2);
		return;
	}
	if (m < -1)
	{
		bresenham_large_neg_slope(x1, y1, x2, y2);
		return;
	}

}

void draw_vertical_line(int x, int y1, int y2)
{
	// for vector going up
	for (int i = y1; i <= y2; i++)
	{
		putPixel(x, i);
	}
	// for vector going down
	for (int i = y1; i >= y2; i--)
	{
		putPixel(x, i);
	}
}

void draw_horizontal_line(int y, int x1, int x2)
{
	// for vector going forward
	for (int i = x1; i <= x2; i++)
	{
		putPixel(i, y);
	}
	// for vector going backward
	for (int i = x1; i >= x2; i--)
	{
		putPixel(i, y);
	}
}

void bresenham_small_pos_slope(int x1, int y1, int x2, int y2)
{
	int x, y, e, delta_y, delta_x;

	delta_y = y2 - y1;
	delta_x = x2 - x1;
	e = 0;
	y = y1;
	for (x = x1; x < x2; x++)
	{
		putPixel(x, y);
		if (2 * (e + delta_y) < delta_x)
		{
			e = e + delta_y;
		}
		else
		{
			y++;
			e = e + delta_y - delta_x;
		}
	}
}

// TODO verify this works
void bresenham_large_pos_slope(int x1, int y1, int x2, int y2)
{
	int x, y, e, delta_y, delta_x;

	delta_y = y2 - y1;
	delta_x = x2 - x1;
	e = 0;
	x = x1;
	for (y = y1; y < y2; y++)
	{
		putPixel(x, y);
		if (2 * (e + delta_x) < delta_y)
		{
			e = e + delta_x;
		}
		else
		{
			x++;
			e = e + delta_x - delta_y;
		}
	}
}

void bresenham_small_neg_slope(int x1, int y1, int x2, int y2)
{
	int x, y, e, delta_y, delta_x;

	delta_y = y2 - y1;
	//delta_y *= -1;
	delta_x = x2 - x1;
	e = 0;
	y = y1;
	for (x = x1; x < x2; x++)
	{
		putPixel(x, y);
		if (2 * (e + delta_y) > delta_x)
		{
			e = e + delta_y;
		}
		else
		{
			y--;
			e = e + delta_y + delta_x;
		}
	}
}

void bresenham_large_neg_slope(int x1, int y1, int x2, int y2)
{
	int x, y, e, delta_y, delta_x;

	delta_y = y2 - y1;
	delta_x = x2 - x1;
	e = 0;
	x = x1;
	for (y = y1; y > y2; y--)
	{
		putPixel(x, y);
		if (2 * (e + delta_x) < delta_y)
		{
			e = e + delta_x;
		}
		else
		{
			x++;
			e = e + delta_x + delta_y;
		}
	}
}

void drawCircle(int x0, int y0, int R)
{
	int x;
	int y;
	int d;

	x = 0;
	y = R;
	d = 3 - (2 * R);

	symmetrically_fill_pixel(x0, y0, x, y);
	while (x <= y)
	{
		x++;
		if (d < 0)
		{
			d = d + (4 * x) + 6;
		}
		else
		{
			d = d + (4 * (x - y)) + 10;
			y--;
		}
		symmetrically_fill_pixel(x0, y0, x, y);
	}
}

/*
 * Fill the pixel at (x, y) symmetrically for a circle centered at (x0, y0).
 */
void symmetrically_fill_pixel(int x0, int y0, int x, int y)
{
	putPixel(x0 + x, y0 + y); // octant 1
	putPixel(x0 - x, y0 + y); // octant 4
	putPixel(x0 - x, y0 - y); // octant 5
	putPixel(x0 + x, y0 - y); // octant 8
	putPixel(x0 + y, y0 + x); // octant 2
	putPixel(x0 - y, y0 + x); // octant 3
	putPixel(x0 - y, y0 - x); // octant 6
	putPixel(x0 + y, y0 - x); // octant 7
}

struct line
{
	int x1, x2, y1, y2;
	// TODO: part of Homework Task 3
	// This function should initialize the variables of struct line
	void init()
	{
		x1 = -1;
		x2 = -1;
		y1 = -1;
		y2 = -1;
	}
	// This function should update the values of member variables and draw a line when 2 points are clicked. 
	void AddPoint(int x, int y)
	{
		if (x1 == -1 && y1 == -1)
		{
			x1 = x;
			y1 = y;
		}
		else
		{
			x2 = x;
			y2 = y;
		}
	}
};

struct circle
{
	int x0, y0, R;
	// This function should initialize the variables of struct circle
	void init()
	{
		// if elems are -1, then they haven't been set by the user
		this->x0 = -1;
		this->y0 = -1;
		this->R = -1;
	}
	// This function should update the values of member variables and draw a circle when 2 points are clicked
	// The first clicked point should be the center of the circle
	// The second clicked point should be a point on the circle
	void AddPoint(int x, int y)
	{
		if (this->x0 == -1 && this->y0 == -1)
		{
			// set the mid point
			this->x0 = x;
			this->y0 = y;
		}
		else
		{
			// set the radius
			this->R = sqrt(pow(x - this->x0, 2) + pow(y - this->y0, 2));
		}
	}
};

// Added for homeowrk 2 by Max Hanson
// these are where the mouse-click callbacks store their information about the circles/lines being drawn
struct circle in_progress_circle;
struct line in_progress_line;

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
	else if (p_key == GLFW_KEY_L && p_action == GLFW_PRESS)
	{
		std::cout << "Line mode" << std::endl;
		// This part switch on the line mode

		mode = 1;
	}
	else if (p_key == GLFW_KEY_C && p_action == GLFW_PRESS)
	{
		std::cout << "Circle mode" << std::endl;
		// This part should switch on the circle mode

		mode = 2;
	}
}

//!GLFW mouse callback
void glfwMouseButtonCallback(GLFWwindow* p_window, int p_button, int p_action, int p_mods)
{
	double xpos, ypos;
	glfwGetCursorPos(p_window, &xpos, &ypos);
	ypos = g_windowHeight - ypos;
	if (p_button == GLFW_MOUSE_BUTTON_LEFT && p_action == GLFW_PRESS) 
	{
		std::cout << "You clicked pixel: " << xpos << ", " << ypos << std::endl;
		// This part should draw appropriate figure according to the current mode

		switch (mode)
		{
		case 0:
			// put-pixel mode
			putPixel(xpos, ypos);
			break;
		case 1:
			// line mode
			if (first_click_line_flag)
			{
				in_progress_line.AddPoint(xpos, ypos);
				drawLine(in_progress_line.x1, in_progress_line.y1, in_progress_line.x2, in_progress_line.y2);
				first_click_line_flag = false;
			}
			else
			{
				in_progress_line.init();
				in_progress_line.AddPoint(xpos, ypos);
				first_click_line_flag = true;
			}
			break;
		case 2: 
			// circle mode
			if (first_click_circle_flag)
			{
				in_progress_circle.AddPoint(xpos, ypos); // add new point
				drawCircle(in_progress_circle.x0, in_progress_circle.y0, in_progress_circle.R); // draw the circle
				first_click_circle_flag = false; // reset flags
			}
			else
			{
				in_progress_circle.init(); // make a new circle.
				in_progress_circle.AddPoint(xpos, ypos); // add the first points
				first_click_circle_flag = true; // mark that the first points have been added
			}
			break;
		}
	}
}

//-------------------------------------------------------------------------------

struct color
{
	unsigned char r, g, b;
};

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
	glfwSetMouseButtonCallback(g_window, glfwMouseButtonCallback);

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
	glDrawPixels(g_image_width, g_image_height, GL_LUMINANCE, GL_FLOAT, &g_image[0]);
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

void initImage()
{
	g_image.resize(g_image_width * g_image_height);
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
			float tmp = g_image[i* g_image_width + j];
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
	fclose(fp);

	return true;
}

void drawImage()
{	
	drawLine(150, 10, 450, 10);
	drawLine(150, 310, 450, 310);
	drawLine(150, 10, 150, 310);
	drawLine(450, 10, 450, 310);
	drawLine(150, 310, 300, 410);
	drawLine(300, 410, 450, 310);

	drawCircle(500, 500, 50);
}

int main()
{
	mode = 0; // added for homework 2. Set init'l mode to pixel-putting mode.
	first_click_circle_flag = false;
	first_click_line_flag = false;

	initImage();
	drawImage();
	writeImage();

	// render loop
	initWindow();
	initGL();
	renderLoop();

	return 0;
}
