#include "pch.h"
#include <windows.h>

extern "C" __declspec(dllexport) void ProcessImageCpp(
unsigned char* pData,
int width,
int height,
int stride,
unsigned char keyR,
unsigned char keyG,
unsigned char keyB,
int tolerance
)
{ 
	int toleranceSquared = tolerance * tolerance;

    for (int y = 0; y < height; y++) {
		unsigned char* row = pData + y * stride;

        for (int x = 0; x < width; x++) {
            int index = x * 4;

            unsigned char b = row[index];
			unsigned char g = row[index + 1];
			unsigned char r = row[index + 2];

            int diffR = (int)r - keyR;
            int diffG = (int)g - keyG;
            int diffB = (int)b - keyB;

            int distanceSquared = diffR * diffR + diffG * diffG + diffB * diffB;

            if (distanceSquared <= toleranceSquared) {
                row[index + 3] = 0;
            }
        }
    }
}
