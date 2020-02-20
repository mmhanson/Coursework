# build the application into an executable named 'bouncer'
all: bouncer.cpp
	g++ -Wno-deprecated-declarations -g -o bouncer bouncer.cpp -D__STDC_CONSTANT_MACROS -I ../ffmpeg/include `pkg-config --cflags --libs libavformat libswscale libavutil`


clean:
	rm -f bouncer
	rm -f *.o
	rm -f *.cool
	rm -f *.mp4

# Use ffmpeg to assemble the 300 frames of the movie as individual files to a movie named 'movie.mp4'
movie: 
	ffmpeg -start_number 0 -i frame%3d.cool -vcodec mpeg4 movie.mp4

