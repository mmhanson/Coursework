#include <cstdlib>
#include <cstdio>
#include <cmath>
#include <fstream>
#include <vector>
#include <iostream>
#include <cassert>
#include <random>
#include <algorithm>
#include <Eigen>

using namespace Eigen;

#define MAX(a, b) ((a) > (b) ? (a) : (b))

// image background color
Vector3f bgcolor(1.0f, 1.0f, 1.0f);

// lights in the scene
std::vector<Vector3f> lightPositions = { Vector3f(0.0, 60, 60)
									   , Vector3f(-60.0, 60, 60)
									   , Vector3f(60.0, 60, 60) };

class Sphere
{
public:
	Vector3f center;  // position of the sphere
	float radius;  // sphere radius
	Vector3f surfaceColor; // surface color

	Sphere(
		const Vector3f &c,
		const float &r,
		const Vector3f &sc) :
		center(c), radius(r), surfaceColor(sc)
	{
	}

	// line vs. sphere intersection (note: this is slightly different from ray vs. sphere intersection!)
	bool intersect(const Vector3f &rayOrigin, const Vector3f &rayDirection, float &t0, float &t1) const
	{
		Vector3f l = center - rayOrigin;
		float tca = l.dot(rayDirection);
		if (tca < 0) return false;
		float d2 = l.dot(l) - tca * tca;
		if (d2 > (radius * radius)) return false;
		float thc = sqrt(radius * radius - d2);
		t0 = tca - thc;
		t1 = tca + thc;

		return true;
	}
};

// diffuse reflection model
Vector3f diffuse(const Vector3f &L, // direction vector from the point on the surface towards a light source
	const Vector3f &N, // normal at this point on the surface
	const Vector3f &diffuseColor,
	const float kd // diffuse reflection constant
)
{
	Vector3f resColor = Vector3f::Zero();

	// note: only called if light ray is not intersected
	resColor = resColor + 0.33f * kd * MAX(L.dot(N), 0) * diffuseColor;

	return resColor;
}

// Phong reflection model
Vector3f phong(const Vector3f &L, // direction vector from the point on the surface towards a light source
	const Vector3f &N, // normal at this point on the surface
	const Vector3f &V, // direction pointing towards the viewer
	const Vector3f &diffuseColor,
	const Vector3f &specularColor,
	const float kd, // diffuse reflection constant
	const float ks, // specular reflection constant
	const float alpha) // shininess constant
{
	Vector3f resColor = Vector3f::Zero();
	Vector3f reflection;

	// TODO: implement Phong shading model
	reflection = 2 * N * (N.dot(L)) - L;
	resColor = 0.33f * specularColor * (ks * pow(MAX(reflection.dot(V), 0.0f), alpha));

	return resColor;
}

Vector3f calc_surface_normal(Sphere sphere, Vector3f surfacePoint)
{
	Vector3f surface_normal(0.0f, 0.0f, 0.0f);
	surface_normal = surfacePoint - sphere.center;
	surface_normal.normalize();

	return surface_normal;
}

Vector3f calc_lighting(
	const Vector3f &point,
	const Vector3f &anti_ray_dir,
	const std::vector <Sphere> &spheres,
	Sphere sphere,
	const std::vector <Vector3f> &light_positions)
{
	Vector3f pixelColor(0.0f, 0.0f, 0.0f);

	for (int idx = 0; idx < light_positions.size(); idx++)
	{
		Vector3f light_position = light_positions.at(idx);
		Vector3f light_direction = light_position - point;
		light_direction.normalize();

		bool intersect_flag = false;
		for (int s_idx = 0; s_idx < spheres.size(); s_idx++)
		{
			Sphere iter_sphere = spheres.at(s_idx);
			float t0;
			float t1;

			if (iter_sphere.intersect(point, light_direction, t0, t1))
			{
				intersect_flag = true;
				break;
			}
		}
		if (!intersect_flag)
		{
			//pixelColor = pixelColor + (0.33f * sphere.surfaceColor);
			Vector3f surfaceNormal = calc_surface_normal(sphere, point);
			float kd = 1.0f;
			float ks = 3.0f;
			float alpha = 100.0f;

			pixelColor = pixelColor + diffuse(light_direction, surfaceNormal, sphere.surfaceColor, kd);
			pixelColor = pixelColor + phong(light_direction, surfaceNormal, anti_ray_dir,
				sphere.surfaceColor, Vector3f::Ones(), kd, ks, alpha);
		}
	}

	return pixelColor;
}

Vector3f trace(
	const Vector3f &rayOrigin,
	const Vector3f &rayDirection,
	const std::vector<Sphere> &spheres)
{
	Vector3f pixelColor(1.0f, 0.0f, 0.0f);
	Vector3f anti_ray_dir;

	anti_ray_dir = rayDirection * -1;
	anti_ray_dir.normalize();

	for (int idx = 0; idx < spheres.size(); idx++)
	{
		float t0; // first point of intersection (along ray)
		float t1;
		Sphere sphere = spheres.at(idx);

		if (sphere.intersect(rayOrigin, rayDirection, t0, t1))
		{
			Vector3f point = rayOrigin + (rayDirection * t0);
			return calc_lighting(point, anti_ray_dir, spheres, sphere, lightPositions);
		}
	}
	return bgcolor;
}

void render(const std::vector<Sphere> &spheres)
{
  unsigned width = 640;
  unsigned height = 480;
  Vector3f *image = new Vector3f[width * height];
  Vector3f *pixel = image;
  float invWidth  = 1 / float(width);
  float invHeight = 1 / float(height);
  float fov = 30;
  float aspectratio = width / float(height);
	float angle = tan(M_PI * 0.5f * fov / 180.f);
	
	// Trace rays
	for (unsigned y = 0; y < height; ++y) 
	{
		for (unsigned x = 0; x < width; ++x) 
		{
			float rayX = (2 * ((x + 0.5f) * invWidth) - 1) * angle * aspectratio;
			float rayY = (1 - 2 * ((y + 0.5f) * invHeight)) * angle;
			Vector3f rayDirection(rayX, rayY, -1);
			rayDirection.normalize();
			*(pixel++) = trace(Vector3f::Zero(), rayDirection, spheres);
		}
	}
	
	// Save result to a PPM image
	std::ofstream ofs("./render.ppm", std::ios::out | std::ios::binary);
	ofs << "P6\n" << width << " " << height << "\n255\n";
	for (unsigned i = 0; i < width * height; ++i) 
	{
		const float x = image[i](0);
		const float y = image[i](1);
		const float z = image[i](2);

		ofs << (unsigned char)(std::min(float(1), x) * 255) 
			  << (unsigned char)(std::min(float(1), y) * 255) 
			  << (unsigned char)(std::min(float(1), z) * 255);
	}
	
	ofs.close();
	delete[] image;
}

int main(int argc, char **argv)
{
	std::vector<Sphere> spheres;
	// position, radius, surface color
	spheres.push_back(Sphere(Vector3f(0.0, 0, -20), 4, Vector3f(1.00, 0.32, 0.36)));
	spheres.push_back(Sphere(Vector3f(5.0, -1, -15), 2, Vector3f(0.90, 0.76, 0.46)));
	spheres.push_back(Sphere(Vector3f(5.0, 0, -25), 3, Vector3f(0.65, 0.77, 0.97)));
	spheres.push_back(Sphere(Vector3f(-5.5, 0, -13), 3, Vector3f(0.90, 0.90, 0.90)));
	spheres.push_back(Sphere(Vector3f(0.0, -10004, -20), 10000, Vector3f(0.50, 0.50, 0.50)));

	render(spheres);

	return 0;
}
