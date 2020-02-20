#include "GL/glew.h"
#include "GLFW/glfw3.h"
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <cmath>

#define M_PI 3.141592654f

unsigned int g_windowWidth = 800;
unsigned int g_windowHeight = 600;
char* g_windowName = "HW3-3D-Basics";

GLFWwindow* g_window;

// Model data
std::vector<float> g_meshVertices;
std::vector<float> g_meshNormals;
std::vector<unsigned int> g_meshIndices;
GLfloat g_modelViewMatrix[16];

// Default options
bool enablePersp = true;
bool teapotSpin = false;
bool enableDolly = false;
bool showCheckerboard = false;

// Dolly zoom options 
float fov = M_PI / 4.f;
float distance = 4.5f;

// Auxiliary math functions
float dotProduct(const float* a, const float* b)
{
	return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
}

void crossProduct(const float* a, const float* b, float* r)
{
	r[0] = a[1] * b[2] - a[2] * b[1];
	r[1] = a[2] * b[0] - a[0] * b[2];
	r[2] = a[0] * b[1] - a[1] * b[0];
}

float radianToDegree(float angleInRadian) {
	return angleInRadian * 180.f / M_PI;
}

void normalize(float* a)
{
	const float len = sqrt(a[0] * a[0] + a[1] * a[1] + a[2] * a[2]);

	a[0] /= len;
	a[1] /= len;
	a[2] /= len;
}

void computeNormals()
{
	int idx;
	int a;
	int b;
	int c;
	int n_x;
	int n_y;
	int n_z;
	std::vector<int> vertex_triangle_count;
	float abs_normal;
	float triangle_count;
	float b_minus_a[3];
	float c_minus_a[3];
	float normal[3];

	g_meshNormals.resize(g_meshVertices.size());
	vertex_triangle_count.resize(g_meshNormals.size() / 3);

	// Compute the surface normal for each triangle and add it to the normal for each vertex of the triangle,
	// keeping count of how many surface normals have been added to each vertex.
	// The algorithm for computing the normal vector based on the formula: n = normalize((b-a) x (c-a))
	// this formula is taken from this webpage: https://www.khronos.org/opengl/wiki/Calculating_a_Surface_Normal.
	for (idx = 0; idx < g_meshIndices.size() / 3; idx++)
	{
		a = g_meshIndices.at((idx * 3) + 0) * 3; // indices of the verticies composing the triangle
		b = g_meshIndices.at((idx * 3) + 1) * 3; // each vertex index points to the x-coord of the vertex in g_meshVertices
		c = g_meshIndices.at((idx * 3) + 2) * 3;

		// compute vectors (b-a) and (c-a)
		b_minus_a[0] = g_meshVertices.at(b + 0) - g_meshVertices.at(a + 0);
		b_minus_a[1] = g_meshVertices.at(b + 1) - g_meshVertices.at(a + 1);
		b_minus_a[2] = g_meshVertices.at(b + 2) - g_meshVertices.at(a + 2);
		c_minus_a[0] = g_meshVertices.at(c + 0) - g_meshVertices.at(a + 0);
		c_minus_a[1] = g_meshVertices.at(c + 1) - g_meshVertices.at(a + 1);
		c_minus_a[2] = g_meshVertices.at(c + 2) - g_meshVertices.at(a + 2);

		// compute cross-product
		normal[0] = (float)(b_minus_a[1] * c_minus_a[2]) - (float)(b_minus_a[2] * c_minus_a[1]);
		normal[1] = (float)(b_minus_a[2] * c_minus_a[0]) - (float)(b_minus_a[0] * c_minus_a[2]);
		normal[2] = (float)(b_minus_a[0] * c_minus_a[1]) - (float)(b_minus_a[1] * c_minus_a[0]);

		// normalize the cross-product to get the final normal vector
		abs_normal = sqrt(pow(normal[0], 2) + pow(normal[1], 2) + pow(normal[2], 2));
		normal[0] = normal[0] / abs_normal;
		normal[1] = normal[1] / abs_normal;
		normal[2] = normal[2] / abs_normal;

		// add normal vector of the triangle to the normal vectors of all vertexes of triangle
		g_meshNormals.at(a + 0) += normal[0];
		g_meshNormals.at(a + 1) += normal[1];
		g_meshNormals.at(a + 2) += normal[2];
		g_meshNormals.at(b + 0) += normal[0];
		g_meshNormals.at(b + 1) += normal[1];
		g_meshNormals.at(b + 2) += normal[2];
		g_meshNormals.at(c + 0) += normal[0];
		g_meshNormals.at(c + 1) += normal[1];
		g_meshNormals.at(c + 2) += normal[2];

		// record the additions
		vertex_triangle_count.at(a / 3) += 1;
		vertex_triangle_count.at(b / 3) += 1;
		vertex_triangle_count.at(c / 3) += 1;
	}

	// average the normal vector for each vertex
	for (idx = 0; idx < vertex_triangle_count.size(); idx++)
	{
		n_x = idx * 3; // index of x-coord of normal vector in g_meshNormals
		n_y = n_x + 1;
		n_z = n_y + 1;

		triangle_count = vertex_triangle_count.at(idx);
		g_meshNormals.at(n_x) = g_meshNormals.at(n_x) / triangle_count;
		g_meshNormals.at(n_y) = g_meshNormals.at(n_y) / triangle_count;
		g_meshNormals.at(n_z) = g_meshNormals.at(n_z) / triangle_count;
	}
}

void loadObj(std::string p_path)
{
	std::ifstream nfile;
	nfile.open(p_path);
	std::string s;

	while (nfile >> s)
	{
		if (s.compare("v") == 0)
		{
			float x, y, z;
			nfile >> x >> y >> z;
			g_meshVertices.push_back(x);
			g_meshVertices.push_back(y);
			g_meshVertices.push_back(z);
		}		
		else if (s.compare("f") == 0)
		{
			std::string sa, sb, sc;
			unsigned int a, b, c;
			nfile >> sa >> sb >> sc;

			a = std::stoi(sa);
			b = std::stoi(sb);
			c = std::stoi(sc);

			g_meshIndices.push_back(a - 1);
			g_meshIndices.push_back(b - 1);
			g_meshIndices.push_back(c - 1);
		}
		else
		{
			std::getline(nfile, s);
		}
	}

	computeNormals();

	std::cout << p_path << " loaded. Vertices: " << g_meshVertices.size() / 3 << " Triangles: " << g_meshIndices.size() / 3 << std::endl;
}

double getTime()
{
	return glfwGetTime();
}

void glfwErrorCallback(int error, const char* description)
{
	std::cerr << "GLFW Error " << error << ": " << description << std::endl;
	exit(1);
}

void togglePerspective()
{
	const float halfWidth = distance * tan(fov / 2);
	const float zNear = 1.0;
	const float zFar = 40.0;

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	// Perspective Projection
	if (enablePersp)
	{
		// Dolly zoom computation
		if (enableDolly) {
			fov = fmod(getTime(), 1.4073f) + 0.093133f; // stays between 0.093133 and 1.50043
			distance = halfWidth / tan(fov / 2); // stays between 40.0 and 2.0
		}
		else
		{
			// restore distance and FOV to defaults
			fov = M_PI / 4.f;
			distance = 4.5f;
		}

		float fovInDegree = radianToDegree(fov);
		gluPerspective(fovInDegree, (GLfloat)g_windowWidth / (GLfloat)g_windowHeight, zNear, zFar);
	}
	// Othogonal Projection
	else
	{
		// Scale down the object for a better view in orthographic projection
		glScalef(0.5, 0.5, 0.5);

		// To find these parameters, I just tried many values until the edges of the teapot stayed around the same
		// spot when going between orthogonal and perspective projection modes.
		glOrtho(-1.25f, 1.25f, -0.9f, 0.9f, zNear, zFar);
	}
}

void glfwKeyCallback(GLFWwindow* p_window, int p_key, int p_scancode, int p_action, int p_mods)
{
	if (p_key == GLFW_KEY_ESCAPE && p_action == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(g_window, GL_TRUE);
	}
	if (p_key == GLFW_KEY_P && p_action == GLFW_PRESS) {

		// Perspective Projection
		enablePersp = true;
		togglePerspective();
		std::cout << "Perspective activated\n";

	}
	if (p_key == GLFW_KEY_O && p_action == GLFW_PRESS) {

		// Orthographic Projection
		enablePersp = false;
		togglePerspective();
		std::cout << "Orthogonal activated\n";

	}
	if (p_key == GLFW_KEY_S && p_action == GLFW_PRESS) {

		// Toggle Spinning
		if (!teapotSpin) {
			std::cout << "Teapot spinning on\n";
		}
		else {
			std::cout << "Teapot spinning off\n";
		}
		teapotSpin = !teapotSpin;
	}
	if (p_key == GLFW_KEY_D && p_action == GLFW_PRESS) {

		// Toggle dolly zoom
		if (!enableDolly)
		{
			std::cout << "Dolly zoom on\n";
		}
		else
		{
			std::cout << "Dolly zoom off\n";
		}
		enableDolly = !enableDolly;
	}
	if (p_key == GLFW_KEY_C && p_action == GLFW_PRESS) {

		// Show/hide Checkerboard
		if (!showCheckerboard)
		{
			std::cout << "Show checkerboard\n";
		}
		else {
			std::cout << "Hide checkerboard\n";
		}
		showCheckerboard = !showCheckerboard;
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
	
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glEnable(GL_DEPTH_TEST);
	glShadeModel(GL_SMOOTH);
}

void printHotKeys() {
	std::cout << "\nHot Keys..\n"
		<< "Orthogonal Projection:  O\n"
		<< "Perspective Projection: P\n"
		<< "Toggle Spinning:        S\n"
		<< "Toggle Dolly Zoom:      D\n"
		<< "Show/hide Checkerboard: C\n"
		<< "Exit:                   Esc\n\n";
}

void clearModelViewMatrix()
{
	for (int i = 0; i < 4; ++i)
	{
		for (int j = 0; j < 4; ++j)
		{
			g_modelViewMatrix[4 * i + j] = 0.0f;
		}
	}
}

// The matrix for rotating the teapot was taken from this webpage: https://en.wikipedia.org/wiki/Transformation_matrix#Rotation
// I adapted it to 3D for rotating around the y-axis myself just by tinkering with it on paper.
void updateModelViewMatrix()
{	
	float theta;
	float cos_theta;
	float sin_theta;

	clearModelViewMatrix();

	if (teapotSpin)
	{
		theta = (M_PI / 2) * getTime(); // rotates pi/2 radians per second
		cos_theta = cos(theta);
		sin_theta = sin(theta);

		// rotate it by @theta radians
		g_modelViewMatrix[0] = cos_theta;
		g_modelViewMatrix[2] = -1 * sin_theta;
		g_modelViewMatrix[5] = 1.0f;
		g_modelViewMatrix[8] = sin_theta;
		g_modelViewMatrix[10] = cos_theta;
		g_modelViewMatrix[15] = 1.0f;

		g_modelViewMatrix[14] = -distance; // move it away from the camera by @distance
	}
	else
	{
		// do nothing (identity matrix)
		g_modelViewMatrix[0] = 1.0f;
		g_modelViewMatrix[5] = 1.0f;
		g_modelViewMatrix[10] = 1.0f;
		g_modelViewMatrix[15] = 1.0f;

		g_modelViewMatrix[14] = -distance; // move it away from the camera by @distance
	}
}

void setModelViewMatrix()
{
	glMatrixMode(GL_MODELVIEW);
	updateModelViewMatrix();
	glLoadMatrixf(g_modelViewMatrix);
}

void drawTeapot() {
	glBegin(GL_TRIANGLES);
	for (size_t f = 0; f < g_meshIndices.size(); ++f)
	{
		const float scale = 0.1f;
		const unsigned int idx = g_meshIndices[f];
		const float x = scale * g_meshVertices[3 * idx + 0];
		const float y = scale * g_meshVertices[3 * idx + 1];
		const float z = scale * g_meshVertices[3 * idx + 2];

		const float nx = g_meshNormals[3 * idx + 0];
		const float ny = g_meshNormals[3 * idx + 1];
		const float nz = g_meshNormals[3 * idx + 2];

		glNormal3f(nx, ny, nz);
		glVertex3f(x, y, z);
	}
	glEnd();
}
void renderTeapot() {
	drawTeapot();
}

void drawCheckerBoard() {
	int checkerCount = g_windowWidth;
	int checkerSize = (g_windowWidth) / checkerCount;

	glBegin(GL_QUADS);
	for (int i = 0; i < checkerCount; ++i) {
		for (int j = 0; j < checkerCount; ++j) {
			if ((i + j) % 2 == 0)
				glColor3f(0.0, 0.1, 1.0);
			else
				glColor3f(1.0, 0.0, 1.0);

			float x = i - checkerCount / 2; // to be between -1 and 1
			float z = j - checkerCount / 2;
			x *= checkerSize;
			z *= checkerSize;
			float y = -1.0f;
			glVertex3f(x, y, z);
			glVertex3f(x, y, z - checkerSize);
			glVertex3f(x + checkerSize, y, z - checkerSize);
			glVertex3f(x + checkerSize, y, z);
		}
	}
	glEnd();
}
void renderCheckerBoard() {

	/*
	/* If you want to keep checkerboard still while rotating the
	/* the teapot, you need to change the transformation for the
	/* checkerboard plane
	*/
	glMatrixMode(GL_MODELVIEW);
	clearModelViewMatrix();

	g_modelViewMatrix[0] = 1;
	g_modelViewMatrix[2] = 0;
	g_modelViewMatrix[5] = 1;
	g_modelViewMatrix[8] = 0;
	g_modelViewMatrix[10] = 1;
	g_modelViewMatrix[14] = -distance;
	g_modelViewMatrix[15] = 1.0f;
	
	glLoadMatrixf(g_modelViewMatrix);

	// Disable shading for the checkerboard
	glDisable(GL_LIGHTING);
	drawCheckerBoard();
	glEnable(GL_LIGHTING);
}

void render()
{
	togglePerspective();
	setModelViewMatrix();
	renderTeapot();
	if (showCheckerboard)
		renderCheckerBoard();
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

int main()
{
	initWindow();
	initGL();
	loadObj("data/teapot.obj");
	printHotKeys();
	renderLoop();
}
