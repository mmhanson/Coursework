This is a custom ffmpeg codec (.cool) and a driver that uses this codec to create an animation of a bouncing ball. The codec is very basic, using only basic compression techniques. But it is still very effective, yielding less than one byte per pixel in the encoded image.

I worked on this project in March 2019 for the CS 3505 (Software Practice II) class at the University of Utah in a team of 2, including myself.

To build:
1. Download dependencies: `sudo apt-get install -y libavformat-dev libswscale-dev libavutil-dev`

2. Download and patch FFmpeg. Note: you may need to download an older release, near March 2019 for the patch to work. `git clone git@github.com:FFmpeg/FFmpeg.git && cp -rf ./ffmpeg-patch/* ./FFmpeg`

3. Compile ffmpeg: `cd FFmpeg && ./configure && make`

4. Compile the bouncer: `cd ../bouncer && make`
